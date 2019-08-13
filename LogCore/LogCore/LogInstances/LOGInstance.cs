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
using LogCore.Exceptions;
using LogCore.Outputters;

namespace LogCore
{
    public class LOGInstance : ILOGInstance
    {
        private readonly List<ILogOutput> _outputter = new List<ILogOutput>();
        private readonly Dictionary<string, string> _additionalFields = new Dictionary<string, string>();
        private readonly bool _disposeSingleton;

        /// <inheritdoc />
        public LogSeverity SeverityFilter { get; private set; } = LogSeverity.Trace;

        /// <summary>
        /// initializes the Logging system with defined outputters
        /// </summary>
        /// <param name="staticInstance">
        /// defines weather this log instance will be made to an singleton.
        /// Only one singleton(staticInstance) can exist otherwise an <see cref="AlreadyInitializedException"/> is thrown.
        /// </param>
        /// <param name="outputs">when left empty Log messages will be displayed only to the console</param>
        /// <param name="additionalFields">all <seealso cref="KeyValuePair{TKey,TValue}"/> will get added to every LogMessage processed.</param>
        public LOGInstance(bool staticInstance = false, IEnumerable<ILogOutput> outputs = null, IEnumerable<KeyValuePair<string, string>> additionalFields = null)
        {
            _disposeSingleton = staticInstance;
            if (staticInstance)
            {
                LOG.SetupSingleton(this);
            }

            if (additionalFields != null)
            {
                foreach (KeyValuePair<string, string> additionalField in additionalFields)
                {
                    _additionalFields.Add(additionalField.Key, additionalField.Value);
                }
            }

            if (outputs == null)
            {
                ConsoleOutputter co = new ConsoleOutputter();
                co.Initialize();
                _outputter.Add(co);
                return;
            }

            foreach (ILogOutput logOutput in outputs)
            {
                logOutput.Initialize();
                _outputter.Add(logOutput);
            }
        }

        /// <summary>
        /// forwards message to all outputters
        /// </summary>
        /// <param name="message"></param>
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

            lock (_outputter)
            {
                foreach (ILogOutput logOutput in _outputter)
                {
                    logOutput.HandleMessage(message);
                }
            }
        }

        /// <summary>Shuts down all Outputters.</summary>
        public void Dispose()
        {
            if (_disposeSingleton)
            {
                LOG.ClearSingleton();
            }
            foreach (ILogOutput logOutput in _outputter)
            {
                try
                {
                    logOutput.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public void SetLogFilter(LogSeverity severity)
        {
            SeverityFilter = severity;
        }

        /// <inheritdoc/>
        public void Fatal(string message, string tag = "Fatal")
        {
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Fatal);
            HandleMessage(msg);
        }

        /// <inheritdoc/>
        public void Error(string message, string tag = "Error")
        {
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Error);
            HandleMessage(msg);
        }

        /// <inheritdoc/>
        public void Warning(string message, string tag = "Warn")
        {
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Warning);
            HandleMessage(msg);
        }

        /// <inheritdoc/>
        public void Info(string message, string tag = "INFO")
        {
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Info);
            HandleMessage(msg);
        }

        /// <inheritdoc/>
        public void Debug(string message, string tag = "DBG")
        {
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Debug);
            HandleMessage(msg);
        }

        /// <inheritdoc/>
        public void Trace(string message, string tag = "TRACE")
        {
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Trace);
            HandleMessage(msg);
        }

        /// <inheritdoc/>
        public void LogMessage(LOGMessage message)
        {
            HandleMessage(message);
        }
    }
}
