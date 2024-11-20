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

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Word list file not found.");
            }
            else
            {
                // Filter for words of exactly 5 letters
                var wordList = File.ReadAllLines(filePath)
                                   .Where(w => w.Length == 5) 
                                   .Select(w => w.ToUpper()) 
                                   .ToArray();
                if (wordList.Length > 0)
                {
                    string targetWord = wordList[new Random().Next(wordList.Length)];
                    bool isWin = false;
                    int currentTurn = 1;

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
                            guess = Console.ReadLine().ToUpper();
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
                        bool[] targetUsed = new bool[5];

                        // Check exact matches G
                        for (int i = 0; i < 5; i++)
                            feedback[i] = (guess[i] == targetWord[i]) ? 'G' : 'X';

                        // Check for letters right in the wrong position (Y)
                        for (int i = 0; i < 5; i++)
                        {
                            if (feedback[i] == 'G') targetUsed[i] = true;

                            else
                            {
                                // Check if the letter in the guess exists in the target word and has not been used yet
                                for (int j = 0; j < 5 && feedback[i] != 'G'; j++)
                                {
                                    if (!targetUsed[j] && guess[i] == targetWord[j])
                                    {
                                        feedback[i] = 'Y'; // Mark 'Y' for correct letter in the wrong position
                                        targetUsed[j] = true;
                                    }
                                }
                            }
                        }

                        // Print feedback
                        Console.WriteLine(new string(feedback));

                        if (new string(feedback) == "GGGGG")
                        {
                            Console.WriteLine("You win!");
                            isWin = true;
                        }

                        currentTurn++; 
                    }

                    // If the user didn't win after 6 tries, print the losing message
                    if (!isWin) Console.WriteLine($"You lose. The correct word was: {targetWord}");
                }
                else
                {
                    Console.WriteLine("The word list is empty.");
                }
            }
        }
    }
}
