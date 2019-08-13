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
using System.Text;

namespace LogCore.Outputters
{
    /// <summary>
    /// This Outputter Displays all messages to the console
    /// </summary>
    public class ConsoleOutputter : ILogOutput
    {
        /// <summary>
        /// The Amount of spaces used to fill the LogTag
        /// </summary>
        public int LOGTagLength;

        /// <summary>
        /// this creates an instance of <see cref="ConsoleOutputter"/>
        /// </summary>
        /// <param name="logTagLength">The Tag Length defaults to 9 chars which align most of the log tags without taking to much space in one console line.</param>
        public ConsoleOutputter(int logTagLength = 9)
        {
            LOGTagLength = logTagLength;
        }

        /// <inheritdoc />
        public void Initialize()
        {
        }

        /// <inheritdoc />
        public void HandleMessage(LOGMessage message)
        {
            Console.Write($"[{message.Occurence}] ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            StringBuilder writeTag = new StringBuilder();
            try
            {
                if (message.TAG.Length < LOGTagLength)
                {
                    for (int i = 0; i < LOGTagLength - message.TAG.Length; i++)
                    {
                        writeTag.Append(" ");
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            writeTag.Append(message.TAG);

            Console.Write(writeTag + ": ");
            Console.ForegroundColor = message.GetSeverityColor();
            Console.WriteLine(message.Message);
            Console.ResetColor();
        }

        /// <summary>
        /// This function is left empty since no resources have to be released.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
