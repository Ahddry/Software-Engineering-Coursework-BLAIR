using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Coursework1.Core;
using Coursework1.Models;
using Microsoft.Win32;

namespace Coursework1.UI.View_Models
{
    class ImportMessagesViewModel : BaseViewModel
    {
        #region Buttons
        public ICommand ChooseFilesCommand { get; private set; }
        public ICommand ImportSelectedCommand { get; private set; }
        public ICommand ImportAllCommand { get; private set; }
        #endregion
        
        public List<string> ChosenFilesList { get; set; }
        public List<string> UnreadableFiles { get; set; }
        public List<Tuple<string,string>> ImportedFiles { get; set; }
        public ObservableCollection<string> ChosenFiles { get; set; }
        public ObservableCollection<string> UnreadFiles { get; set; }

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
            {
                foreach (string filename in openFileDialog.FileNames)
                    ChosenFilesList.Add(filename);
            }
            ChosenFiles = new ObservableCollection<string>(ChosenFilesList);
            OnChanged(nameof(ChosenFiles));
        }

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
            ChosenFilesList = new();
            ChosenFiles = new ObservableCollection<string>(ChosenFilesList);
            OnChanged(nameof(ChosenFiles));
        }

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
                MessageBoxResult delete = MessageBox.Show("Would you like to delete all the successfully read files" +
                    " from the Import Messages folder?", "Delete read files?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (delete)
                {
                    case MessageBoxResult.Yes:
                        foreach (Tuple<string, string> file in ImportedFiles)
                        {
                            File.Delete(file.Item2);
                        }
                        MessageBox.Show("Files deleted!");
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                ImportedFiles = new();
            }
            ChosenFilesList = new();
        }

        private void ReadFiles(List<string> files)
        {
            foreach (string file in files)
            {
                if (file.Contains(".json"))
                {
                    string content = File.ReadAllText(file);
                    if (content.Contains("\"Type\":\"Email\""))
                    {
                        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                        Email email;
                        MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                        DataContractJsonSerializer ser = new(typeof(Email));
                        email = ser.ReadObject(ms) as Email;
                        ms.Close();
                        email.WriteToJSON();
                        ImportedFiles.Add(new("Email", file));
                    }
                    else if (content.Contains("\"Type\":\"SMS\""))
                    {
                        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                        SMS sms;
                        MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                        DataContractJsonSerializer ser = new(typeof(SMS));
                        sms = ser.ReadObject(ms) as SMS;
                        ms.Close();
                        sms.WriteToJSON();
                        ImportedFiles.Add(new("SMS", file));
                    }
                    else if (content.Contains("\"Type\":\"Tweet\""))
                    {
                        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                        Tweet tweet;
                        MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                        DataContractJsonSerializer ser = new(typeof(Tweet));
                        tweet = ser.ReadObject(ms) as Tweet;
                        ms.Close();
                        tweet.WriteToJSON();
                        ImportedFiles.Add(new("Tweet", file));
                    }
                    else
                    {
                        UnreadableFiles.Add(file);
                    }
                }
                else
                {
                    string content = File.ReadAllText(file);
                    if (content.Contains("\n"))
                    {
                        string[] lines = content.Split("\n");
                        if (lines.Length > 2)
                        {
                            string header = lines[0];
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
                                    case 'S':
                                        SMS sms = new(header, body);
                                        if (sms.Sender == "Unknown")
                                            UnreadableFiles.Add(file);
                                        else if (string.IsNullOrWhiteSpace(sms.Text))
                                            UnreadableFiles.Add(file);
                                        else if (sms.Text.Length > 140)
                                            UnreadableFiles.Add(file);
                                        else
                                        {
                                            sms.WriteToJSON();
                                            ImportedFiles.Add(new("SMS", file));

                                        }
                                        break;
                                    case 'E':
                                        Email email = new(header, body);
                                        if (email.Sender == "Unknown")
                                            UnreadableFiles.Add(file);
                                        else if (string.IsNullOrWhiteSpace(email.Object))
                                            UnreadableFiles.Add(file);
                                        else if (string.IsNullOrWhiteSpace(email.Text))
                                            UnreadableFiles.Add(file);
                                        else
                                        {
                                            email.WriteToJSON();
                                            ImportedFiles.Add(new("Email", file));
                                        }
                                        break;
                                    case 'T':
                                        Tweet tweet = new(header, body);
                                        if (tweet.Sender == "Unknown")
                                            UnreadableFiles.Add(file);
                                        else if (string.IsNullOrWhiteSpace(tweet.Text))
                                            UnreadableFiles.Add(file);
                                        else if (tweet.Text.Length > 140)
                                            UnreadableFiles.Add(file);
                                        else
                                        {
                                            tweet.WriteToJSON();
                                            ImportedFiles.Add(new("Email", file));
                                        }
                                        break;
                                    default:
                                        UnreadableFiles.Add(file);
                                        break;
                                }
                            }
                            else
                            {
                                UnreadableFiles.Add(file);
                            }
                        }
                        else
                        {
                            UnreadableFiles.Add(file);
                        }
                    }
                    else
                    {
                        UnreadableFiles.Add(file);
                    }
                }
            }
            string successInfo = "Successfully imported the following files:\n";
            foreach (Tuple<string,string> message in ImportedFiles)
            {
                successInfo += $"{message.Item1}: {message.Item2}\n";
            }
            MessageBox.Show(successInfo, "Successfull import", MessageBoxButton.OK, MessageBoxImage.Information);
            UnreadFiles = new ObservableCollection<string>(UnreadableFiles);
            OnChanged(nameof(UnreadFiles));
        }
    }
}
