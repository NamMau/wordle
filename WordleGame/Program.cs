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

                if (wordList.Length == 0)
                {
                    Console.WriteLine("The word list is empty.");
                }
                else
                {
                    // The number of games they want to play
                    int numberOfGames = 0;
                    while (numberOfGames <= 0)
                    {
                        Console.Write("How many games would you like to play? ");
                        if (!int.TryParse(Console.ReadLine(), out numberOfGames) || numberOfGames <= 0)
                        {
                            Console.WriteLine("Please enter a valid number greater than 0.");
                        }
                    }

                    int gamesSolved = 0;
                    int gamesUnsolved = 0;

                    for (int game = 1; game <= numberOfGames; game++)
                    {
                        Console.WriteLine($"\nGame {game}:");
                        string targetWord = game switch
                        {
                            //1 => "SMOKE",  // Game 1 target word is "SMOKE"
                            1 => "GLASS",  // Game 2 target word is "GLASS"
                            _ => wordList[new Random().Next(wordList.Length)]  // For other games, choose randomly
                        };

                        bool isWin = false;
                        int currentTurn = 1;

                        // User has max 6 turns
                        while (currentTurn <= 6 && !isWin)
                        {
                            Console.WriteLine($"Attempt {currentTurn}:");

                            string guess = string.Empty;

                            // Until the user enters a valid guess of exactly 5 letters
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

                            // Check exact matches (^)
                            for (int i = 0; i < 5; i++)
                                feedback[i] = (guess[i] == targetWord[i]) ? '^' : '-';

                            // Check for letters right in the wrong position (*)
                            for (int i = 0; i < 5; i++)
                            {
                                if (feedback[i] == '^') targetUsed[i] = true;

                                else
                                {
                                    for (int j = 0; j < 5 && feedback[i] != '^'; j++)
                                    {
                                        if (!targetUsed[j] && guess[i] == targetWord[j])
                                        {
                                            feedback[i] = '*'; // Mark '*' for correct letter in the wrong position
                                            targetUsed[j] = true;
                                        }
                                    }
                                }
                            }
                            DisplayFeedbackWithFrame(guess, new string(feedback));

                            if (new string(feedback) == "^^^^^")
                            {
                                Console.WriteLine("You win!");
                                isWin = true;
                            }

                            currentTurn++;
                        }

                        // If the user didn't win after 6 tries, print the losing message
                        if (!isWin)
                        {
                            Console.WriteLine($"You lose. The correct word was: {targetWord}");
                            gamesUnsolved++; // Increment unsolved games count
                        }
                        else
                        {
                            gamesSolved++; // Increment solved games count
                        }
                    }

                    // Print final summary
                    Console.WriteLine("\nMy Wordle Summary");
                    Console.WriteLine("=================");
                    Console.WriteLine($"You played {numberOfGames} games:");
                    Console.WriteLine($"|--> Number of wordles solved: {gamesSolved}");
                    Console.WriteLine($"|--> Number of wordles unsolved: {gamesUnsolved}");
                    Console.WriteLine("Thanks for playing!");

                    // Debugging check: Ensure totals match
                    if (gamesSolved + gamesUnsolved != numberOfGames)
                    {
                        Console.WriteLine("Error: Totals do not match number of games played.");
                    }
                }
            }
        }

        // Function to display feedback with frame
        // Function to display feedback with frame
        // Function to display feedback with frame
        static void DisplayFeedbackWithFrame(string guess, string feedback)
        {
            // Count '^' and '*' in feedback
            int correctSpotCount = feedback.Count(c => c == '^');
            int wrongSpotCount = feedback.Count(c => c == '*');

            // List to track correct letters and used letters
            var correctLetters = new System.Collections.Generic.List<char>();
            var usedLetters = new System.Collections.Generic.List<char>();

            // First pass: add correct letters in the correct positions
            for (int i = 0; i < 5; i++)
            {
                if (feedback[i] == '^')
                {
                    correctLetters.Add(guess[i]); // Correct letter in correct position
                }
            }

            // Second pass: add letters in wrong positions
            for (int i = 0; i < 5; i++)
            {
                if (feedback[i] == '*' && !correctLetters.Contains(guess[i]))
                {
                    correctLetters.Add(guess[i]); // Correct letter in wrong position
                }
            }

            // Add used letters
            for (int i = 0; i < 5; i++)
            {
                if (!usedLetters.Contains(guess[i]) && !correctLetters.Contains(guess[i]) && word.Contains(guess[i]))
                {
                    usedLetters.Add(guess[i]);
                }
            }

            // Create the string for correct letters and used letters without duplicates
            string correctLettersString = string.Join(" ", correctLetters.Distinct());
            string usedLettersString = string.Join(" ", usedLetters.Distinct());

            // Display the frame
            Console.WriteLine("-------------");
            Console.WriteLine($"| {string.Join(' ', guess.ToCharArray())} |");
            Console.WriteLine($"| {string.Join(' ', feedback.ToCharArray())} |");
            Console.WriteLine("|");
            Console.WriteLine($"| Correct spot(^): {correctSpotCount}");
            Console.WriteLine($"| Wrong spot(*): {wrongSpotCount}");
            Console.WriteLine("|");
            Console.WriteLine($"| Correct letters: {correctLettersString}");
            Console.WriteLine($"| Used letters: {usedLettersString}");
        }
    }
}
