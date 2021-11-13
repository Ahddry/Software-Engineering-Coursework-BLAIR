using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1.Models
{
    [DataContract]
    public class MessageType
    {
        [DataMember]
        public virtual string Header { get; private set; }
        [DataMember]
        public virtual string Body { get; protected set; }
        [DataMember]
        public virtual string Type { get; protected set; }
        [DataMember]
        public virtual string Date { get; protected set; }
        [DataMember]
        public virtual string Sender { get; protected set; }
        [DataMember]
        public virtual string Text { get; protected set; }
        [DataMember]
        public virtual string Other { get; protected set; }

        public MessageType(string header, string body)
        {
            Header = header;
            Body = body;
            Type = "Unknown";
            Date = DateTime.Now.ToString();
            Sender = "Unknown";
            Text = string.Empty;
            Other = string.Empty;
        }

        public virtual void WriteToJSON()
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the User object to the stream.
            var ser = new DataContractJsonSerializer(typeof(MessageType));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            DateTime SaveTime = DateTime.Now;
            string path = @$"\..\..\..\Saved Messages\{SaveTime.Year}.{SaveTime.Month}.{SaveTime.Day}-{SaveTime.Hour}.{SaveTime.Minute}.{SaveTime.Second}_{Header}.json";
            File.WriteAllBytes(path, json);
        }

        public virtual string GetPath()
        {
            DateTime dateTime = Convert.ToDateTime(Date);
            string path = @$"{System.IO.Directory.GetCurrentDirectory()}\..\..\..\Saved Messages\{dateTime.Year}.{dateTime.Month}.{dateTime.Day}-{dateTime.Hour}.{dateTime.Minute}.{dateTime.Second}_{Header}.json";
            return path;
        }
    }
}
