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
using LogCore.Outputters;

namespace TestApplication
{
    class Program
    {
        static void Main()
        {
            //Creating a list of desired outputers for our logging.
            //in this case a console outputter which is displaying the messages to the console and an instance of our own test implementation.
            List<ILogOutput> outs = new List<ILogOutput> { new ConsoleOutputter(), new CustomExampleLogOut()};

            //Here were setting up Additional fields which will be added as additional fields to every Log Message passing through our logging instance.
            Dictionary<string, string> additionalfields = new Dictionary<string, string> { {"SessionID", "ExampleSession"} };

            //here were creating the Log Instance telling it to set up the Singleton so we can use the static Log Instance and
            //the two earlier created collections with the outputs and additional information.
            using (ILOGInstance loin = new LOGInstance(true, outs, additionalfields))
            {
                //Setting the Severity filter to INFO so only Messages of type INFO and above will be displayed. this can be used to filter debug messages in a release environment.
                loin.SetLogFilter(LogSeverity.Info);

                //This message will be ignored Since only Messages with the severity of info and above will be handled.
                LOG.Debug("DebugMessage");
                //This message will be Handled.
                LOG.Error("Stuff gone wrong");
            }
        }
    }
}