using Coursework1.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// Model to the view that allows the user to read all the statistics on the saved messages.
    /// </summary>
    public class StatisticsViewModel : BaseViewModel
    {
        public string SMSNumber { get; set; }
        public string EmailNumber { get; set; }
        public string TweetNumber { get; set; }
        #region Lists
        public ObservableCollection<Statistics> SIRStats { get; set; }  //Display to the SIRList
        public ObservableCollection<Statistics> SocialMediasMentionsStats { get; set; }//Display to thr MentionsList
        public ObservableCollection<Statistics> SocialMediasHashtagsStats { get; set; }//Display to th HashtagsList
        public List<Statistics> SIRList { get; set; } //List of the statistics of all the Significant Incident Report nature
        public List<Statistics> MentionsList { get; set; } //List of the statistics of all the Mentioned users in Tweets
        public List<Statistics> HashtagsList { get; set; }//List of the statistics of all the Hashtags in Tweets
        #endregion

        /// <summary>
        /// /// Model to the view that allows the user to read all the statistics on the saved messages.
        /// </summary>
        public StatisticsViewModel()
        {
            #region Setup
            SIRList = new();
            MentionsList = new();
            HashtagsList = new();
            SIRStats = new(SIRList);
            SocialMediasMentionsStats = new(MentionsList);
            SocialMediasHashtagsStats = new(HashtagsList);
            SMSNumber = "0"; EmailNumber = "0"; TweetNumber = "0";
            int smsCount = 0, emailCount = 0, tweetCount = 0, noiCount = 0, mentionCount = 0, hashtagCount = 0;

            List<string> foundSirNoI = new();
            List<string> foundMentions = new();
            SortedSet<string> existingMentions = new();
            List<string> foundHashtags = new();
            SortedSet<string> existingHashtags = new();
            List<string> existingNoI = new();
            existingNoI.Add("Theft");
            existingNoI.Add("Staff Attack");
            existingNoI.Add("ATM Theft");
            existingNoI.Add("Raid");
            existingNoI.Add("Customer Attack");
            existingNoI.Add("Staff Abuse");
            existingNoI.Add("Bomb Threat");
            existingNoI.Add("Terrorism");
            existingNoI.Add("Suspicious Incident");
            existingNoI.Add("Intelligence");
            existingNoI.Add("Cash Loss");
            #endregion
            string path = @$"{ System.IO.Directory.GetCurrentDirectory()}\Saved Messages\";
            foreach (string file in Directory.EnumerateFiles(path, "*.json")) //Read all the saved messages
            {
                string content = File.ReadAllText(file);
                if (content.Contains("\"Type\":\"Email\""))//If it s an email
                {
                    //Reserealize the file as an Email
                    emailCount++;
                    //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                    Email email;
                    MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                    DataContractJsonSerializer ser = new(typeof(Email));
                    email = ser.ReadObject(ms) as Email;
                    ms.Close();
                    if (email.SIR) //If the mail is a SIR
                    {
                        foundSirNoI.Add(email.NatureOfIncident); //Add its nature to the list
                        noiCount++;
                    }
                }
                else if (content.Contains("\"Type\":\"SMS\""))//If it is a SMS
                {
                    smsCount++;
                }
                else if (content.Contains("\"Type\":\"Tweet\""))//If it is a Tweet
                {
                    //Reserealize the file as a Tweet
                    tweetCount++;
                    //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                    Tweet tweet;
                    MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                    DataContractJsonSerializer ser = new(typeof(Tweet));
                    tweet = ser.ReadObject(ms) as Tweet;
                    ms.Close();
                    if (tweet.Mentions != null)
                    {
                        foreach (string mention in tweet.Mentions) //Add all the mentions to the mentions lists
                        {
                            foundMentions.Add(mention);
                            existingMentions.Add(mention);
                            mentionCount++;
                        }
                    }
                    if (tweet.Hashtags != null)
                    {
                        foreach (string hashtag in tweet.Hashtags)//Add all the hashtags to the hashtags lists
                        {
                            foundHashtags.Add(hashtag);
                            existingHashtags.Add(hashtag);
                            hashtagCount++;
                        }
                    }
                }
            }
            //Display the messages counts
            EmailNumber = emailCount.ToString();
            SMSNumber = smsCount.ToString();
            TweetNumber = tweetCount.ToString();
            int count = 0;
            foreach (string natureOfIncident in existingNoI) //Count the differrent Natures of Incident and what they represent in regard of the total
            {
                foreach (string noi in foundSirNoI)
                {
                    if (natureOfIncident == noi)
                        count++;
                }
                SIRList.Add(new(natureOfIncident, count, noiCount));
                count = 0;
            }
            foreach (string mention in existingMentions)//Count the differrent Mention and what they represent in regard of the total
            {
                foreach (string men in foundMentions)
                {
                    if (mention == men)
                        count++;
                }

                MentionsList.Add(new(mention, count, mentionCount));
                count = 0;
            }
            foreach (string hashtag in existingHashtags)//Count the differrent Hashtag and what they represent in regard of the total
            {
                foreach (string hash in foundHashtags)
                {
                    if (hashtag == hash)
                        count++;
                }
                HashtagsList.Add(new(hashtag, count, hashtagCount));
                count = 0;
            }   //Display all the statistics.
            SIRStats = new(SIRList);
            SocialMediasMentionsStats = new(MentionsList);
            SocialMediasHashtagsStats = new(HashtagsList);
        }
    }
}
