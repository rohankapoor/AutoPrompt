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
            var userInput = AutoPrompt.GetPath("Enter path: ");

            Console.WriteLine("Path entered by user : " + userInput);

            Console.ReadLine();
        }
    }
}
