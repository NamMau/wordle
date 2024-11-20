using System;
using System.IO;
using System.Linq;

namespace WordleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Link file path to project
            string filePath = @"D:\1651\code\Wordle\WordleGame\wordst.txt";

            // Check if the word list file exists; if not, print an error and exit
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Word list file not found.");
                return;
            }

            // Read the word list, filter for words of exactly 5 letters, and convert them to uppercase
            var wordList = File.ReadAllLines(filePath)
                               .Where(w => w.Length == 5) // Only words with 5 letters
                               .Select(w => w.ToUpper()) // Convert words to uppercase
                               .ToArray();

            // If the word list is empty, exit the program
            if (wordList.Length == 0) return;

            // Randomly select a target word from the word list
            string targetWord = wordList[new Random().Next(wordList.Length)];
            bool isWin = false; // Flag to track if the user wins
            int currentTurn = 1; // Track the number of turns

            // Game play user has max 6 turns
            while (currentTurn <= 6 && !isWin)
            {
                // Print the current turn number
                Console.WriteLine($"Try {currentTurn}:");

                string guess = string.Empty;

                // Until the user enters a valid guess of exactly 5 letters
                // and the word is valid (exists in the word list)
                while (guess.Length != 5 || !wordList.Contains(guess.ToUpper()))
                {
                    Console.Write("Enter a 5-letter word: ");
                    guess = Console.ReadLine().ToUpper(); // Convert to uppercase
                    if (guess.Length != 5)
                    {
                        Console.WriteLine("Invalid input. Please enter exactly 5 letters.");
                    }
                    else if (!wordList.Contains(guess))
                    {
                        Console.WriteLine("Invalid word. Please enter a valid word from the word list.");
                    }
                }

                char[] feedback = new char[5];
                bool[] targetUsed = new bool[5]; // Track which letters in the target word have been used

                // Check exact matches (green - 'G')
                for (int i = 0; i < 5; i++)
                    feedback[i] = (guess[i] == targetWord[i]) ? 'G' : 'X'; // Mark 'G' for correct letters, 'X' for incorrect

                // Check for letters that are in the target word but in the wrong position (yellow - 'Y')
                for (int i = 0; i < 5; i++)
                {
                    if (feedback[i] == 'G') targetUsed[i] = true; // Skip letters already marked as correct

                    else
                    {
                        // Check if the letter in the guess exists in the target word and has not been used yet
                        for (int j = 0; j < 5 && feedback[i] != 'G'; j++)
                        {
                            if (!targetUsed[j] && guess[i] == targetWord[j])
                            {
                                feedback[i] = 'Y'; // Mark 'Y' for correct letter in the wrong position
                                targetUsed[j] = true; // Mark this target letter as used
                            }
                        }
                    }
                }

                // Print feedback after processing the guess
                Console.WriteLine(new string(feedback));

                // Check if the guess was correct (all 'G's)
                if (new string(feedback) == "GGGGG")
                {
                    Console.WriteLine("You win!");
                    isWin = true; // Set win flag to true
                }

                currentTurn++; // Move to the next turn
            }

            // If the user didn't win after 6 tries, print the losing message
            if (!isWin) Console.WriteLine($"You lose. The correct word was: {targetWord}");
        }
    }
}
