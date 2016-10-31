using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rohankapoor.AutoPrompt.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PromptForPath();
        }

        private static void PromptForPath()
        {
            Console.WriteLine("                         Welcome to demo of AutoPrompt APIs");
            Console.WriteLine("\n\n1. Demo of prompt with initial input:\n");
            var userInput1 = AutoPrompt.PromptForInput("Q: How do you feel? (Press backspce to edit)\nA: ", "Awesome");
            Console.WriteLine("User feels '" + userInput1+ "'");

            Console.WriteLine("\n\n2. Demo of prompt with options:\n");
            var userInput2 = AutoPrompt.PromptForInput("Q: Choose occupation (Press up/down arrow to choose, Backspace to edit)\nA: ", new string[] { "Engineer", "Scientist", "Salesman", "Manager", "TopManagement" });
            Console.WriteLine("Occupation selected: '" + userInput2 + "'");

            Console.WriteLine("\n\n3. Demo of prompt with options with edits disallowed :\n");
            var userInput3 = AutoPrompt.PromptForInput("Q: Choose sex (Press up/down arrow to choose)\nA: ", new string[] { "Male", "Female", "Others"}, false);
            Console.WriteLine("Occupation selected: '" + userInput3 + "'");

            Console.WriteLine("\n\n4. Demo of prompt with searchable options:\n");
            var userInput4 = AutoPrompt.PromptForInput_Searchable("Q: Choose city (Press up/down arrow or type name)\nA: ", new string[] { "New York City", "San Fransisco", "New Delhi", "Bangalore", "Tokyo" });
            Console.WriteLine("City selected: '" + userInput4 + "'");

            Console.WriteLine("\n\n5. Demo of prompt for password:\n");
            var userInput5 = AutoPrompt.GetPassword("Q: Enter password: ");
            Console.WriteLine("Password entered: '" + userInput5 + "'");

            Console.WriteLine("\n\n6. Demo of prompt for file/folder:\n");
            var userInput6 = AutoPrompt.GetPath("Q: Enter path of file:\n\tUp/down arrow to go through files in current directory.\n\t'\\' to step into directory\n\tBackspace to edit\n\tOr just type to go to nearest search match\n\nA: ");
            Console.WriteLine("Path entered: '" + userInput6 + "'");

            Console.ReadLine();
        }
    }
}
