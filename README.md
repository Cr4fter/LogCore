# LogCore

Basic expandable libary to centralize logging systems.

As a default the libary can log to Console and File.

## setup
the base of the logging system is the LOGInstance to setup the system create an instance of this class. The Instance can act as an singleton to enable access throu the static class LOG or be used in instance mode(see [InstanceMode](#Instance Modes)). The LOGInstance requires also an IEnumerable of Logoutputs to use, IEnumerable is used to allow any array type as an input since internaly the modules will allways be stored in an List. The LogInstance is now ready to be used until the Dispose function is Called witch should be called to safely shutdown the logger. LOGInstance implements IDisposable so Using can be used to enshure the deserialisation.

### Instance Modes
#### instance Mode
In this mode multiple loggers can be active at the same time as long as they dont access the sme resource like log Files. the only way to send a logmessage is throu the instance istelf. An inctanced Loger can run besides an Logger using the Singleton Mode.
#### Singleton Mode
As a helper this libary bings a static class LOG this can be used to log messages without a direct LOGInstance reference. however the static class must still be initialized, when a static log mehtod is invoked wihtout initialisation a NotInitialized exception is thrown.
To intitialize the static LOG Class create an loginstance and set singleton mode to true in the constructor, as this functionality utilizes the singleton pattern only one loginstance can be active at the time in singleton mode, when attempting to create a second singleton a AlreadyInitialized Exception is thrown. when the LOGInstance ios disposed the singleton will be cleared aswell so a new instance can be setup.

## custom Logoutput

To create an custom output create a class deriving from IlogOutput. The class needs to override the void Initialize(), void HandleMessage(LOGMessage message) and the void Dispose() function.
#### Multithreadding
This Log Libary can be used Multithreaded but only one thread can work on logmesages at a time so many logmessages from multiple threads can slowdown the process since the threads will wait a long time to get a hold of the Mutex.
