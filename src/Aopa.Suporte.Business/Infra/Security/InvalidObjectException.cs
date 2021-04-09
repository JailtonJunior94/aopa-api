using System;
using System.Runtime.Serialization;

namespace Aopa.Suporte.Business.Infra.Security
{
    public class InvalidObjectException<T> : Exception
    {
        public string OriginalObject { get; set; }
        public Type DestinationType { get; set; }

        public InvalidObjectException(string originalObject, string message) : base(message)
        {
            OriginalObject = originalObject;
            DestinationType = typeof(T);
        }

        public InvalidObjectException(string originalObject, string message, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            OriginalObject = originalObject;
            DestinationType = typeof(T);
        }
    }
}
