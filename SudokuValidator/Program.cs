using System;
using System.Collections.Generic;
using System.IO;

namespace SudokuValidator
{
    public static class Program
    {
        // todo: create sudokugrid class with overloaded toString() method to print grid
        // and encapsulate all functionality into internal validateFile method

        // todo: wire up unit tests to increase code coverage

        // sudoku uses (1) 9x9 main grid with (9) 3x3 sub grids with values 1-9 or '.' for "empty/no value"

        const int gridWidth = 9;
        const int gridHeight = 9;
        const int subGridWidth = 3;
        const int subGridHeight = 3;

        public static void Main(string[] args)
        {
            Console.WriteLine("Sudoku File Validator v1.0\n");

            if (args.Length == 0)
            {
                Console.WriteLine("Please enter sudoku input file name!");
                return;
            }

            var filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found!");
                return;
            }

            Console.WriteLine("Validating " + filePath + "...");

            validateGrid(loadGrid(filePath));

            Console.WriteLine("Press any key to quit...");
            Console.ReadLine();
        }

        private static string[,] loadGrid(string filePath)
        {
            var sudokuGrid = new string[gridHeight, gridWidth];

            try
            {
                var fileInput = File.ReadAllText(filePath);
                var i = 0;

                // load grid
                foreach (var row in fileInput.Split('\n'))
                {
                    if (String.IsNullOrEmpty(row))
                    {
                        continue;
                    }
                    var j = 0;
                    foreach (var col in row.Trim().Split(' '))
                    {
                        sudokuGrid[i, j] = col.Trim();
                        j++;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load grid!");
                Console.WriteLine(ex.ToString());
            }

            printGrid(sudokuGrid);

            return sudokuGrid;
        }

        private static void printGrid(string[,] sudokuGrid)
        {
            // Console.WriteLine("Sudoku Grid:");
            for (int i = 0; i < sudokuGrid.GetLength(0); i++)
            {
                for (int j = 0; j < sudokuGrid.GetLength(1); j++)
                {
                    Console.Write(sudokuGrid[i, j]);
                    Console.Write(' ');
                }
                Console.WriteLine("");
            }
        }

        private static void validateGrid(string[,] sudokuGrid)
        {
            var isValid = true; // default to truthy

            try
            {
                // todo: consolidate as much duplicated iterative code here as possible
                // and refactor to be more short circuited so its easier
                // to read, maintain, and isn't nested.

                // validate all columns contain 1-9 with no dupes - move into sub routine
                for (int i = 0; i < gridWidth; i++)
                {
                    var foundNums = new List<string>();
                    for (int j = 0; j < gridHeight; j++)
                    {
                        // Console.WriteLine("col found nums count = " + foundNums.Count);
                        // do we need to check for numbers < 1 and > 9 or is it assumed they're always 1-9 or "empty"
                        // if (sudokuGrid[i, j] < 1 || sudokuGrid[i, j] > 9 || foundNums.Contains(sudokuGrid[i, j]))
                        if (foundNums.Contains(sudokuGrid[i, j]))
                        {
                            // invalid number found
                            isValid = false;
                            //  Console.WriteLine("breaking col check loop 1");
                            break;
                        }

                        // only handle non empty cells ('.') in grid
                        if (sudokuGrid[i, j] != ".")
                        {
                            foundNums.Add(sudokuGrid[i, j]);
                        }
                    }
                    if (!isValid)
                    {
                        // Console.WriteLine("breaking col check loop 2");
                        break;
                    }
                }

                if (!isValid)
                {
                    Console.WriteLine("failed column validation!");
                }
                else
                {
                    // only continue if still valid
                    Console.WriteLine("passed column validation!");

                    // validate all rows contain 1-9 with no dupes - move into sub routine
                    for (int i = 0; i < gridHeight; i++)
                    {
                        var foundNums = new List<string>();
                        for (int j = 0; j < gridWidth; j++)
                        {
                            // Console.WriteLine("row found nums count = " + foundNums.Count);
                            // do we need to check for numbers < 1 and > 9 or is it assumed they're always 1-9 or "empty"
                            // if (sudokuGrid[j, i] < 1 || sudokuGrid[j, i] > 9 || foundNums.Contains(sudokuGrid[j, i]))
                            if (foundNums.Contains(sudokuGrid[j, i]))
                            {
                                // invalid number found
                                isValid = false;
                                // Console.WriteLine("breaking row check loop 1");
                                break;
                            }

                            // only handle non empty cells ('.') in grid
                            if (sudokuGrid[j, i] != ".")
                            {
                                foundNums.Add(sudokuGrid[j, i]);
                            }
                        }
                        if (!isValid)
                        {
                            // Console.WriteLine("breaking row check loop 2");
                            break;
                        }
                    }

                    if (!isValid)
                    {
                        Console.WriteLine("failed row validation!");
                    }
                    else
                    {
                        // only continue if still valid
                        Console.WriteLine("passed row validation!");

                        // validate all 3x3 sub grids contain 1-9 with no dupes
                        for (int x = 0; x < gridWidth; x += subGridWidth)
                        {
                            for (int y = 0; y < gridHeight; y += subGridHeight)
                            {
                                var foundNums = new List<string>();
                                for (int i = x; i < x + subGridHeight; i++)
                                {
                                    for (int j = y; j < y + subGridWidth; j++)
                                    {
                                        // Console.WriteLine("grid found nums count = " + foundNums.Count);
                                        // do we need to check for numbers < 1 and > 9 or is it assumed they're always 1-9 or "empty"
                                        // if (sudokuGrid[i, j] < 1 || sudokuGrid[i, j] > 9 || foundNums.Contains(sudokuGrid[i, j]))
                                        if (foundNums.Contains(sudokuGrid[i, j]))
                                        {
                                            // invalid number found
                                            isValid = false;
                                            // Console.WriteLine("breaking grid check loop 1");
                                            break;
                                        }

                                        // only handle non empty cells ('.') in grid
                                        if (sudokuGrid[i, j] != ".")
                                        {
                                            foundNums.Add(sudokuGrid[i, j]);
                                        }
                                    }
                                    if (!isValid)
                                    {
                                        // Console.WriteLine("breaking grid check loop 2");
                                        break;
                                    }

                                }
                            }
                        }

                        if (!isValid)
                        {
                            Console.WriteLine("failed grid validation!");
                        }
                        else
                        {
                            Console.WriteLine("passed grid validation!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to validate grid!");
                Console.WriteLine(ex.ToString());
            }

            if (isValid)
            {
                Console.WriteLine("Yes, valid!");
            }
            else
            {
                Console.WriteLine("No, invalid!");
            }
        }
    }
}
