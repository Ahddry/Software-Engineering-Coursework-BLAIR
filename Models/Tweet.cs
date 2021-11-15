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
    /// <summary>
    /// Represents Type of Message, with a sending username following the Twitter username format,
    /// and an up to 140 characters long text
    /// </summary>
    [DataContract]
    public class Tweet : MessageType
    {
        public List<Tuple<string, string>> Abbreviations { get; private set; }//List of all the textspeak abbreviations and their detailed version.
        [DataMember]
        public string[] Hashtags { get; private set; } //List of all the Hashtags found in the Tweet
        [DataMember]
        public string[] Mentions { get; private set; }//List of all the Mentioned users found in the Tweet
        /// <summary>
        /// Represents Type of Message, with a sending username following the Twitter username format,
        /// and an up to 140 characters long text
        /// </summary>
        /// <param name="header">Header of the Tweet. Needs to start with an 'T' and be followed by 9 digits</param>
        /// <param name="body">Body of the Tweet, to be parsed</param>
        public Tweet(string header, string body) : base(header, body)
        {
            Type = "Tweet";//Set the type of the message
            FindMentions();//Find all the mentioned users in the message
            FindHashtags();//Find all the Hashtages in the message
            Abbreviations = new();
            LoadAbbreviations();//Load all the textspeak abbreviations and their detailed version from the CSV file.
            if (body.Contains('\r'))//Remove all the superfluous '\r' characters
                Body = Body.Replace("\r", string.Empty);
            foreach (Tuple<string, string> expression in Abbreviations)//replace all the textspeaks abbreviations by their detailed versions
            {
                while (Body.Contains($"{expression.Item1} ") || Body.Contains($"{expression.Item1}\n"))
                {
                    Body = Body.Replace($"{expression.Item1}", $"{expression.Item1}:<{expression.Item2}>");
                }
            }

            string[] lines = Body.Split("\n");
            if (lines.Length > 1) //Find the Username of the Sender
            {
                lines[0] = lines[0].Replace(" ", string.Empty);
                if (lines[0] != Sender)
                    Sender = "Unknown";
            }
            else Sender = "Unknown";

            if (Sender != "Unknown")
            {
                if (Body.Length > Sender.Length + 2)//Extract the text from the body
                    Text = Body[(Sender.Length + 1)..];
                else Text = string.Empty;
            }
            else
                Text = Body;
            if (Mentions != null)//Add all the mentions to the Other category, so they can be seen from the Datagrid
            {
                Other += "Mentions: ";
                foreach (string elem in Mentions)
                    Other += $"{elem} ";
                Other += "\n";
            }
            if (Hashtags != null)//Add all the hashtags to the Other category, so they can be seen from the Datagrid
            {
                Other += "Hashtags: ";
                foreach (string elem in Hashtags)
                    Other += $"{elem} ";
                Other += "\n";
            }
        }
        /// <summary>
        /// Converts the Tweet and serialize it to a JSON stream.
        /// </summary>
        public override void WriteToJSON()
        {
            DateTime SaveTime = DateTime.Now;
            Date = SaveTime.ToString();
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serialize the Tweet object to the stream.
            var ser = new DataContractJsonSerializer(typeof(Tweet));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            string path = @$"{System.IO.Directory.GetCurrentDirectory()}\..\..\..\Saved Messages\{SaveTime.Year}.{SaveTime.Month}.{SaveTime.Day}-{SaveTime.Hour}.{SaveTime.Minute}.{SaveTime.Second}_{Header}.json";
            File.WriteAllBytes(path, json);
        }
        /// <summary>
        /// Extract all the Hashtags from the body of the message
        /// </summary>
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
        /// <summary>
        /// Extract all the mentioned usernames from the body of the message
        /// </summary>
        private void FindMentions()
        {
            //Simple Regex I made Myself
            MatchCollection matches = Regex.Matches(Body, @"\B(\@[a-zA-Z0-9]{1,20}\b)");
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
        /// <summary>
        /// Load all the textspeak abbreviations and their detailed version from the CSV file.
        /// </summary>
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
