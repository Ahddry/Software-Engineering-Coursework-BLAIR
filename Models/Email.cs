﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Coursework1.Models
{
    [DataContract]
    public class Email : MessageType
    {
        [DataMember]
        public string SIR { get; private set; }
        [DataMember]
        public string[] QuarantinedLinks { get; private set; }

        public Email(string header, string body) : base(header, body)
        {
            Type = "Email";
            SIR = string.Empty;
            
        }

        public override void WriteToJSON()
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the User object to the stream.
            var ser = new DataContractJsonSerializer(typeof(Email));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            DateTime SaveTime = DateTime.Now;
            string path = @$"{System.IO.Directory.GetCurrentDirectory()}\..\..\..\Saved Messages\{SaveTime.Year}.{SaveTime.Month}.{SaveTime.Day}-{SaveTime.Hour}.{SaveTime.Minute}.{SaveTime.Second}_{Header}.json";
            File.WriteAllBytes(path, json);
        }

        
    }
}
