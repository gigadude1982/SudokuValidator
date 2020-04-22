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

            var isValid = validateGrid(loadGrid(filePath));

            Console.WriteLine(isValid ? "Yes, valid!" : "No, invalid!");

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

        private static bool validateGrid(string[,] sudokuGrid)
        {
            try
            {
                // todo: consolidate as much duplicated iterative code here as possible
                // and move into 3 separate sub-routines

                // validate all columns contain 1-9 with no dupes
                for (int i = 0; i < gridWidth; i++)
                {
                    var foundNums = new List<string>();
                    for (int j = 0; j < gridHeight; j++)
                    {
                        if (foundNums.Contains(sudokuGrid[i, j]))
                        {
                            // dupe found
                            Console.WriteLine("failed column validation!");
                            return false;
                        }

                        // only handle non empty cells ('.') in grid
                        if (sudokuGrid[i, j] != ".")
                        {
                            foundNums.Add(sudokuGrid[i, j]);
                        }
                    }
                }

                Console.WriteLine("passed column validation!");

                // validate all rows contain 1-9 with no dupes
                for (int i = 0; i < gridHeight; i++)
                {
                    var foundNums = new List<string>();
                    for (int j = 0; j < gridWidth; j++)
                    {
                        if (foundNums.Contains(sudokuGrid[j, i]))
                        {
                            // dupe found
                            Console.WriteLine("failed row validation!");
                            return false;
                        }

                        // only handle non empty cells ('.') in grid
                        if (sudokuGrid[j, i] != ".")
                        {
                            foundNums.Add(sudokuGrid[j, i]);
                        }
                    }

                }

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
                                if (foundNums.Contains(sudokuGrid[i, j]))
                                {
                                    // dupe found
                                    Console.WriteLine("failed grid validation!");
                                    return false;
                                }

                                // only handle non empty cells ('.') in grid
                                if (sudokuGrid[i, j] != ".")
                                {
                                    foundNums.Add(sudokuGrid[i, j]);
                                }
                            }
                        }
                    }
                }
                
                Console.WriteLine("passed grid validation!");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to validate grid!");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
