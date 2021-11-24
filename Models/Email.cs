using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;

namespace Coursework1.Models
{
    /// <summary>
    /// Represents Type of Message, with a sending address following the email address format, an object that can be up to
    /// 20 characters long and an up to 1028 characters long text
    /// </summary>
    [DataContract]
    public class Email : MessageType
    {
        [DataMember]
        public bool SIR { get; private set; } //Is the email a Significant Incident Report
        [DataMember]
        public string[] QuarantinedLinks { get; private set; } //All the embed URLs are quarantined in this array
        [DataMember]
        public string SIRSortCode { get; private set; } //A unique code for the SIR (only if SIR)
        [DataMember]
        public string NatureOfIncident { get; private set; } //Nature of the Incident (only if SIR)
        [DataMember]
        public string Object { get; private set; } //Object of the Email

        /// <summary>
        /// Represents Type of Message, with a sending address following the email address format, an object that can be up to
        /// 20 characters long and an up to 1028 characters long text
        /// </summary>
        /// <param name="header">Header of the Email. Needs to start with an 'E' and be followed by 9 digits</param>
        /// <param name="body">Body of the Email, to be parsed</param>
        public Email(string header, string body) : base(header, body)
        {
            Type = "Email";         //apply the type
            if (body.Contains('\r'))//Remove all the superfluous '\r' characters
                Body = Body.Replace("\r", string.Empty);
            string[] emails = ExtractEmails(body);//Find the email addresses in the body
            if (emails != null)
                Sender = emails[0];     //give its value to the Sender
            else Sender = "Unknown";
            ExtractUrls();              //Find all the URLs in the message
            if (Body.Contains("Sort Code:"))
            {
                SIR = true;
                int index = Body.IndexOf("Sort Code:"); //Extract the Sort Code if the mail is a SIR
                SIRSortCode = Body.Substring(index + 11, 9);
            }
            else
            {
                SIR = false;                //Otherwise, set it to false.
                SIRSortCode = string.Empty;
            }
            if (SIR)
            {   //If the message is a SIR, we will try to find the Nature of the incident
                string[] lines = Body.Split("\n");
                foreach (string line in lines)
                {
                    if (line.Contains("Nature of Incident:"))
                        NatureOfIncident = line[20..];
                }
                if (NatureOfIncident == null) //If we can't find a Nature of Incident, we make sure that the message cannot be treated as a valid SIR email.
                    Text = string.Empty;
                else if (Body.Length > (Body.IndexOf(NatureOfIncident) + NatureOfIncident.Length + 1))
                    Text = Body[(Body.IndexOf(NatureOfIncident) + NatureOfIncident.Length + 1)..]; //Extract the text from the body
                else //detect if the body doesn't have any text
                    Text = string.Empty;
                Other += $"SIGNIFICANT INCIDENT REPORT\nSort Code: {SIRSortCode}\nNature of Incident: {NatureOfIncident}\n"; //Add a summary for the datagrid
                Object = $"SIR: {SIRSortCode} - {NatureOfIncident}"; //Apply the SIR sort code and the nature of incident as the object of the email
            }
            else
            {
                NatureOfIncident = string.Empty; //Standard emails don't have any Nature of Incident
                if (Sender != "Unknown")
                {
                    string[] lines = Body.Split("\n");
                    int lineNb = 0;
                    if (lines.Length > 1) // Find the object of the message
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
                        Text = Body[(Sender.Length + Object.Length + 2)..]; //Extract the text from the body
                    else//detect if the body doesn't have any text
                        Text = string.Empty;

                    if (Object.Length > 20) //Detect if the object of the message is too long
                        Object += "A very long string to make sure it will be over 60 characters long and will enable us to accept the very long SIR as objects.";
                }
                else
                {   //Procedure if the user is unknown
                    if (Body.Contains("\n"))
                    {
                        string[] lines = Body.Split("\n");
                        Object = lines[0];
                        Text = Body[(Object.Length + 1)..];
                    }
                    else
                        Text = Body;
                }
            }
            Other = $"Object: {Object}\n";//Apply object as the object of the mail
            if (emails != null)
            {
                if (emails.Length > 1) //Find other email addresses mentioned in the mail
                {
                    Other += "Mentioned email address: ";
                    for (int i = 1; i < emails.Length; i++)
                    {
                        Other += emails[i] + "; ";
                    }
                }
            }
        }
        /// <summary>
        /// Converts the Email and serialize it to a JSON stream.
        /// </summary>
        public override void WriteToJSON()
        {
            DateTime SaveTime = DateTime.Now;
            Date = SaveTime.ToString();
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serialize the Email object to the stream.
            var ser = new DataContractJsonSerializer(typeof(Email));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            string path = @$"{System.IO.Directory.GetCurrentDirectory()}\..\..\..\Saved Messages\{SaveTime.Year}.{SaveTime.Month}.{SaveTime.Day}-{SaveTime.Hour}.{SaveTime.Minute}.{SaveTime.Second}_{Header}.json";
            File.WriteAllBytes(path, json);
        }
        /// <summary>
        /// Extract all the email addresses from the given string.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>An array with all the email addresses from the string.</returns>
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
        /// <summary>
        /// Extract all the URLs from the body of the Email, quarantine them, and replace them with an informative message.
        /// </summary>
        private void ExtractUrls()
        {
            //Regex fuction found on
            //https://social.msdn.microsoft.com/Forums/vstudio/en-US/af82ce78-6aa7-43cc-8a10-cdacd9b93728/find-url-from-a-text-file-in-c?forum=csharpgeneral
            MatchCollection matches = Regex.Matches(Body, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
            QuarantinedLinks = new string[matches.Count];
            int c = 0;
            foreach (Match match in matches)
            {
                QuarantinedLinks[c] = match.ToString();//Store all the URLs found
                c++;
            }
            foreach (string url in QuarantinedLinks)
            {
                Body = Body.Replace(url, "<URL Quarantined>");//replace the URLs with an informative message
            }
        }
    }
}
