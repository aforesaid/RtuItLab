using System;
using Newtonsoft.Json;

namespace RtuItLab.Infrastructure.MassTransit
{
    public class ResponseMassTransit<T>
    {
        public T Content { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.Auto)]
        public Exception Exception { get; set; }
    }
}
