using Coursework1.Core;
using Coursework1.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// Model to the view that allows the user to import messages from an external source.
    /// </summary>
    public class ImportMessagesViewModel : BaseViewModel
    {
        #region Buttons //Commands of the buttons
        public ICommand ChooseFilesCommand { get; private set; }
        public ICommand ImportSelectedCommand { get; private set; }
        public ICommand ImportAllCommand { get; private set; }
        #endregion

        #region Lists
        public List<string> ChosenFilesList { get; set; } //Files chosen by the user to be imported
        public List<string> UnreadableFiles { get; set; } //Files the system couldn't read
        public List<Tuple<string, string>> ImportedFiles { get; set; }//Files successufully imported, with their path
        public ObservableCollection<string> ChosenFiles { get; set; } //Display to the ChosenFilesList
        public ObservableCollection<string> UnreadFiles { get; set; } //Display to thr UnreadableFilesList
        #endregion

        /// <summary>
        /// Model to the view that allows the user to import messages from an external source.
        /// </summary>
        public ImportMessagesViewModel()
        {
            ChooseFilesCommand = new RelayCommand(ChooseFilesButtonClick);
            ImportSelectedCommand = new RelayCommand(ImportSelectedButtonClick);
            ImportAllCommand = new RelayCommand(ImportAllButtonClick);
            ChosenFilesList = new();
            UnreadableFiles = new();
            ImportedFiles = new();
            ChosenFiles = new ObservableCollection<string>(ChosenFilesList);
            UnreadFiles = new ObservableCollection<string>(UnreadableFiles);
        }
        /// <summary>
        /// Open a file dialog to allow the user to choose .txt files or .json files to be imported.
        /// </summary>
        public void ChooseFilesButtonClick()
        {
            ChosenFilesList = new();
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Filter = "Text files (*.txt)|*.txt|JSON files (*.json)|*.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog() == true)
            {   //Add the selected files to the chosen files list
                foreach (string filename in openFileDialog.FileNames)
                    ChosenFilesList.Add(filename);
            }
            ChosenFiles = new ObservableCollection<string>(ChosenFilesList);//Display the list
            OnChanged(nameof(ChosenFiles));
        }
        /// <summary>
        /// Imports all the messages the user selected.
        /// </summary>
        public void ImportSelectedButtonClick()
        {
            if (ChosenFilesList.Count == 0)
            {
                MessageBox.Show("You haven't selected any file");
            }
            else
            {
                ReadFiles(ChosenFilesList);
                ImportedFiles = new();
            }
            ChosenFilesList = new(); //Clear the list
            ChosenFiles = new ObservableCollection<string>(ChosenFilesList);
            OnChanged(nameof(ChosenFiles));
        }
        /// <summary>
        /// Import all the files from the Import Messages file.
        /// </summary>
        public void ImportAllButtonClick()
        {
            string path = @$"{ System.IO.Directory.GetCurrentDirectory()}\..\..\..\Import Messages\";
            foreach (string file in Directory.EnumerateFiles(path))
            {
                ChosenFilesList.Add(file);
            }
            if (ChosenFilesList.Count == 0)
            {
                MessageBox.Show("There is no file to parse");
            }
            else
            {
                ReadFiles(ChosenFilesList);
                //Prompt the user wether he wants or not to delete the imported files from their previous location
                MessageBoxResult delete = MessageBox.Show("Would you like to delete all the successfully read files" +
                    " from the Import Messages folder?", "Delete read files?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (delete)
                {
                    case MessageBoxResult.Yes:
                        foreach (Tuple<string, string> file in ImportedFiles)
                        {   //detele every imported file
                            File.Delete(file.Item2);
                        }
                        MessageBox.Show("Files deleted!");
                        break;
                    case MessageBoxResult.No: //Do nothing
                        break;
                }
                ImportedFiles = new();//Clear the lists
            }
            ChosenFilesList = new();
        }
        /// <summary>
        /// Treatment for all the selected files to be imported
        /// </summary>
        /// <param name="files"></param>
        private void ReadFiles(List<string> files)
        {
            foreach (string file in files)
            {
                #region JSON Files
                if (file.Contains(".json"))
                {
                    string content = File.ReadAllText(file);
                    if (content.Contains("\"Type\":\"Email\"")) //If it s an email
                    {
                        //Reserealize the file as an Email
                        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                        Email email;
                        MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                        DataContractJsonSerializer ser = new(typeof(Email));
                        email = ser.ReadObject(ms) as Email;
                        ms.Close();
                        if (email.Header != null && email.Sender != null && email.Body != null)
                        {
                            email.WriteToJSON();
                            ImportedFiles.Add(new("Email", file));
                        }
                        else
                        {
                            UnreadableFiles.Add(file);
                        }
                    }
                    else if (content.Contains("\"Type\":\"SMS\"")) //If it is a SMS
                    {
                        //Reserealize the file as a SMS
                        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                        SMS sms;
                        MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                        DataContractJsonSerializer ser = new(typeof(SMS));
                        sms = ser.ReadObject(ms) as SMS;
                        ms.Close();
                        if (sms.Header != null && sms.Sender != null && sms.Body != null)
                        {
                            sms.WriteToJSON();
                            ImportedFiles.Add(new("SMS", file));
                        }
                        else
                        {
                            UnreadableFiles.Add(file);
                        }
                    }
                    else if (content.Contains("\"Type\":\"Tweet\"")) //If it is a Tweet
                    {
                        //Reserealize the file as a Tweet
                        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                        Tweet tweet;
                        MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                        DataContractJsonSerializer ser = new(typeof(Tweet));
                        tweet = ser.ReadObject(ms) as Tweet;
                        ms.Close();
                        if (tweet.Header != null && tweet.Sender != null && tweet.Body != null)
                        {
                            tweet.WriteToJSON();
                            ImportedFiles.Add(new("Tweet", file));
                        }
                        else
                        {
                            UnreadableFiles.Add(file);
                        }
                    }
                    else
                    {   //The unknown files are added in the list
                        UnreadableFiles.Add(file);
                    }
                }
                #endregion
                #region TXT files
                else
                {
                    string content = File.ReadAllText(file);
                    if (content.Contains("\n")) //Find if the content of the file follows the right formal
                    {
                        string[] lines = content.Split("\n");
                        if (lines.Length > 2)
                        {
                            string header = lines[0];//Exctract the header of the message
                            header = header.TrimEnd('\r', '\n');
                            string body = "";
                            for (int i = 1; i < lines.Length; i++)
                            {
                                body += lines[i] + "\n";
                            }
                            bool isHeaderOk = true;
                            if (string.IsNullOrWhiteSpace(header) || string.IsNullOrWhiteSpace(body))
                            {
                                isHeaderOk = false;
                            }
                            //Find if the header follows all the valid conditions
                            if (header.Length != 10)
                            {
                                isHeaderOk = false;
                            }

                            string testHeaderIndex = header[1..];
                            bool testIndex = int.TryParse(testHeaderIndex, out int theNumber);
                            if (!testIndex)
                                isHeaderOk = false;
                            if (theNumber == 0)
                                isHeaderOk = false;
                            if (isHeaderOk)
                            {
                                header = header.ToUpper();
                                switch (header[0])
                                {
                                    case 'S': //Parse the body as a SMS
                                        SMS sms = new(header, body);
                                        if (sms.Sender == "Unknown")
                                            UnreadableFiles.Add(file);
                                        else if (string.IsNullOrWhiteSpace(sms.Text))//Find if the text follows the conditions
                                            UnreadableFiles.Add(file);
                                        else if (sms.Text.Length > 140)
                                            UnreadableFiles.Add(file);
                                        else
                                        {   //Save the SMS if eveything is valid
                                            sms.WriteToJSON();
                                            ImportedFiles.Add(new("SMS", file));
                                        }
                                        break;
                                    case 'E'://Parse the body as an Email
                                        Email email = new(header, body);
                                        if (email.Sender == "Unknown")//Find if the text follows all the conditions
                                            UnreadableFiles.Add(file);
                                        else if (string.IsNullOrWhiteSpace(email.Object))
                                            UnreadableFiles.Add(file);
                                        else if (string.IsNullOrWhiteSpace(email.Text))
                                            UnreadableFiles.Add(file);
                                        else
                                        {   //Save the Email if everything is valid
                                            email.WriteToJSON();
                                            ImportedFiles.Add(new("Email", file));
                                        }
                                        break;
                                    case 'T'://Parse the body as a Tweet
                                        Tweet tweet = new(header, body);
                                        if (tweet.Sender == "Unknown")//Find if the text follows all the conditions
                                            UnreadableFiles.Add(file);
                                        else if (string.IsNullOrWhiteSpace(tweet.Text))
                                            UnreadableFiles.Add(file);
                                        else if (tweet.Text.Length > 140)
                                            UnreadableFiles.Add(file);
                                        else
                                        {   //Save the Tweet if everything is valid
                                            tweet.WriteToJSON();
                                            ImportedFiles.Add(new("Email", file));
                                        }
                                        break;
                                    default://If not, mark it as not valid
                                        UnreadableFiles.Add(file);
                                        break;
                                }
                            }
                            else
                            {   //If not, mark it as not valid
                                UnreadableFiles.Add(file);
                            }
                        }
                        else
                        {   //If not, mark it as not valid
                            UnreadableFiles.Add(file);
                        }
                    }
                    else
                    {   //If not, mark it as not valid
                        UnreadableFiles.Add(file);
                    }
                }
                #endregion
            }
            string successInfo = "Successfully imported the following files:\n";
            foreach (Tuple<string, string> message in ImportedFiles)
            {   //Display a message to inform the user the the messages were successfully imported
                successInfo += $"{message.Item1}: {message.Item2}\n";
            }
            if (!string.IsNullOrWhiteSpace(successInfo))
            {
                MessageBox.Show(successInfo, "Successfull import", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            UnreadFiles = new ObservableCollection<string>(UnreadableFiles); //Display the path of the messages  that were not imported
            OnChanged(nameof(UnreadFiles));
        }
    }
}
