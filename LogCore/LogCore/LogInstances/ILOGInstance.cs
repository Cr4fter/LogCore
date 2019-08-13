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

namespace LogCore
{
    public interface ILOGInstance : ILogInstance, IDisposable
    {
        LogSeverity SeverityFilter { get; }

        void SetLogFilter(LogSeverity severity);

        /// <summary>
        /// Creates a LogMessage instance with the severity <see cref="LogSeverity.Fatal"/>
        /// </summary>
        /// <param name="message">the message of the log</param>
        /// <param name="tag">the log to categorize the message. if omitted Fatal is used</param>
        void Fatal(string message, string tag = "Fatal");

        /// <summary>
        /// Creates a LogMessage instance with the severity <see cref="LogSeverity.Error"/>
        /// </summary>
        /// <param name="message">the message of the log</param>
        /// <param name="tag">the log to categorize the message. if omitted Fatal is used</param>
        void Error(string message, string tag = "Error");

        /// <summary>
        /// Creates a LogMessage instance with the severity <see cref="LogSeverity.Warning"/>
        /// </summary>
        /// <param name="message">the message of the log</param>
        /// <param name="tag">the log to categorize the message. if omitted Fatal is used</param>
        void Warning(string message, string tag = "Warn");

        /// <summary>
        /// Creates a LogMessage instance with the severity <see cref="LogSeverity.Info"/>
        /// </summary>
        /// <param name="message">the message of the log</param>
        /// <param name="tag">the log to categorize the message. if omitted Fatal is used</param>
        void Info(string message, string tag = "INFO");

        /// <summary>
        /// Creates a LogMessage instance with the severity <see cref="LogSeverity.Debug"/>
        /// </summary>
        /// <param name="message">the message of the log</param>
        /// <param name="tag">the log to categorize the message. if omitted Fatal is used</param>
        void Debug(string message, string tag = "DBG");

        /// <summary>
        /// Creates a LogMessage instance with the severity <see cref="LogSeverity.Fatal"/>
        /// </summary>
        /// <param name="message">the message of the log</param>
        /// <param name="tag">the log to categorize the message. if omitted Fatal is used</param>
        void Trace(string message, string tag = "TRACE");

        /// <summary>
        /// Handles a externaly created <see cref="LOGMessage"/>
        /// </summary>
        /// <param name="message">the instance to be handled by the log system</param>
        void LogMessage(LOGMessage message);
    }
}
