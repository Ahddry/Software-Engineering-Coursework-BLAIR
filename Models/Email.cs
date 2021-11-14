using System;
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
        public bool SIR { get; private set; }
        [DataMember]
        public string[] QuarantinedLinks { get; private set; }
        [DataMember]
        public string SIRSortCode { get; private set; }
        [DataMember]
        public string NatureOfIncident { get; private set; }
        [DataMember]
        public string Object { get; private set; }

        public Email(string header, string body) : base(header, body)
        {
            Type = "Email";
            if (body.Contains('\r'))
                Body = Body.Replace("\r", string.Empty);
            string[] emails = ExtractEmails(body);
            if (emails != null)
                Sender = emails[0];
            else Sender = "Unknown";
            ExtractUrls();
            if (Body.Contains("Sort Code:"))
            {
                SIR = true;
                int index = Body.IndexOf("Sort Code:");
                SIRSortCode = Body.Substring(index + 11, 11);
            }
            else
            {
                SIR = false;
                SIRSortCode = string.Empty;
            }
            if (SIR)
            {
                string[] lines = Body.Split("\n");
                foreach (string line in lines)
                {
                    if (line.Contains("Nature of Incident:"))
                        NatureOfIncident = line[20..];
                }
                if (NatureOfIncident == null)
                    Text = string.Empty;
                else if (Body.Length > (Body.IndexOf(NatureOfIncident) + NatureOfIncident.Length + 1))
                    Text = Body[(Body.IndexOf(NatureOfIncident) + NatureOfIncident.Length + 1)..];
                else
                    Text = string.Empty;
                Other += $"SIGNIFICANT INCIDENT REPORT\nSort Code: {SIRSortCode}\nNature of Incident: {NatureOfIncident}\n";
                Object = $"SIR: {SIRSortCode} - {NatureOfIncident}";
            }
            else
            {
                NatureOfIncident = string.Empty;
                if (Sender != "Unknown")
                {
                    string[] lines = Body.Split("\n");
                    int lineNb = 0;
                    if (lines.Length > 1)
                    {
                        foreach (string line in lines)
                        {
                            if (line.Contains(Sender))
                            {
                                Object = lines[lineNb + 1];
                                break;
                            }
                            lineNb++;
                        }
                    }
                    else Object = string.Empty;

                    if (Body.Length > (Sender.Length + Object.Length + 3))
                        Text = Body[(Sender.Length + Object.Length + 2)..];
                    else
                        Text = string.Empty;
                }
                else
                {
                    if(Body.Contains("\n"))
                    {
                        string[] lines = Body.Split("\n");
                        Object = lines[0];
                        Text = Body[(Object.Length + 1)..];
                    }
                    else
                        Text = Body;
                }
            }
            Other = $"Object: {Object}\n";
            if (emails != null)
            {
                if (emails.Length > 1)
                {
                    Other += "Mentioned email address: ";
                    for (int i = 1; i < emails.Length; i++)
                    {
                        Other += emails[i] + "; ";
                    }
                }
            }
        }

        public override void WriteToJSON()
        {
            DateTime SaveTime = DateTime.Now;
            Date = SaveTime.ToString();
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the User object to the stream.
            var ser = new DataContractJsonSerializer(typeof(Email));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            string path = @$"{System.IO.Directory.GetCurrentDirectory()}\..\..\..\Saved Messages\{SaveTime.Year}.{SaveTime.Month}.{SaveTime.Day}-{SaveTime.Hour}.{SaveTime.Minute}.{SaveTime.Second}_{Header}.json";
            File.WriteAllBytes(path, json);
        }

        private static string[] ExtractEmails(string str)
        {
            //https://forums.asp.net/t/1643850.aspx?How+to+catch+email+address+or+a+phone+number+in+a+string+c+
            string RegexPattern = @"\b[A-Z0-9._-]+@[A-Z0-9][A-Z0-9.-]{0,61}[A-Z0-9]\.[A-Z.]{2,6}\b";

            // Find matches
            MatchCollection matches = Regex.Matches(str, RegexPattern, RegexOptions.IgnoreCase);
            
            string[] MatchList = new string[matches.Count];

            // add each match
            int c = 0;
            foreach (Match match in matches)
            {
                MatchList[c] = match.ToString();
                c++;
            }
            if (c != 0)
                return MatchList;
            else return null;
        }

        private void ExtractUrls()
        {
            //Regex fuction found on
            //https://social.msdn.microsoft.com/Forums/vstudio/en-US/af82ce78-6aa7-43cc-8a10-cdacd9b93728/find-url-from-a-text-file-in-c?forum=csharpgeneral
            MatchCollection matches = Regex.Matches(Body, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
            QuarantinedLinks = new string[matches.Count];
            int c = 0;
            foreach (Match match in matches)
            {
                QuarantinedLinks[c] = match.ToString();
                c++;
            }
            foreach (string url in QuarantinedLinks)
            {
                Body = Body.Replace(url, "<URL Quarantined>");
            }
        }
    }
}
