using System;
using System.IO;
using System.Linq;

namespace WordleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // link the file to project
            string filePath = @"D:\1651\code\Wordle\WordleGame\wordst.txt";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Word list file not found. Please check the file path and try again.");
                return;
            }

            // the word must be match with length of letters = 5
            string[] wordList = File.ReadAllLines(filePath)
                                    .Where(w => w.Length == 5)
                                    .Select(w => w.ToUpper())
                                    .ToArray();

            if (wordList.Length == 0)
            {
                Console.WriteLine("Word list is empty or does not contain valid 5-letter words.");
                return;
            }

            Random random = new Random();
            string targetWord = wordList[random.Next(wordList.Length)]; //choose randomly targetword and has length is contains 5 letters

            int maxTurns = 6;
            bool isWin = false;

            //whenever currentTurn <= maxTurns, user can keep guess until the currentTurn bigger than 6 the program will shut
            for (int currentTurn = 1; currentTurn <= maxTurns; currentTurn++)
            {
                Console.WriteLine($"Try {currentTurn}:");

                string guess;
                do
                {
                    Console.Write("Enter a 5-letter word: ");
                    guess = Console.ReadLine().ToUpper();

                    if (guess.Length != 5)
                    {
                        Console.WriteLine("Invalid input. Please enter a 5-letter word.");
                    }
                } while (guess.Length != 5);

                // Evaluate the guess of user and print feedback
                char[] feedback = EvaluateGuess(guess, targetWord);
                Console.WriteLine(new string(feedback));

                if (new string(feedback) == "GGGGG")
                {
                    Console.WriteLine("You win!");
                    isWin = true;
                    break;
                }
            }

            if (!isWin)
            {
                Console.WriteLine($"You lose. The correct word was: {targetWord}");
            }

            Console.ReadLine();
        }

        static char[] EvaluateGuess(string guess, string targetWord)
        {
            char[] feedback = new char[5];
            bool[] targetUsed = new bool[5];

            // Mark correct as G means "green", correct letter at the corresponding position
            for (int i = 0; i < 5; i++)
            {
                if (guess[i] == targetWord[i])
                {
                    feedback[i] = 'G';
                    targetUsed[i] = true; // Mark this character as used
                }
                else
                {
                    feedback[i] = 'X'; // Default to 'X' (gray)
                }
            }

            // Mark correct letters in the wrong positions as 'Y'
            for (int i = 0; i < 5; i++)
            {
                if (feedback[i] == 'G') continue; // Skip already marked 'G'

                for (int j = 0; j < 5; j++)
                {
                    if (!targetUsed[j] && guess[i] == targetWord[j])
                    {
                        feedback[i] = 'Y';
                        targetUsed[j] = true; // Mark this character as used
                        break;
                    }
                }
            }
            return feedback;
        }
    }
}
