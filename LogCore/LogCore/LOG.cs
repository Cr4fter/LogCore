#region License
// Copyright 2019 Noah Forberich
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System.Diagnostics;
using LogCore.Exceptions;

namespace LogCore
{
    public static class LOG
    {
        /// <summary>
        /// If no System was Setup Earlier a new Default Console Logger will be created.
        /// </summary>
        private static void InitializeIfNotSetup()
        {
            if (Instance == null)
            {
                isDefaultLogger = true;
                Instance = new LOGInstance();
            }
        }

        /// <summary>
        /// Clears a default logger that might have implicitly be created and setsup the passed instance as the singleton.
        /// </summary>
        /// <param name="instance"></param>
        public static void SetupSingleton(ILOGInstance instance)
        {
            if (Instance != null)
            {
                if (isDefaultLogger)
                {
                    Instance.Dispose();
                }
                else
                {
                    throw new AlreadyInitializedException();
                }
            }
            Instance = instance;
            isDefaultLogger = false;
        }

        public static void ClearSingleton()
        {
            if (Instance == null) throw new NotInitializedException();
            Instance = null;
            isDefaultLogger = false;
        }

        public static ILOGInstance GetSingleton()
        {
            return Instance;
        }

        internal static ILOGInstance Instance;
        internal static bool isDefaultLogger;
        public static void Fatal(string message, string tag = "Fatal")
        {
            InitializeIfNotSetup();
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Fatal);
            Instance.HandleMessage(msg);
        }

        public static void Error(string message, string tag = "Error")
        {
            InitializeIfNotSetup();
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Error);
            Instance.HandleMessage(msg);
        }

        public static void Warning(string message, string tag = "Warn")
        {
            InitializeIfNotSetup();
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Warning);
            Instance.HandleMessage(msg);
        }

        public static void Info(string message, string tag = "INFO")
        {
            InitializeIfNotSetup();
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Info);
            Instance.HandleMessage(msg);
        }

        /// <summary>
        /// Logs a Trace Message only when the calling binary was build with the DEBUG Constant.
        /// </summary>
        /// <param name="message">the Debug Message</param>
        /// <param name="tag">Defaults to DBG</param>
        [Conditional("DEBUG")]
        public static void Debug(string message, string tag = "DBG")
        {
            InitializeIfNotSetup();
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Debug);
            Instance.HandleMessage(msg);
        }

        /// <summary>
        /// Logs a Trace Message only when the calling binary was build with the TRACE Constant.
        /// </summary>
        /// <param name="message">the Trace Message</param>
        /// <param name="tag">Defaults to TRACE</param>
        [Conditional("TRACE")]
        public static void Trace(string message, string tag = "TRACE")
        {
            InitializeIfNotSetup();
            LOGMessage msg = new LOGMessage(message, tag, LogSeverity.Trace);
            Instance.HandleMessage(msg);
        }
    }
}
