#region License
// Copyright 2019 Noah Forberich
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using LogCore;
using NUnit.Framework;

namespace UnitTest
{
    public class UnitTest4
    {
        private static LogSeverity _testingSeverity;
        private static string _testingMessage;

        [Test]
        public void DefaultFilter()
        {
            _testingMessage = "msg";
            using (ILOGInstance logInstance = new LOGInstance(false, new []{new TestOutputter()}))
            {
                _testingSeverity = LogSeverity.Trace;
                logInstance.Trace("msg");

                _testingSeverity = LogSeverity.Debug;
                logInstance.Debug("msg");

                _testingSeverity = LogSeverity.Info;
                logInstance.Info("msg");

                _testingSeverity = LogSeverity.Warning;
                logInstance.Warning("msg");

                _testingSeverity = LogSeverity.Error;
                logInstance.Error("msg");

                _testingSeverity = LogSeverity.Fatal;
                logInstance.Fatal("msg");
            }
        }

        [Test]
        public void InfoFilter()
        {
            _testingMessage = "msg";
            using (ILOGInstance logInstance = new LOGInstance(false, new[] { new TestOutputter() }))
            {
                logInstance.SetLogFilter(LogSeverity.Info);
                _testingSeverity = LogSeverity.Fatal;
                logInstance.Trace("msg");

                _testingSeverity = LogSeverity.Fatal;
                logInstance.Debug("msg");

                _testingSeverity = LogSeverity.Info;
                logInstance.Info("msg");

                _testingSeverity = LogSeverity.Warning;
                logInstance.Warning("msg");

                _testingSeverity = LogSeverity.Error;
                logInstance.Error("msg");

                _testingSeverity = LogSeverity.Fatal;
                logInstance.Fatal("msg");
            }
        }

        class TestOutputter : ILogOutput
        {
            public void Dispose() { }

            public void Initialize() { }

            public void HandleMessage(LOGMessage message)
            {
                Assert.AreEqual(_testingSeverity, message.LogSeverity);
                Assert.AreEqual(_testingMessage, message.Message);
            }
        }
    }
}