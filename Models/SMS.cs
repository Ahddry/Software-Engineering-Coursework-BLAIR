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
    /// Represents Type of Message, with a sending phone number following the international phone number format, 
    /// and an up to 140 characters long text
    /// </summary>
    [DataContract]
    public class SMS : MessageType
    {
        public List<Tuple<string, string>> Abbreviations { get; private set; } //List of all the textspeak abbreviations and their detailed version.
        /// <summary>
        /// Represents Type of Message, with a sending phone number following the international phone number format, 
        /// and an up to 140 characters long text
        /// </summary>
        /// <param name="header">Header of the SMS. Needs to start with an 'S' and be followed by 9 digits</param>
        /// <param name="body">Body of the SMS, to be parsed</param>
        public SMS(string header, string body) : base(header, body)
        {
            Type = "SMS"; //Set the type of the message
            ExtractPhoneNumbers(); //Find all the phone numbers in the message
            Abbreviations = new();
            LoadAbbreviations(); //Load all the textspeak abbreviations and their detailed version from the CSV file.
            foreach (Tuple<string, string> expression in Abbreviations) //replace all the textspeaks abbreviations by their detailed versions
            {
                while (Body.Contains($"{expression.Item1} ") || Body.Contains($"{expression.Item1}\n"))
                {
                    Body = Body.Replace($"{expression.Item1}", $"{expression.Item1}:<{expression.Item2}>");
                }
            }
            if (Sender != "Unknown") //Extract the text from the body
                Text = Body[(Sender.Length + 1)..];
            else
                Text = Body;
        }
        /// <summary>
        /// Converts the SMS and serialize it to a JSON stream.
        /// </summary>
        public override void WriteToJSON()
        {
            DateTime SaveTime = DateTime.Now;
            Date = SaveTime.ToString();
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serialize the SMS object to the stream.
            var ser = new DataContractJsonSerializer(typeof(SMS));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            string path = @$"{System.IO.Directory.GetCurrentDirectory()}\..\..\..\Saved Messages\{SaveTime.Year}.{SaveTime.Month}.{SaveTime.Day}-{SaveTime.Hour}.{SaveTime.Minute}.{SaveTime.Second}_{Header}.json";
            File.WriteAllBytes(path, json);
        }
        /// <summary>
        /// Extract all the phone numbers from the body of the message
        /// </summary>
        private void ExtractPhoneNumbers()
        {
            //Regex pattern found on
            //https://stackoverflow.com/a/19133469
            // Find matches
            MatchCollection matches = Regex.Matches(Body, @"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}\b");
            string[] phoneNumbers = new string[matches.Count];
            int c = 0;
            foreach (Match match in matches)
            {
                phoneNumbers[c] = match.ToString(); // add each match
                c++;
            }
            if (c >= 1)
                Sender = phoneNumbers[0];
            else Sender = "Unknown";
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
