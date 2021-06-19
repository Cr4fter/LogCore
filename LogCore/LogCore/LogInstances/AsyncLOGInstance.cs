#region License
// Copyright 2019 Noah Forberich
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using LogCore.Outputters;

namespace LogCore.LogInstances
{
    public class AsyncLOGInstance : ILOGInstance
    {
        private readonly List<ILogOutput> _outputters = new List<ILogOutput>();
        readonly Dictionary<string, string> _additionalFields = new Dictionary<string, string>();
        private readonly bool _singleton;

        private readonly List<LOGMessage> _messageQueue = new List<LOGMessage>();

        private bool _shouldRun;
        private readonly Thread _workerThread;
        private readonly SyncType _synchroniser;
        private readonly EventWaitHandle _waiter;
        private bool _handlingInProgress = false;

        /// <inheritdoc />
        public override LogSeverity SeverityFilter { get; internal set; } = LogSeverity.Trace;

        public AsyncLOGInstance(bool spawnWorkerThread = true, SyncType synchroniser = SyncType.None, bool singletonMode = false, IEnumerable<ILogOutput> outputters = null, IEnumerable<KeyValuePair<string, string>> additionalFields = null)
        {
            //Setting up the Singleton if requested by the user
            if (singletonMode)
            {
                _singleton = true;
                LOG.SetupSingleton(this);
            }

            //Coppying the requested output types or creating a new default Console logger
            if (outputters == null)
            {
                _outputters.Add(new ConsoleOutputter());
            }
            else
            {
                _outputters.AddRange(outputters);
            }
            //Initializing the Log OutPutters
            foreach (ILogOutput outputter in _outputters)
            {
                outputter.Initialize();
            }
            //setting up the Worker thread if requested by the user
            _synchroniser = synchroniser;
            if (spawnWorkerThread)
            {
                _shouldRun = true;
                _workerThread = new Thread(ThreadRunner);
                _waiter = new EventWaitHandle(false, EventResetMode.AutoReset);
                _workerThread.Start();
            }
            //Prepairing the additional fields witch get added to every logMessage
            if (additionalFields == null) return;
            foreach (KeyValuePair<string, string> additionalField in additionalFields)
            {
                _additionalFields.Add(additionalField.Key, additionalField.Value);
            }
        }

        /// <summary>
        /// The function executed by the Worker thread witch handles all available messages and the waits in the defined way for new messages
        /// </summary>
        private void ThreadRunner()
        {
            while (_shouldRun)
            {
                ProcessMessages();

                switch (_synchroniser)
                {
                    case SyncType.None:
                        break;
                    case SyncType.Sleep1:
                        Thread.Sleep(1000);
                        break;
                    case SyncType.Sleep5:
                        Thread.Sleep(5000);
                        break;
                    case SyncType.EventWaitHandle:
                        _waiter.WaitOne();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// this function processes all available LogMessages up to a user defined limit.
        /// </summary>
        /// <param name="maxMessages">the amount of messages to process. -1 to process all found messages </param>
        /// <returns>the amount of processed messages</returns>
        public int ProcessMessages(int maxMessages = -1)
        {
            int counter = 0;
            LOGMessage currMessage;
            _handlingInProgress = true;
            for (int i = 0; i != maxMessages; i++)
            {
                lock (_messageQueue)
                {
                    if (_messageQueue.Count == 0)
                    {
                        _handlingInProgress = false;
                        return counter;
                    }
                    currMessage = _messageQueue[0];
                    _messageQueue.RemoveAt(0);
                }
                foreach (ILogOutput outputter in _outputters)
                {
                    outputter.HandleMessage(currMessage);
                }

                counter++;
            }

            _handlingInProgress = false;
            return counter;
        }
        /// <summary>
        /// the function used to insert new Messages into the queue
        /// </summary>
        /// <param name="message">the message to be handled</param>
        public void HandleMessage(LOGMessage message)
        {
            if (message.LogSeverity > SeverityFilter)
            {
                return;
            }
            foreach (KeyValuePair<string, string> additionalField in _additionalFields)
            {
                message.AdditionalFields.Add(additionalField.Key, additionalField.Value);
            }

            lock (_messageQueue)
            {
                _messageQueue.Add(message);
            }

            _waiter.Set();
        }

        /// <summary>
        /// Clears up the singleton if it was setup.
        /// shuts down the WorkerThread if it was started.
        /// clears up the outputters.
        /// </summary>
        public override void Dispose()
        {
            if (_singleton)
            {
                LOG.ClearSingleton();
            }

            while (_handlingInProgress) { }
            if (_workerThread != null)
            {
                //Signaling thread to shut down
                _shouldRun = false;
                //Making sure All messages got handled
                _waiter.Set();
                //Waiting for the thread to complete its work
                _workerThread.Join();
            }
            //handling all messages missed by the worker thread
            ProcessMessages();
            foreach (ILogOutput outputter in _outputters)
            {
                outputter.Dispose();
            }
        }

        /// <inheritdoc />
        public override void SetLogFilter(LogSeverity severity)
        {
            SeverityFilter = severity;
        }

        /// <inheritdoc/>
        public override void Fatal(string message, string tag = "Fatal")
        {
            LOGMessage logMessage = new LOGMessage(message, tag, LogSeverity.Fatal);
            HandleMessage(logMessage);
        }
        /// <inheritdoc/>
        public override void Error(string message, string tag = "Error")
        {
            LOGMessage logMessage = new LOGMessage(message, tag, LogSeverity.Error);
            HandleMessage(logMessage);
        }

        /// <inheritdoc/>
        public override void Warning(string message, string tag = "Warn")
        {
            LOGMessage logMessage = new LOGMessage(message, tag, LogSeverity.Warning);
            HandleMessage(logMessage);
        }

        /// <inheritdoc/>
        public override void Info(string message, string tag = "INFO")
        {
            LOGMessage logMessage = new LOGMessage(message, tag, LogSeverity.Info);
            HandleMessage(logMessage);
        }

        /// <inheritdoc/>
        public override void Debug(string message, string tag = "DBG")
        {
            LOGMessage logMessage = new LOGMessage(message, tag, LogSeverity.Debug);
            HandleMessage(logMessage);
        }

        /// <inheritdoc/>
        public override void Trace(string message, string tag = "TRACE")
        {
            LOGMessage logMessage = new LOGMessage(message, tag, LogSeverity.Trace);
            HandleMessage(logMessage);
        }

        /// <inheritdoc/>
        public override void LogMessage(LOGMessage message)
        {
            HandleMessage(message);
        }
    }

    /// <summary>
    /// defines how the Message Handler thread waits between handling queued messages
    /// </summary>
    public enum SyncType
    {
        /// <summary>
        /// The Worker thread constantly checks if new messages arrived.
        /// This should be only used for short lived Loggers handling an extreme amount of messages.
        /// </summary>
        None,
        /// <summary>
        /// The Worker thread handles all queued messages and then goes to sleep for 1 seconds.
        /// </summary>
        Sleep1,
        /// <summary>
        /// The Worker thread handles all queued messages and then goes to sleep for 5 seconds.
        /// </summary>
        Sleep5,
        /// <summary>
        /// a <see cref="EventWaitHandle"/> is used to let the thread only run when messages are available.
        /// in most cases this should be used since the overhead is extremely small.
        /// </summary>
        EventWaitHandle
    }
}
