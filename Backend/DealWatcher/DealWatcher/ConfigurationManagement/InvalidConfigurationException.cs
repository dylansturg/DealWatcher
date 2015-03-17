using System;

namespace DealWatcher.ConfigurationManagement
{
    public class InvalidConfigurationException : Exception
    {
        private const String ErrorFormat =
            "Performing configuration operation and received invalid Type.  Expecting {0} but received {1}.";
        public InvalidConfigurationException(String message) : base(message)
        {   
        }

        public InvalidConfigurationException(String expectedType, Type receivedType)
            : base(String.Format(ErrorFormat, expectedType, receivedType.FullName))
        {
        }
    }
}