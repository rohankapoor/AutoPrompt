# AutoPrompt [![Travis](https://img.shields.io/travis/rohankapoor/AutoPrompt.svg)](https://travis-ci.org/rohankapoor/AutoPrompt) [![NuGet](https://img.shields.io/nuget/v/rohankapoor.AutoPrompt.svg)](https://www.nuget.org/packages/rohankapoor.AutoPrompt)

Auto complete command prompts for C# .Net console apps. This library provides APIs that can be used to accept input from the command line with added functionality.

The user can supply prefilled input, edit it, use Up or Down arrow to go throught the input options supplied or even press a key to go to the closest maching option
Directory or file path is also autofilled based on the contents of directory or drive choosen:

## Installation

To install AutoPrompt, run the following command in the Package Manager Console

```
PM> Install-Package rohankapoor.AutoPrompt
```

Or

In Visual Studio solution, right click and Manage Nuget Packages. Search online for 'rohankapoor' or 'autoprompt'.

## Overview of APIs

  * [Prompt for path of directory or file](#prompt-for-path-of-directory-or-file)
  * [Prompt with initial input](#prompt-with-initial-input)
  * [Prompt with a set of options](#prompt-with-a-set-of-options)
  * [Prompt with a set of non-editable options](#prompt-with-a-set-of-non-editable-options)
  * [Prompt with a set of searchable options](#prompt-with-a-set-of-searchable-options)
  * [Prompt for password](#prompt-for-password)
  

### Prompt for path of directory or file

Prompts the user on command line for a path. This paths are auto completed based on the drive/directory/file choose.
User can:
- Press Up/Down arrow to choose next drive or directory or file in current path
- Press \ to navigate into the drive or directory (the Up/Down arrow to go through current files and dirs)
- Key in a character after \ to go to the closest matching file or directory on current path
- Backspace to edit the path

#####Usage:
```
string userInput = AutoPrompt.GetPath("Choose certification file :\n\tUp/down arrow to go through files in current directory.\n\t'\\' to step into directory\n\tBackspace to edit\n\tOr just type to go to nearest search match\n\n>");
```

### Prompt with initial input

Prompts the user for input on command line with a prompt string as well as a pre filled input which can be edited

#####Usage:
```
string userInput = AutoPrompt.PromptForInput("How do you feel? (Press backspce to edit) :", "Awesome");
```



### Prompt with a set of options

Prompts the user for input on command line with a prompt string as well as a set of option that can be choosen using Up and Down arrow. The input is editable as well

#####Usage:
```
string userInput = AutoPrompt.PromptForInput("Choose occupation (Press up/down arrow to choose) :", new string[] { "Engineer", "Scientist", "Salesman", "Manager", "TopManagement" });
```

### Prompt with a set of non-editable options

Prompts the user for input on command line with a prompt string as well as a set of option. User can only choose among various options using Up and Down arrow. 

#####Usage:
```
string userInput = AutoPrompt.PromptForInput("Choose occupation (Press up/down arrow to choose) :", new string[] { "Engineer", "Scientist", "Salesman", "Manager", "TopManagement" }, false);
```


### Prompt with a set of searchable options

Prompts the user for input on command line with a prompt string as well as a set of option that can be choosen using Up and Down arrow. The input is editable and searchable.

To search, key in an alphabet and the option closest match to the char key'd in will be set as user input

#####Usage:
```
string userInput = AutoPrompt.PromptForInput_Searchable("Choose city (Press up/down arrow or type name) :", new string[] { "New York City", "San Fransisco", "New Delhi", "Bangalore", "Tokyo" });
```


### Prompt for password

Prompts the user for password input on command line. The entered password is masked with * echoded on the console with each characted of the password key'd in

#####Usage:
```
string userInput = AutoPrompt.GetPassword("Enter password :");
```


