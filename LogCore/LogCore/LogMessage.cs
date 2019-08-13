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

namespace LogCore
{
    public class LOGMessage
    {
        /// <summary>
        /// Creates a LOGMessage instance with only a message.
        /// the TAG is set to INFO.
        /// the LogSeverity is set to INFO.
        /// </summary>
        /// <param name="message">The message to be outputted</param>
        public LOGMessage(string message)
        {
            Message = message;
            TAG = "INFO";
            Occurence = DateTime.Now;
            LogSeverity = LogSeverity.Info;
        }
        /// <summary>
        /// Creates a LOGMessage using a Message and an Tag
        /// the LogSeverity is drt to INFO.
        /// </summary>
        /// <param name="message">The message to be outputted</param>
        /// <param name="tag">The tag to display the message under</param>
        public LOGMessage(string message, string tag)
        {
            Message = message;
            TAG = tag;
            Occurence = DateTime.Now;
            LogSeverity = LogSeverity.Info;
        }
        /// <summary>
        /// Creates a LOGMessage using a Message, Tag and LogSeverity
        /// </summary>
        /// <param name="message">The message to be outputted</param>
        /// <param name="tag">The tag to display the message under</param>
        /// <param name="logSeverity"><see cref="LogSeverity"/></param>
        public LOGMessage(string message, string tag, LogSeverity logSeverity)
        {
            Message = message;
            TAG = tag;
            Occurence = DateTime.Now;
            LogSeverity = logSeverity;
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string output = $"[{DateTime.Now}] {TAG.ToUpper()} - '{LogSeverity.ToString()}: {Message}'";
            foreach (KeyValuePair<string, string> additionalField in AdditionalFields)
            {
                output += $" - '{additionalField.Key}' : '{additionalField.Value}'";
            }
            return output;
        }
        /// <summary>
        /// Returns the <seealso cref="ConsoleColor"/> class to display the message under.
        /// </summary>
        /// <returns><see cref="ConsoleColor"/></returns>
        public ConsoleColor GetSeverityColor()
        {
            return GetSeverityColor(LogSeverity);
        }
        /// <summary>
        /// Maps Message Severitys to an <see cref="ConsoleColor"/>.
        /// this function is internally used by <see cref="GetSeverityColor()"/>
        /// </summary>
        /// <param name="sv">the Severity</param>
        /// <returns><see cref="ConsoleColor"/></returns>
        public static ConsoleColor GetSeverityColor(LogSeverity sv)
        {
            switch (sv)
            {
                case LogSeverity.Fatal:
                    return ConsoleColor.DarkRed;
                case LogSeverity.Error:
                    return ConsoleColor.Red;
                case LogSeverity.Warning:
                    return ConsoleColor.Yellow;
                case LogSeverity.Info:
                    return ConsoleColor.DarkGreen;
                case LogSeverity.Debug:
                    return ConsoleColor.DarkGray;
                case LogSeverity.Trace:
                    return ConsoleColor.Gray;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sv), sv, null);
            }
        }
        /// <summary>
        /// the time at witch this message was logged
        /// </summary>
        public DateTime Occurence;
        /// <summary>
        /// the tag used to organize LogMessages
        /// </summary>
        public string TAG;
        /// <summary>
        /// The message Content
        /// </summary>
        public string Message;
        /// <summary>
        /// The severity of the message
        /// </summary>
        public LogSeverity LogSeverity;
        /// <summary>
        /// this Can be used to append more information to a log message.
        /// Ex. use to differ between different sessions in multiple threads.
        /// </summary>
        public Dictionary<string, string> AdditionalFields = new Dictionary<string, string>();
    }
}
