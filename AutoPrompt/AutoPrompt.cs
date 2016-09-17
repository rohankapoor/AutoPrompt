using System;
using System.Collections.Generic;
using System.IO;

namespace rohankapoor.AutoPrompt
{
    public class AutoPrompt
    {
        #region Public APIs

        /// <summary>
        /// Used to get path from command prompt. Starts by showing available directories
        /// Press:
        ///      Up Arrow to go to next file or directory
        ///      Down Arrow to go to previous file or directory
        ///      '\' to navigate into current directory
        ///      Any key to list matching files and directory. The search criteria is 'begins with'. Eg, When 'W' is pressed in C:\, the prompt will auto fill with 'Windows' as the closest match. The search is case sensitive
        /// </summary>
        /// <param name="message">Message that appears on the prompt. For eg, Enter Value:</param>
        /// <returns>Full path of the file or directory selected</returns>
        public static string GetPath(string message)
        {
            string[] options = Environment.GetLogicalDrives();

            for (int optionsIndex = 0; optionsIndex < options.Length; optionsIndex++)
            {
                options[optionsIndex] = options[optionsIndex].Remove(options[optionsIndex].Length - 1);
            }

            string userInput = options[0];
            int currentOptionDisplayedIndex = 0;
            int optionCount = options.Length;

            ConsoleKeyInfo KeyInfo;
            Console.Write(message + options[0]);

            KeyInfo = Console.ReadKey(true);
            while (KeyInfo.Key != ConsoleKey.Enter)
            {
                if (Char.IsLetterOrDigit(KeyInfo.KeyChar) || Char.IsSymbol(KeyInfo.KeyChar)
                    || (Char.IsPunctuation(KeyInfo.KeyChar) && KeyInfo.KeyChar != '\\')
                    || (Char.IsPunctuation(KeyInfo.KeyChar) && KeyInfo.KeyChar != '\\')
                    || Char.IsWhiteSpace(KeyInfo.KeyChar)
                    )
                {
                    Console.Write(KeyInfo.KeyChar);

                    userInput += KeyInfo.KeyChar;

                    // Performing Closest Search
                    int FoundIndex = -1;
                    int OptionIndex = 0;
                    foreach (string Option in options)
                    {
                        if (Option.ToLower().StartsWith(userInput.ToLower()))
                        {
                            FoundIndex = OptionIndex;

                            // Display that Option Erasing Old
                            for (int UserInputIndex = 0; UserInputIndex < userInput.Length; UserInputIndex++)
                            {
                                Console.Write("\b" + " " + "\b");
                            }

                            // Display new
                            userInput = options[FoundIndex];
                            currentOptionDisplayedIndex = FoundIndex;

                            Console.Write(options[currentOptionDisplayedIndex]);

                            break;
                        }
                        OptionIndex++;
                    }
                }
                else if (!userInput.Equals(String.Empty) && KeyInfo.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b" + " " + "\b");

                    if (!string.IsNullOrEmpty(userInput))
                    {
                        userInput = userInput.Substring(0, userInput.Length - 1);
                    }
                }
                else if (!userInput.Equals(String.Empty) && KeyInfo.KeyChar == '\\')
                {
                    options = GetFilesDirs(userInput + @"\").ToArray();
                    if (options.Length>0)
                    {
                        Erase(userInput);
                        Console.Write(options[0]);

                        userInput = options[0];
                        optionCount = options.Length;
                        currentOptionDisplayedIndex = 0;
                    }
                    else
                    {
                        // Not a directory. No action required.
                    }
                }
                else if (!userInput.Equals(String.Empty) && KeyInfo.Key == ConsoleKey.Delete)
                {
                    // Erase everything
                    for (int UserInputIndex = 0; UserInputIndex < userInput.Length; UserInputIndex++)
                    {
                        Console.Write("\b" + " " + "\b");
                    }

                    // Set UserInput to Empty
                    userInput = "";

                }
                else if (KeyInfo.Key == ConsoleKey.UpArrow || KeyInfo.Key == ConsoleKey.DownArrow)
                {
                    // Need to display another option

                    // Erase on screen and in UserInput string
                    for (int UserInputIndex = 0; UserInputIndex < userInput.Length; UserInputIndex++)
                    {
                        Console.Write("\b" + " " + "\b");
                    }

                    // Display new option
                    if (KeyInfo.Key == ConsoleKey.UpArrow)
                    {
                        if (currentOptionDisplayedIndex > 0)
                        {
                            currentOptionDisplayedIndex--;
                        }
                        else
                        {
                            currentOptionDisplayedIndex = optionCount - 1;
                        }
                    }
                    else if (KeyInfo.Key == ConsoleKey.DownArrow)
                    {
                        if (currentOptionDisplayedIndex < optionCount - 1)
                        {
                            currentOptionDisplayedIndex++;
                        }
                        else
                        {
                            currentOptionDisplayedIndex = 0;
                        }
                    }

                    // Show it
                    Console.Write(options[currentOptionDisplayedIndex]);

                    // Set new user input
                    userInput = options[currentOptionDisplayedIndex];
                }

                KeyInfo = Console.ReadKey(true);
            }

            Console.WriteLine();

            return userInput;
        }

        /// <summary>
        /// Asks the user for input, with a default input value already set
        /// </summary>
        /// <param name="message">Message that appears on the prompt. For eg, Enter Value:</param>
        /// <param name="initialInput">The value prefilled. Eg, "22 Jump Street" prefilled if prompt is to accept address</param>
        /// <returns>User entered string or 'initialInput' if no edits were made</returns>
        public static string PromptForInput(string message, string initialInput)
        {
            string userInput = initialInput;

            ConsoleKeyInfo KeyInfo;

            Console.Write(message);
            Console.Write(initialInput);

            KeyInfo = Console.ReadKey(true);
            while (KeyInfo.Key != ConsoleKey.Enter)
            {
                if (Char.IsLetterOrDigit(KeyInfo.KeyChar) || Char.IsSymbol(KeyInfo.KeyChar)
                    || Char.IsPunctuation(KeyInfo.KeyChar) || Char.IsPunctuation(KeyInfo.KeyChar)
                    || Char.IsWhiteSpace(KeyInfo.KeyChar))
                {
                    // If its a Letter or Digit, echo to screen, add to Input String
                    Console.Write(KeyInfo.KeyChar);

                    userInput += KeyInfo.KeyChar;
                }
                else if (!userInput.Equals(String.Empty) && KeyInfo.Key == ConsoleKey.Backspace)
                {
                    //User hit back space

                    // Erasing on Console
                    Console.Write("\b" + " " + "\b");

                    // Erasing in Input String
                    if (!String.IsNullOrEmpty(userInput))
                    {
                        userInput = userInput.Substring(0, userInput.Length - 1);
                    }
                }

                // Getting a next key from console
                KeyInfo = Console.ReadKey(true);
            }
            Console.WriteLine();

            return userInput.Trim();
        }

        /// <summary>
        /// Accepts user input by displaying options. The input is editable as well
        /// Press:
        ///     Up Arrow to go to next option
        ///     Down Arrow to go to previous option
        /// </summary>
        /// <param name="message">Message that appears on the prompt. For eg, Enter Value:</param>
        /// <param name="options">List of values. Eg {Mr, Mrs, Ms} can be passed if prompt if for salutation. User can switch between these values by pressing up/down arrow OR enter something like Dr or Er and hit enter</param>
        /// <returns>User entered string or one of the string in 'options' choosen by the user</returns>
        public static string PromptForInput(string message, string[] options)
        {
            int OptionCount = options.Length;
            int CurrentOptionDisplayedIndex = 0;
            string UserInput = options[0];

            ConsoleKeyInfo KeyInfo;

            Console.Write(message + options[0]);

            KeyInfo = Console.ReadKey(true);
            while (KeyInfo.Key != ConsoleKey.Enter)
            {
                if (Char.IsLetterOrDigit(KeyInfo.KeyChar) || Char.IsSymbol(KeyInfo.KeyChar)
                    || Char.IsPunctuation(KeyInfo.KeyChar) || Char.IsPunctuation(KeyInfo.KeyChar)
                    || Char.IsWhiteSpace(KeyInfo.KeyChar))
                {
                    Console.Write(KeyInfo.KeyChar);

                    UserInput += KeyInfo.KeyChar;
                }
                else if (!UserInput.Equals(String.Empty) && KeyInfo.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b" + " " + "\b");

                    if (!String.IsNullOrEmpty(UserInput))
                    {
                        UserInput = UserInput.Substring(0, UserInput.Length - 1);
                    }
                }
                else if (KeyInfo.Key == ConsoleKey.UpArrow || KeyInfo.Key == ConsoleKey.DownArrow)
                {
                    // We have to display another option

                    // Erase on screen and in UserInput string
                    for (int UserInputIndex = 0; UserInputIndex < UserInput.Length; UserInputIndex++)
                    {
                        Console.Write("\b" + " " + "\b");
                    }

                    // Display new option
                    if (KeyInfo.Key == ConsoleKey.UpArrow)
                    {
                        if (CurrentOptionDisplayedIndex > 0)
                        {
                            CurrentOptionDisplayedIndex--;
                        }
                        else
                        {
                            CurrentOptionDisplayedIndex = OptionCount - 1;
                        }
                    }
                    else if (KeyInfo.Key == ConsoleKey.DownArrow)
                    {
                        if (CurrentOptionDisplayedIndex < OptionCount - 1)
                        {
                            CurrentOptionDisplayedIndex++;
                        }
                        else
                        {
                            CurrentOptionDisplayedIndex = 0;
                        }
                    }

                    // Show the new option
                    Console.Write(options[CurrentOptionDisplayedIndex]);

                    // Set new user input
                    UserInput = options[CurrentOptionDisplayedIndex];

                }
                KeyInfo = Console.ReadKey(true);
            }
            Console.WriteLine();

            return UserInput.Trim();
        }

        /// <summary>
        /// Accepts user input by displaying options. The options are autofilled with matches from key press. This is like auto suggest for a text box
        /// Press:
        ///     Up Arrow to go to next option
        ///     Down Arrow to go to previous option
        ///     Any key to list closest matching option. The search criteria is 'begins with'. Eg, When 'C' is pressed and options is a list of US states, then 'California' is set to as choosen option. The search is case sensitive
        /// </summary>
        /// <param name="message">Message that appears on the prompt. For eg, Enter Value:</param>
        /// <param name="options">Input options that the user can choose from using up-down arrow or type to get to</param>
        /// <returns>User entered string or one of the string in 'options' choosen by the user</returns>
        public static string PromptForInput_Searchable(string message, string[] options)
        {
            int OptionCount = options.Length;
            int CurrentOptionDisplayedIndex = 0;
            string UserInput = options[0];

            ConsoleKeyInfo KeyInfo;

            Console.Write(message + options[0]);

            KeyInfo = Console.ReadKey(true);
            while (KeyInfo.Key != ConsoleKey.Enter)
            {

                if (Char.IsLetterOrDigit(KeyInfo.KeyChar) || Char.IsSymbol(KeyInfo.KeyChar)
                    || Char.IsPunctuation(KeyInfo.KeyChar) || Char.IsPunctuation(KeyInfo.KeyChar)
                    || Char.IsWhiteSpace(KeyInfo.KeyChar))
                {
                    Console.Write(KeyInfo.KeyChar);

                    UserInput += KeyInfo.KeyChar;

                    // Performing Closest Search
                    int FoundIndex = -1;
                    int OptionIndex = 0;
                    foreach (string Option in options)
                    {
                        if (Option.ToLower().StartsWith(UserInput.ToLower()))
                        {
                            FoundIndex = OptionIndex;

                            /* Display that Option */
                            /* Erasing Old */
                            for (int UserInputIndex = 0; UserInputIndex < UserInput.Length; UserInputIndex++)
                            {
                                Console.Write("\b" + " " + "\b");
                            }

                            /* display new */
                            UserInput = options[FoundIndex];
                            CurrentOptionDisplayedIndex = FoundIndex;

                            Console.Write(options[CurrentOptionDisplayedIndex]);

                            break;
                        }
                        OptionIndex++;
                    }
                    // Closest search ends
                }
                else if (!UserInput.Equals(String.Empty) && KeyInfo.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b" + " " + "\b");

                    if (!string.IsNullOrEmpty(UserInput))
                    {
                        UserInput = UserInput.Substring(0, UserInput.Length - 1);
                    }
                }
                else if (!UserInput.Equals(String.Empty) && KeyInfo.Key == ConsoleKey.Delete)
                {
                    // Erase it all
                    for (int UserInputIndex = 0; UserInputIndex < UserInput.Length; UserInputIndex++)
                    {
                        Console.Write("\b" + " " + "\b");
                    }

                    // Set UserInput to Empty
                    UserInput = "";

                }
                else if (KeyInfo.Key == ConsoleKey.UpArrow || KeyInfo.Key == ConsoleKey.DownArrow)
                {
                    // Display another option

                    // Erase on screen and in UserInput string
                    for (int UserInputIndex = 0; UserInputIndex < UserInput.Length; UserInputIndex++)
                    {
                        Console.Write("\b" + " " + "\b");
                    }

                    // Display new option
                    if (KeyInfo.Key == ConsoleKey.UpArrow)
                    {
                        if (CurrentOptionDisplayedIndex > 0)
                            CurrentOptionDisplayedIndex--;
                        else
                            CurrentOptionDisplayedIndex = OptionCount - 1;

                    }
                    else if (KeyInfo.Key == ConsoleKey.DownArrow)
                    {
                        if (CurrentOptionDisplayedIndex < OptionCount - 1)
                            CurrentOptionDisplayedIndex++;
                        else
                            CurrentOptionDisplayedIndex = 0;
                    }

                    // Show new option
                    Console.Write(options[CurrentOptionDisplayedIndex]);

                    // Set new user input
                    UserInput = options[CurrentOptionDisplayedIndex];
                }

                KeyInfo = Console.ReadKey(true);
            }
            Console.WriteLine();

            return UserInput;
        }

        /// <summary>
        /// Gets a password without displaying it on screen. Input is masked by *s
        /// </summary>
        /// <param name="message">Message that appears on the prompt. For eg, Enter Value:</param>
        /// <returns>The password entered by the user</returns>
        public static string GetPassword(string message)
        {
            Console.Write(message);

            string PasswordString = "";
            ConsoleKeyInfo KeyInfo;

            KeyInfo = Console.ReadKey(true);
            while (KeyInfo.Key != ConsoleKey.Enter)
            {
                if (Char.IsLetterOrDigit(KeyInfo.KeyChar))
                {
                    // If its a Letter or Digit, echo * to screen, add to Input String
                    Console.Write("*");
                    PasswordString += KeyInfo.KeyChar;
                }
                else if (PasswordString != String.Empty && KeyInfo.Key == ConsoleKey.Backspace)
                {
                    // User hit back space

                    // Erasing on Console
                    Console.Write("\b" + " " + "\b");

                    // Erasing in Input String
                    if (!string.IsNullOrEmpty(PasswordString))
                    {
                        PasswordString = PasswordString.Substring(0, PasswordString.Length - 1);
                    }
                }

                KeyInfo = Console.ReadKey(true);
            }
            Console.WriteLine();

            return PasswordString;
        }

        #endregion

        #region Utils

        private static void Erase(string userInput)
        {
            for (int i = 0; i < userInput.Length; i++)
            {
                Console.Write("\b" + " " + "\b");
            }
        }

        private static List<string> GetFilesDirs(string userInput)
        {
            List<string> result = new List<string>();
            if (Directory.Exists(userInput))
            {
                try
                {
                    var directories = Directory.GetDirectories(userInput);
                    result.AddRange(directories);
                    var files = Directory.GetFiles(userInput);
                    result.AddRange(files);
                }
                catch (Exception e)
                {

                }
            }

            return result;
        }

        #endregion
    }
}
