#region License
// Copyright 2019 Noah Forberich
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System.IO;
using System.Text;

namespace LogCore.Outputters
{
    /// <summary>
    /// This Outputter Writes all Log Messages to a file.
    /// </summary>
    public class FileOutputter : ILogOutput
    {
        private FileStream _logFileStream;
        private readonly FileInfo _outputPath;
        private readonly Encoding _fileEncoding;

        /// <summary>
        /// Creates an instance of <see cref="FileOutputter"/>.
        /// This step does not open/create an output file this is donne by <see cref="Initialize"/>
        /// </summary>
        /// <param name="outputPath">a FileInfo Object containing the path where the log files will be outputted</param>
        /// <param name="fileEncoding">The encoding used to convert the message to bytes. if this argument is omited or null is passed <see cref="Encoding.UTF8"/> is used as default.</param>
        public FileOutputter(FileInfo outputPath, Encoding fileEncoding = null)
        {
            _outputPath = outputPath;
            _fileEncoding = fileEncoding ?? Encoding.UTF8;
        }

        /// <inheritdoc />
        public void Initialize()
        {
            _logFileStream = _outputPath.Open(FileMode.Append);
        }

        /// <inheritdoc />
        public void HandleMessage(LOGMessage message)
        {
            if (!_logFileStream.CanWrite) return;
            byte[] messageBytes = _fileEncoding.GetBytes($"{message}\n");
            _logFileStream.Write(messageBytes, 0, messageBytes.Length);
        }

        /// <summary>
        /// This Disposes the Logfile stream while making sure that everything is written to disk.
        /// </summary>
        public void Dispose()
        {
            _logFileStream.Flush(true);
            _logFileStream.Dispose();
        }
    }
}
