# Software Engineering Coursework

This project is a coursework for the *SET09102* **Software Engineering** module at Napier University, first semester of 2021/2022. The project is a messaging service that validates, sanitises and categorises incoming messages to Napier Bank in the form of SMS text messages, emails and Tweets. It takes the form of a desktop application that is written in **C#** and **WPF**, and uses the MVVM pattern.

Release date: 2021-11-25

## About

The main goal of this coursework was to realise a software that is able to validate, sanitize and categorise incoming messages to the system, in the form of SMS, Emails and Tweets. We also had to create an intuitive user interface that would allow users to write messages directly into the software, as well as read saved messages, and ask to import more from an external source. It was also requested to display trending, mentions and Significant Incident Reports lists, I gathered under the read statistics requirement.
The User Interface had to take the form of a desktop application. I chose to create a WPF application that is based on the C# programming language for the logic of the app, while the User Interface is designed with the Extensible Application Markup Language (XAML) language.

## Goal and requirements

### Requirements

Detailed requirements for the project can be found in the [requirements document](Requirements.pdf).

### Requirements summary

You should complete the following tasks by exercising advanced software development technologies you learnt in this module :

- Develop a **WPF** application using **C#** that realises all the functionalities of the NBM service
- The system must deal with three types of message, all messages are strings of ASCII characters that have a Message Header comprising a Message ID (Message-type “S”,”E” or “T” followed by 9 numeric characters, e.g. “E1234567701”) followed by the Body of the message.
  - **SMS** messages
  - **Emails**
  - **Tweets**
- Messages must be processed as follows:
  - **SMS** Messages: Textspeak abbreviations must be expanded to their full form enclosed in `“<...>”`
  - **Standard email** messages will contain text. Any URLs contained in messages will be removed and written to a quarantine list and replaced by `“<URL Quarantined>”` in the body of the message.
  - **Significant Incident Reports** will have the Subject in the form “SIR dd/mm/yy” and will comprise a message body as above. The message body will begin with the following standard texts on the first two lines:
    - **Sort Code**: 99-99-99
    - **Nature of Inciden**t: which will be one of the following (see under):

    |Theft|Staff Attack|ATM Theft|Raid|Customer Attack|Staff Abuse|Bomb Threat|Terrorism|Suspicious Incident|Intelligence|Cash Loss|
    |-----|------------|---------|----|---------------|-----------|-----------|---------|-------------------|------------|---------|

    Sort Code and Nature of Incident will be written to a SIR list. Any URLs contained in messages will be removed and written to a quarantine list and replaced by `“<URL Quarantined>”` in the body of the message
  - **Tweets**: Textspeak abbreviations will be expanded (as in SMS messages above). Hashtags will be added to a hashtag list that will count the number of uses of each to produce a trending list. “Mentions”, i.e. embedded Twitter IDs will be added to a mentions list.
- Testing: Develop test cases and construct tests to verify that messages are processed correctly for each type of message. Use Visual Studio testing facilities (or equivalence on the platform you have chosen) to conduct your tests where appropriate.

Additional requirement:

- Modify your system so that the messages are read from a text file and processed and displayed one-by-one on screen. You can design the structure of this input text file yourself, but it shouldn’t be a JSON file.

### Implemented features

I implemented all the requirements listed above, and I also added a few extra features to the application:

- **User Interface**: I created a user interface that is intuitive and easy to use. The user can write messages directly into the application, and read them from the application. The user can also ask to import more messages from an external source, and the application will read them from a text file and display them one by one on screen. He can read the messages from a table where each message is displayed in a row, and he can also read indiviually each message by clicking on it.
- **Validation**: The application validates the messages, and displays an error message if the message is not valid. The message is not valid if it does not have a Message Header, or if the Message Header is not in the correct format.
- There are two ways for the user to write a message in the app, the first one is the one asked in the requirements, where the user has to write both the header and the body of the message from scratch. The second one is a more intuitive way, where the user can write the body of the message by filling in the fields of a form, and the application will automatically generate the header for the message. The user can also choose the type of message he wants to write, and the application will generate the header accordingly.
- **Statistics**: The application displays statistics about the messages, such as the trending list, the mentions list and the Significant Incident Reports list.
- **Custom styles**: I created custom styles for the application, so that it looks more professional and intuitive.

Testings were implemented using the Visual Studio testing environment.

## Key learnings

This project was a great opportunity to learn about the WPF framework, and to practice the MVVM pattern. I also learned how to use the Visual Studio testing environment, and how to implement unit tests. It was also an opportunity for me to work alone on every aspect of the project, from the design to the implementation, and to learn how to manage a project on my own. It helped me to improve my skills in C# and XAML, and to learn how to use the WPF framework, therefore working on both front-end and back-end aspects of the project. I am very happy with the result, and I am proud of what I achieved.

## Installation

You can download the project from the [download page](https://github.com/Ahddry/Software-Engineering-Coursework-BLAIR/raw/main/Software-Engineering-Coursework-BLAIR.zip), or compile it yourself from the source code with Visual Studio. The project was developed using Visual Studio 2019, and it should work with any version of Visual Studio 2019 or higher. You can download Visual Studio from [here](https://visualstudio.microsoft.com/downloads/).

## Contributing

- Adrien Blair [@Ahddry](https://github.com/Ahddry)

## Copyright

MIT License

> Students, you are free to use this project for any purpose, but please do not submit it as your own work, it will prevent you from being caught for plagiarism.

## Screenshots

![Screenshot 1](/Screenshots/nbm.jpg)

![Screenshot 2](/Screenshots/nbm2.jpg)

![Screenshot 3](/Screenshots/nbm3.jpg)

![Screenshot 4](/Screenshots/nbm4.jpg)
