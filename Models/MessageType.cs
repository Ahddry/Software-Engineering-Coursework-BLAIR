using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Coursework1.Models
{
    /// <summary>
    /// Represents a complex message form.
    /// </summary>
    [DataContract]
    public class MessageType
    {
        [DataMember]
        public virtual string Header { get; private set; }//Header of the message
        [DataMember]
        public virtual string Body { get; protected set; }//Body of the message
        [DataMember]
        public virtual string Type { get; protected set; }//Type of the message
        [DataMember]
        public virtual string Date { get; protected set; }//Date when the message was received/saved
        [DataMember]
        public virtual string Sender { get; protected set; }//Sender of the message
        [DataMember]
        public virtual string Text { get; protected set; }//Text of the message
        [DataMember]
        public virtual string Other { get; protected set; }//Other informations, to be displayed on the datagrid
        /// <summary>
        /// Represents a complex message form.
        /// </summary>
        /// <param name="header">Header of the Messages. Needs to start with a letter and be followed by 9 digits</param>
        /// <param name="body">Body of the message</param>
        public MessageType(string header, string body)
        {
            Header = header;
            Body = body;
            Type = "Unknown";
            Date = DateTime.Now.ToString(); //load the date at the creation of the message
            Sender = "Unknown";
            Text = string.Empty;
            Other = string.Empty;
        }
        /// <summary>
        /// Converts the Message and serialize it to a JSON stream.
        /// </summary>
        public virtual void WriteToJSON()
        {
            DateTime SaveTime = DateTime.Now;
            Date = SaveTime.ToString();
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serialize the message object to the stream.
            var ser = new DataContractJsonSerializer(typeof(MessageType));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            string path = @$"\Saved Messages\{SaveTime.Year}.{SaveTime.Month}.{SaveTime.Day}-{SaveTime.Hour}.{SaveTime.Minute}.{SaveTime.Second}_{Header}.json";
            File.WriteAllBytes(path, json);
        }
        /// <summary>
        /// Find the path to load this message.
        /// </summary>
        /// <returns>The path to access this message in the computer of the user.</returns>
        public virtual string GetPath()
        {
            DateTime dateTime = Convert.ToDateTime(Date);
            string path = @$"{System.IO.Directory.GetCurrentDirectory()}\Saved Messages\{dateTime.Year}.{dateTime.Month}.{dateTime.Day}-{dateTime.Hour}.{dateTime.Minute}.{dateTime.Second}_{Header}.json";
            return path;
        }
    }
}
