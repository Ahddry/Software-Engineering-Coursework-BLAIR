﻿using Coursework1.Core;
using Coursework1.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// View to easily create an email
    /// </summary>
    public class EmailCreationViewModel : BaseViewModel
    {
        public string EmailAddress { get; set; } //Email Address text box text
        public string NatureOfIncident { get; set; } //Email Address text box text
        public List<string> ListOfIncidents { get; set; }//List of all the possible Natures of Incidents
        public CollectionView NaturesOfIncident { get; set; } //List of all the possible Natures of Incidents, to be displayed
        public string SelectedItem //Selected Item of the screen
        {
            get => NatureOfIncident;
            set
            {
                if (NatureOfIncident == value) return;
                NatureOfIncident = value;
                OnChanged(nameof(SelectedItem));
            }
        }
        public string Object { get; set; } //Email Address text box text
        public string Text { get; set; }//Text text box text
        public bool SIR { get; set; }//Text text box text
        public ICommand SIROnCommand { get; private set; }//Save button control
        public ICommand SIROffCommand { get; private set; }//Save button control
        public ICommand SaveButtonCommand { get; private set; }//Save button control
        /// <summary>
        /// View to easily create an email
        /// </summary>
        public EmailCreationViewModel()
        {
            #region Setup
            EmailAddress = string.Empty;//Initialization of the content of the text boxes
            NatureOfIncident = string.Empty;
            ListOfIncidents = new(); //Add all the possible Natures of incident
            ListOfIncidents.Add("Theft");
            ListOfIncidents.Add("Staff Attack");
            ListOfIncidents.Add("ATM Theft");
            ListOfIncidents.Add("Raid");
            ListOfIncidents.Add("Customer Attack");
            ListOfIncidents.Add("Staff Abuse");
            ListOfIncidents.Add("Bomb Threat");
            ListOfIncidents.Add("Terrorism");
            ListOfIncidents.Add("Suspicious Incident");
            ListOfIncidents.Add("Intelligence");
            ListOfIncidents.Add("Cash Loss");
            NaturesOfIncident = new CollectionView(ListOfIncidents);
            Object = string.Empty;
            Text = string.Empty;
            SIR = false;
            SIROnCommand = new RelayCommand(SIROnButtonClick);//Binding the SIROn radiobutton control to its function
            SIROffCommand = new RelayCommand(SIROffButtonClick);//Binding the SIROff button control to its function
            SaveButtonCommand = new RelayCommand(SaveButtonClick);//Binding the Save button control to its function
            #endregion
        }
        /// <summary>
        /// Switch to a SIR mail
        /// </summary>
        public void SIROnButtonClick()
        {
            SIR = true;
        }
        /// <summary>
        /// Switch to a standard email
        /// </summary>
        public void SIROffButtonClick()
        {
            SIR = false;
        }
        /// <summary>
        /// Save the created email
        /// </summary>
        public void SaveButtonClick()
        {
            //Regex pattern found on //https://regex101.com/r/SOgUIV/1
            MatchCollection matches = Regex.Matches(EmailAddress, @"^((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$");
            if (string.IsNullOrWhiteSpace(EmailAddress)) //Testing if the PhoneNumber is filled in
            {
                MessageBox.Show("Your Email needs an email address to be sent!");
            }
            else if (string.IsNullOrWhiteSpace(Text))
            {
                MessageBox.Show("Your Email needs a Text!");//Testing if the Text is filled in
            }
            else if (matches.Count == 0)
            {
                MessageBox.Show("Your Email address must be a valid email address");//Testing if the PhoneNumber begins with an @
            }
            else if (matches[0].ToString() != EmailAddress)
            {
                MessageBox.Show("Your Email address must be a valid email address");//Testing if the PhoneNumber begins with an @
            }
            else
            {
                if (SIR) //If the mail is a Significant Incident Report
                {
                    if (string.IsNullOrWhiteSpace(NatureOfIncident))
                    {
                        MessageBox.Show("Your SIR email needs a Nature of Incident!");
                    }
                    else
                    {
                        //Creation of the header
                        string header = "E";
                        Random random = new();
                        for (int i = 0; i < 9; i++)
                        {
                            int number = random.Next(10);
                            header += number.ToString();
                        }
                        //Creation of the Sort code
                        DateTime date = DateTime.Now;
                        string year = date.Year.ToString();
                        string sortCode = $"{date.Day}-{date.Month}-{year[2..]}";
                        //Creation of the body
                        string body = $"{EmailAddress}\nSort Code: {sortCode}\nNature of Incident: {NatureOfIncident}\n{Text}";
                        Email email = new(header, body);
                        email.WriteToJSON(); //Saving as a JSON File
                        EmailAddress = string.Empty;
                        Object = string.Empty;
                        NatureOfIncident = string.Empty;
                        Text = string.Empty;
                        OnChanged(nameof(EmailAddress));
                        OnChanged(nameof(Text));
                        MessageBox.Show("Email saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else //If the email is a standard email
                {
                    if (string.IsNullOrWhiteSpace(Object))
                    {
                        MessageBox.Show("Your email needs an Object!");
                    }
                    else
                    {
                        //Creation of the header
                        string header = "E";
                        Random random = new();
                        for (int i = 0; i < 9; i++)
                        {
                            int number = random.Next(10);
                            header += number.ToString();
                        }
                        //Creation of the body
                        string body = $"{EmailAddress}\n{Object}\n{Text}";
                        Email email = new(header, body);
                        email.WriteToJSON(); //Saving as a JSON File
                        EmailAddress = string.Empty;
                        Object = string.Empty;
                        NatureOfIncident = string.Empty;
                        Text = string.Empty;
                        OnChanged(nameof(EmailAddress));
                        OnChanged(nameof(Text));
                        MessageBox.Show("Email saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}