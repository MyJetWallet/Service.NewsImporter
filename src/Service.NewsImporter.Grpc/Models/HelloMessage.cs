using System.Runtime.Serialization;
using Service.NewsImporter.Domain.Models;

namespace Service.NewsImporter.Grpc.Models
{
    [DataContract]
    public class HelloMessage : IHelloMessage
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}