using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Coursework1.Models
{
    [DataContract]
    public class SMS : MessageType
    {
        public List<Tuple<string, string>> Abbreviations { get; private set; }

        public SMS(string header, string body) : base(header, body)
        {
            Type = "SMS";
            ExtractPhoneNumbers();
            Abbreviations = new();
            LoadAbbreviations();
            foreach (Tuple<string, string> expression in Abbreviations)
            {
                while (Body.Contains($"{expression.Item1} ") || Body.Contains($"{expression.Item1}\n"))
                {
                    Body = Body.Replace($"{expression.Item1}", $"{expression.Item1}:<{expression.Item2}>");
                }
            }
            if (Sender != "Unknown")
                Text = Body[(Sender.Length + 1)..];
            else
                Text = Body;
        }

        public override void WriteToJSON()
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the User object to the stream.
            var ser = new DataContractJsonSerializer(typeof(SMS));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            DateTime SaveTime = DateTime.Now;
            string path = @$"{System.IO.Directory.GetCurrentDirectory()}\..\..\..\Saved Messages\{SaveTime.Year}.{SaveTime.Month}.{SaveTime.Day}-{SaveTime.Hour}.{SaveTime.Minute}.{SaveTime.Second}_{Header}.json";
            File.WriteAllBytes(path, json);
        }

        private void ExtractPhoneNumbers()
        {
            //Regex fuction found on
            //https://stackoverflow.com/a/19133469
            MatchCollection matches = Regex.Matches(Body, @"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}\b");
            string[] phoneNumbers = new string[matches.Count];
            int c = 0;
            foreach (Match match in matches)
            {
                phoneNumbers[c] = match.ToString();
                c++;
            }
            if (c >= 1)
                Sender = phoneNumbers[0];
            else Sender = "Unknown";
        }

        private void LoadAbbreviations()
        {
            //string path = @$"{ System.IO.Directory.GetCurrentDirectory()}\..\..\..\Other\textwords.csv";
            string path = @"C:\Users\blair\OneDrive\Documents\Travail\Software Engineering\Coursework\Coursework1\Other\textwords.csv";
            try
            {
                string[] info = File.ReadAllLines(path);
                foreach (string lines in info)
                {
                    string[] line = lines.Split(',');
                    Tuple<string, string> pair = new(line[0], line[1]);
                    Abbreviations.Add(pair);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}\nUnable to find {path}");
            }
        }
    }
}
