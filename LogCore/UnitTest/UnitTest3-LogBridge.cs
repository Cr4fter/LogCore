#region License
// Copyright 2019 Noah Forberich
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System.Collections.Generic;
using LogCore;
using LogCore.LogInstances;
using LogCore.Outputters;
using NUnit.Framework;

namespace UnitTest
{
    public class UnitTest3
    {
        private static LogSeverity _testingSeverity;
        private static string _testingTag;
        private static string _testingMessage;
        private static Dictionary<string, string> _testingAdditionalfields;

        [SetUp]
        public void Setup()
        {
            _testingAdditionalfields = new Dictionary<string, string>();
        }
        [Test]
        public void LogBridge()
        {
            using (ILOGInstance parent = new LOGInstance(false, new[] { new TestOutputter() }, new[] { new KeyValuePair<string, string>("SessionID", "TestSession Parent") }))
            {
                using (ILOGInstance child = new AsyncLOGInstance(true, SyncType.EventWaitHandle, false, new[] { new LOGBridgeOutputter(parent) }, new []{new KeyValuePair<string, string>("testKey1", "TestSession Child") }))
                {
                    _testingMessage = "test message info";
                    _testingTag = "testTag";
                    _testingSeverity = LogSeverity.Info;

                    _testingAdditionalfields.Add("SessionID", "TestSession Parent");
                    _testingAdditionalfields.Add("testKey1", "TestSession Child");

                    child.Info(_testingMessage, _testingTag);
                }
            }
        }

        class TestOutputter : ILogOutput
        {
            public void Dispose() { }

            public void Initialize() { }

            public void HandleMessage(LOGMessage message)
            {
                Assert.AreEqual(_testingSeverity, message.LogSeverity);
                Assert.AreEqual(_testingTag, message.TAG);
                Assert.AreEqual(_testingMessage, message.Message);
                Assert.True(message.AdditionalFields.Count >= _testingAdditionalfields.Count);
                foreach (KeyValuePair<string, string> messageAdditionalField in message.AdditionalFields)
                {
                    Assert.AreEqual(_testingAdditionalfields[messageAdditionalField.Key], messageAdditionalField.Value);
                }
            }
        }
    }
}