using System.ComponentModel;
using Newtonsoft.Json;

namespace Domain.Models
{
    public class Response<T> where T : class
    {
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public EnumResponseStatus Status { get; set; }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Errors { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }
    }

    public enum EnumResponseStatus
    {
        [Description("ok")]
        Ok,

        [Description("error")]
        Error
    }
}