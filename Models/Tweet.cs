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
    public class Tweet : MessageType
    {
        public List<Tuple<string, string>> Abbreviations { get; private set; }
        [DataMember]
        public string[] Hashtags { get; private set; }
        [DataMember]
        public string[] Mentions { get; private set; }

        public Tweet(string header, string body) : base(header, body)
        {
            Type = "Tweet";
            FindMentions();
            FindHashtags();
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
            if (Mentions != null)
            {
                Other += "Mentions: ";
                foreach (string elem in Mentions)
                    Other += $"{elem} ";
                Other += "\n";
            }
            if (Hashtags != null)
            {
                Other += "Hashtags: ";
                foreach (string elem in Hashtags)
                    Other += $"{elem} ";
                Other += "\n";
            }
        }

        public override void WriteToJSON()
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the User object to the stream.
            var ser = new DataContractJsonSerializer(typeof(Tweet));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            DateTime SaveTime = DateTime.Now;
            string path = @$"{System.IO.Directory.GetCurrentDirectory()}\..\..\..\Saved Messages\{SaveTime.Year}.{SaveTime.Month}.{SaveTime.Day}-{SaveTime.Hour}.{SaveTime.Minute}.{SaveTime.Second}_{Header}.json";
            File.WriteAllBytes(path, json);
        }

        private void FindHashtags()
        {
            //Simple Regex I made Myself
            MatchCollection matches = Regex.Matches(Body, @"\B(\#[a-zA-Z0-9]+\b)");
            if (matches.Count > 0) Hashtags = new string[matches.Count];
            int c = 0;
            foreach (Match match in matches)
            {
                Hashtags[c] = match.ToString();
                c++;
            }
        }
        private void FindMentions()
        {
            //Simple Regex I made Myself
            MatchCollection matches = Regex.Matches(Body, @"\B(\@[a-zA-Z0-9]+\b)");
            if (matches.Count > 0)
            {
                Mentions = new string[matches.Count - 1];
                Sender = matches[0].ToString();
                if (matches.Count > 1)
                {
                    for (int i = 1; i < matches.Count; i++) 
                    {
                        Mentions[i - 1] = matches[i].ToString();
                    }
                }
            }
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
