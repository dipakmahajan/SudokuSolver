using System;
using System.Text.RegularExpressions;

namespace Sudoku_Solver
{
    class Program
    {
        private static int boardLength = 9;
        private static int[,] sudokuBoard = new int[9, 9];
        private static string[] defaultPuzzle = new string[]
        {
            "  71    8",
            " 9  784",
            " 1  95",
            "9",
            " 56   37",
            "        2",
            "   35  9",
            "  596  2",
            "1    76",
        };

        static void Main(string[] args)
        {
            Console.WriteLine("To enter sudoku puzzle, use single character as one block of the puzzle. Only numbers 1 thru 9 and blank spaces are allowed.");
            Console.WriteLine("Example: {0}{1}", Environment.NewLine, string.Join(Environment.NewLine, defaultPuzzle));
            bool continueSolving = true;
            while (continueSolving)
            {
                if (InputSudokuPuzzle())
                {
                    Console.WriteLine("Sudoku Puzzle: ");
                    PrintSudokuBoard();

                    if (!SolveSudoku(0, 0))
                        Console.WriteLine("The problem cannot be solved");
                    else
                    {
                        Console.WriteLine("{0}Solution: ", Environment.NewLine);
                        PrintSudokuBoard();
                    }
                }

                Console.WriteLine();
                Console.Write("Do you want to solve more sudoku puzzles (press Y/y for yes, any other key to exit)?");
                var continueInput = Console.ReadKey();
                continueSolving = continueInput.KeyChar == 'y' || continueInput.KeyChar == 'Y';
            }
        }

        private static bool InputSudokuPuzzle()
        {
            Console.WriteLine();
            Console.WriteLine("Please enter the sudoku puzzle:");
            for (int i = 0; i < 9; i++)
            {
                var row = Console.ReadLine();
                if (row.Length < 10 && Regex.IsMatch(row, "[1-9 ]+"))
                {
                    for (int j = 0; j < row.Length; j++)
                    {
                        if (row[j] == ' ') continue;
                        sudokuBoard[i, j] = row[j] - '0';
                    }
                }
                else
                {
                    Console.WriteLine("Invalid sudoku puzzle.");
                    return false;
                }
            }

            Console.WriteLine();
            return true;
        }

        private static bool SolveSudoku(int row, int col)
        {
            if (row == boardLength - 1 && col == boardLength) return true;
            if (col == boardLength)
            {
                row++;
                col = 0;
            }

            if (sudokuBoard[row, col] != 0)
                return SolveSudoku(row, col + 1);

            for (int i = 1; i < 10; i++)
            {
                if (IsSafe(row, col, i))
                {
                    sudokuBoard[row, col] = i;
                    if (SolveSudoku(row, col + 1))
                        return true;
                }

                sudokuBoard[row, col] = 0;
            }

            return false;
        }

        private static bool IsSafe(int row, int col, int val)
        {
            for (int i = 0; i < boardLength; i++)
                if (sudokuBoard[row, i] == val || sudokuBoard[i, col] == val)
                    return false;

            int startRow = row - row % 3;
            int startCol = col - col % 3;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (sudokuBoard[startRow + i, startCol + j] == val)
                        return false;

            return true;
        }

        private static void PrintSudokuBoard()
        {
            for (int i = 0; i < boardLength; i++)
            {
                if (i == 0 || i % 3 == 0)
                    Console.WriteLine("_______________________________________");

                for (int j = 0; j < boardLength; j++)
                {
                    if (j == 0 || j % 3 == 0)
                        Console.Write(" | ");

                    Console.Write(" {0} ", sudokuBoard[i, j] == 0 ? "X" : sudokuBoard[i, j]);
                }
                Console.WriteLine(" | ");
            }

            Console.WriteLine("_______________________________________");
        }
    }
}
