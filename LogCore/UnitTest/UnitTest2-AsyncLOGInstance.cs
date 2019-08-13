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
using System.IO;
using LogCore;
using LogCore.Exceptions;
using LogCore.LogInstances;
using LogCore.Outputters;
using NUnit.Framework;

namespace UnitTest
{
    public class AsyncAsyncLoggerTests
    {
        private FileInfo _outputFile;
        private static LogSeverity _testingSeverity;
        private static string _testingTag;
        private static string _testingMessage;
        private static Dictionary<string, string> _testingAdditionalfields;
        [SetUp]
        public void Setup()
        {
            _outputFile = new FileInfo("testfile.txt");
            _testingAdditionalfields = new Dictionary<string, string>();
        }

        [Test]
        public void StaticFatal()
        {
            Assert.Throws<NotInitializedException>(() => LOG.Fatal("FATAL Message"));
        }
        [Test]
        public void StaticError()
        {
            Assert.Throws<NotInitializedException>(() => LOG.Error("ERROR Message"));
        }
        [Test]
        public void StaticWarning()
        {
            Assert.Throws<NotInitializedException>(() => LOG.Warning("WARNING Message"));
        }
        [Test]
        public void StaticInfo()
        {
            Assert.Throws<NotInitializedException>(() => LOG.Info("INFO Message"));
        }
        [Test]
        public void StaticDebug()
        {
            Assert.Throws<NotInitializedException>(() => LOG.Debug("DEBUG Message"));
        }
        [Test]
        public void StaticTrace()
        {
            Assert.Throws<NotInitializedException>(() => LOG.Trace("trace Message"));
        }
        [Test]
        public void FileOutput()
        {
            using (AsyncLOGInstance instance = new AsyncLOGInstance( true, SyncType.EventWaitHandle, false, new[] { new FileOutputter(_outputFile) }, new []{new KeyValuePair<string, string>("SessionID", "TestSession")}))
            {
                _testingAdditionalfields.Add("SessionID", "TestSession");
                instance.Debug("TestCase1");
            }

            using (StreamReader sr = new StreamReader(_outputFile.OpenRead()))
            {
                string content = sr.ReadToEnd();
                Assert.AreEqual(2, content.Split('\n').Length);
            }

            using (AsyncLOGInstance instance = new AsyncLOGInstance(true, SyncType.EventWaitHandle, false, new[] { new FileOutputter(_outputFile) }, new[] { new KeyValuePair<string, string>("SessionID", "TestSession") }))
            {
                instance.Debug("TestCase2");
            }

            using (StreamReader sr = new StreamReader(_outputFile.OpenRead()))
            {
                string content = sr.ReadToEnd();
                Assert.AreEqual( 3, content.Split('\n').Length);
            }
        }
        [Test]
        public void StaticLogTest()
        {
            try
            {
                LOG.Error("this should not work");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(NotInitializedException), e.GetType());
            }
            using (new AsyncLOGInstance(true, SyncType.EventWaitHandle, true, new[] { new TestOutputter() }))
            {
                _testingSeverity = LogSeverity.Error;
                _testingMessage = "test Message";
                _testingTag = "TTag";
                LOG.Error(_testingMessage, _testingTag);
            }

            _testingSeverity = LogSeverity.Debug;
            _testingMessage = null;
            _testingTag = null;
            try
            {
                LOG.Error("this should not work");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(NotInitializedException), e.GetType());
            }
        }

        [Test]
        public void Fatal()
        {
            using (ILOGInstance instance = new AsyncLOGInstance(true, SyncType.EventWaitHandle, true, new []{new TestOutputter()}, new [] {new KeyValuePair<string, string>("SessionID", "TestSession") }))
            {
                _testingAdditionalfields.Add("SessionID", "TestSession");

                _testingMessage = "test fatal message";
                _testingTag = "FATAL";
                _testingSeverity = LogSeverity.Fatal;
                instance.Fatal(_testingMessage, _testingTag);
                LOG.Fatal(_testingMessage, _testingTag);
            }
        }
        [Test]
        public void Error()
        {
            using (ILOGInstance instance = new AsyncLOGInstance(true, SyncType.EventWaitHandle, true, new[] { new TestOutputter() }, new[] { new KeyValuePair<string, string>("SessionID", "TestSession") }))
            {
                _testingAdditionalfields.Add("SessionID", "TestSession");

                _testingMessage = "test error message";
                _testingTag = "ERROR";
                _testingSeverity = LogSeverity.Error;
                instance.Error(_testingMessage, _testingTag);
                LOG.Error(_testingMessage, _testingTag);
            }
        }
        [Test]
        public void Warning()
        {
            using (ILOGInstance instance = new AsyncLOGInstance(true, SyncType.EventWaitHandle, true, new[] { new TestOutputter() }, new[] { new KeyValuePair<string, string>("SessionID", "TestSession") }))
            {
                _testingAdditionalfields.Add("SessionID", "TestSession");

                _testingMessage = "test warning message";
                _testingTag = "WARNING";
                _testingSeverity = LogSeverity.Warning;
                instance.Warning(_testingMessage, _testingTag);
                LOG.Warning(_testingMessage, _testingTag);
            }
        }
        [Test]
        public void Info()
        {
            using (ILOGInstance instance = new AsyncLOGInstance(true, SyncType.EventWaitHandle, true, new[] { new TestOutputter() }, new[] { new KeyValuePair<string, string>("SessionID", "TestSession") }))
            {
                _testingAdditionalfields.Add("SessionID", "TestSession");

                _testingMessage = "test info message";
                _testingTag = "info";
                _testingSeverity = LogSeverity.Info;
                instance.Info(_testingMessage, _testingTag);
                LOG.Info(_testingMessage, _testingTag);
            }
        }
        [Test]
        public void Debug()
        {
            using (ILOGInstance instance = new AsyncLOGInstance(true, SyncType.EventWaitHandle, true, new[] { new TestOutputter() }, new[] { new KeyValuePair<string, string>("SessionID", "TestSession") }))
            {
                _testingAdditionalfields.Add("SessionID", "TestSession");

                _testingMessage = "test DEBUG message";
                _testingTag = "DEBUG";
                _testingSeverity = LogSeverity.Debug;
                instance.Debug(_testingMessage, _testingTag);
                LOG.Debug(_testingMessage, _testingTag);
            }
        }
        [Test]
        public void Trace()
        {
            using (ILOGInstance instance = new AsyncLOGInstance(true, SyncType.EventWaitHandle, true, new[] { new TestOutputter() }, new[] { new KeyValuePair<string, string>("SessionID", "TestSession") }))
            {
                _testingAdditionalfields.Add("SessionID", "TestSession");

                _testingMessage = "test Trace message";
                _testingTag = "Trace";
                _testingSeverity = LogSeverity.Trace;
                instance.Trace(_testingMessage, _testingTag);
                LOG.Trace(_testingMessage, _testingTag);
            }
        }

        [Test]
        public void DoubleSingleton()
        {
            using (new AsyncLOGInstance(true, SyncType.EventWaitHandle, true, new[] { new TestOutputter() }))
            {
                // ReSharper disable once ObjectCreationAsStatement
                Assert.Throws<AlreadyInitializedException>(() => new AsyncLOGInstance(true, SyncType.EventWaitHandle, true, new[] {new TestOutputter()}));
            }
        }
        [TearDown]
        public void Teardown()
        {
            _outputFile.Delete();
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