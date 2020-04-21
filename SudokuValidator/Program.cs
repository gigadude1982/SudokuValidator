using System;
using System.IO;

namespace SudokuValidator
{
    public static class Program
    {
        // todo: create sudokugrid class with overloaded toString() method to print grid and encapsulate all functionality into validateFile method
        // sudoku uses (1) 9x9 main grid with (9) 3x3 sub grids
        const int gridWidth = 9;
        const int gridHeight = 9;
        const int subGridWidth = 3;
        const int subGridHeight = 3;
        const int sudokuSum = 45;

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

        private static int[,] loadGrid(string filePath)
        {
            int[,] sudokuGrid = new int[gridHeight, gridWidth];

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
                        //if (String.IsNullOrEmpty(col))
                        //{
                        //    continue;
                        //}
                        sudokuGrid[i, j] = int.Parse(col.Trim());

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

        private static void printGrid(int[,] sudokuGrid)
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

        private static void validateGrid(int[,] sudokuGrid)
        {
            var isValid = true; // default to truthy

            try
            {
                // verify summation of all columns equals sudoku sum (45)
                for (int i = 0; i < gridWidth; i++)
                {
                    var colSum = 0;
                    for (int j = 0; j < gridHeight; j++)
                    {
                        colSum += sudokuGrid[i, j];
                    }
                    // Console.WriteLine("col " + i + " sum = " + colSum);
                    if (colSum != sudokuSum)
                    {
                        isValid = false;
                    }
                }

                // verify summation of all rows equals sudoku sum (45)
                for (int i = 0; i < gridHeight; i++)
                {
                    var rowSum = 0;
                    for (int j = 0; j < gridWidth; j++)
                    {
                        rowSum += sudokuGrid[j, i];
                    }
                    // Console.WriteLine("row " + i + " sum = " + rowSum);
                    if (rowSum != sudokuSum)
                    {
                        isValid = false;
                    }
                }

                // verify summation of all 3x3 sub grids equals sudoku sum (45)
                for (int x = 0; x < gridWidth; x += subGridWidth)
                {
                    for (int y = 0; y < gridHeight; y += subGridHeight)
                    {
                        var subGridSum = 0;
                        for (int i = x; i < x + subGridHeight; i++)
                        {
                            for (int j = y; j < y + subGridWidth; j++)
                            {
                                // Console.Write(sudokuGrid[i, j]);
                                // Console.Write(' ');
                                subGridSum += sudokuGrid[i, j];
                            }
                            // Console.WriteLine("");
                        }
                        // Console.WriteLine("sub grid [(" + x + "-" + (x + (subGridWidth - 1)) + "),(" + y + "-" + (y + (subGridHeight - 1)) + ")] sum = " + subGridSum);
                        if (subGridSum != sudokuSum)
                        {
                            isValid = false;
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
