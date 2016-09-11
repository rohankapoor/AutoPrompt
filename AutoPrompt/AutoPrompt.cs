using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rohankapoor.AutoPrompt
{
    public class AutoPrompt
    {
        // Used to get path from command prompt
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

            do
            {
                KeyInfo = Console.ReadKey(true);

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
                    userInput = userInput + @"\";

                    options = GetFilesDirs(userInput).ToArray();

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
            }
            while (KeyInfo.Key != ConsoleKey.Enter);

            return userInput;
        }

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
    }
}
