﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace ci_prac_1
{
    class Program
    {
        public static int sudoLength, sudoSquareLength;
        static void Main(string[] args)
        {
            // Let the Console output to a file
            //FileStream ostrm;
            //StreamWriter writer;
            //TextWriter oldOut = Console.Out;
            //try
            //{
            //    File.Delete("./Redirect.txt");
            //    ostrm = new FileStream("./Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    writer = new StreamWriter(ostrm);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Cannot open Redirect.txt for writing");
            //    Console.WriteLine(e.Message);
            //    return;
            //}
            //Console.SetOut(writer);

            // Read a text file with a sudoku puzzle
            StreamReader stream = File.OpenText(args[0]);
            string line = stream.ReadLine();
            string[] lineChars = line.Split(' ');
            sudoLength = lineChars.Length;
            sudoSquareLength = (int)Math.Sqrt(sudoLength);

            // Put the sudoku text in an array
            int[,] field = new int[sudoLength, sudoLength];
            for (int x = 0; x < sudoLength; x++)
                field[x, 0] = int.Parse(lineChars[x]);
            int y = 1;

            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                lineChars = line.Split(' ');
                for (int x = 0; x < sudoLength; x++)
                    field[x, y] = int.Parse(lineChars[x]);
                y++;
            }
            
            // Print the sudoku
            for (y = 0; y < sudoLength; y++)
            {
                for (int x = 0; x < sudoLength; x++)
                    Console.Write(field[x, y].ToString() + " ");
                Console.WriteLine();
            }

            // Execute backtrack algorithm and find a goal
            TimeSpan startTime = DateTime.Now.TimeOfDay;
            Stack sudoStack = new Stack();
            sudoStack.Push(new Sudoku(field));
            Sudoku goal = BackTrack(sudoStack);
            TimeSpan endTime = DateTime.Now.TimeOfDay;

            Console.WriteLine(sudoStack.Count + " items on the stack");
            Console.WriteLine();
            Console.WriteLine(goal is Sudoku ? "Goal found!" : "No goal found");
            Console.WriteLine(goal);
            Console.WriteLine(endTime - startTime);
            //ostrm.Close();
            Console.ReadKey();
        }

        static Sudoku NextSuccessor(Sudoku sudoku)
        {
            // Loop through all of the tiles in the field
            for (int y = 0; y < sudoLength; y++)
                for (int x = 0; x < sudoLength; x++)
                    // If the tile is empty
                    if (sudoku.Field[x, y] == 0)
                    {
                        // If we don't know which numbers are valid/invalid, find those
                        if (sudoku.ExpandedSuccessors[x, y] == null)
                        {
                            sudoku.ExpandedSuccessors[x, y] = GetInvalidNumbers(sudoku.Field, x, y);
                        }

                        // For every number that we can fill in:
                        int amountOfInvalid = 0;
                        for (int i = 1; i <= sudoLength; i++)
                        {
                            // Check if the number is valid (and not checked yet)
                            if (sudoku.ExpandedSuccessors[x, y][i] == Status.Valid)
                            {
                                // If so, create a successor and fill in that number in the sudoku field
                                sudoku.ExpandedSuccessors[x, y][i] = Status.Checked;
                                Sudoku successor = new Sudoku((int[,])sudoku.Field.Clone());
                                successor.Field[x, y] = i;

                                return successor;
                            }
                            else
                            {
                                // If all numbers in this tile are either invalid or already checked,
                                // then the current sudoku has no successor (because this tile can not be filled in)
                                amountOfInvalid++;
                                if (amountOfInvalid == sudoLength)
                                    return null;
                            }
                        }
                    }

            // This can only happen when all tiles are filled in,
            // but that means the current sudoku is a goal
            return null;
        }

        static Sudoku NextSuccessor2(Sudoku sudoku)
        {
            int lowestx = 0, lowesty = 0, lowestAmountOfValidNumbers = sudoLength * 2;

            // For every tile in the sudoku:
            for (int y = 0; y < sudoLength; y++)
                for (int x = 0; x < sudoLength; x++)
                    if (sudoku.Field[x, y] == 0)
                    {
                        // If we don't know which numbers are valid/invalid in this tile, find those
                        if (sudoku.ExpandedSuccessors[x, y] == null)
                            sudoku.ExpandedSuccessors[x, y] = GetInvalidNumbers(sudoku.Field, x, y);

                        // Check how many valid numbers there are that we can fill in
                        int amountOfValidNumbers = 0;
                        for (int i = 1; i <= sudoLength; i++)
                            if (sudoku.ExpandedSuccessors[x, y][i] == Status.Valid)
                                amountOfValidNumbers++;

                        // If all numbers in this tile are either invalid or already checked,
                        // then the current sudoku has no successor (because this tile can not be filled in)
                        if (amountOfValidNumbers == 0)
                            return null;

                        // If we can fill in less numbers than the last tile with the lowest amount of valid numbers,
                        // then this tile has the lowest amount of valid numbers
                        if (amountOfValidNumbers < lowestAmountOfValidNumbers)
                        {
                            lowestx = x;
                            lowesty = y;
                            lowestAmountOfValidNumbers = amountOfValidNumbers;
                        }
                    }
            
            // Get the first number that's valid in the tile with the lowest amount of valid numbers,
            // and then create a successor with a field with that number filled in
            for (int i = 1; i < sudoLength + 1; i++)
                if (sudoku.ExpandedSuccessors[lowestx, lowesty][i] == Status.Valid)
                {
                    sudoku.ExpandedSuccessors[lowestx, lowesty][i] = Status.Checked;
                    Sudoku successor = new Sudoku((int[,])sudoku.Field.Clone());
                    successor.Field[lowestx, lowesty] = i;
                    return successor;
                }

            // This can only happen when all tiles are filled in,
            // but that means the current sudoku is a goal
            return null;
        }

        static Status[] GetInvalidNumbers(int[,] sudoku, int x, int y)
        {
            // Array invalid is an array with how often
            // a number is seen in a row, column, or square.
            // The index of an item in invalid is the number in the field.
            Status[] invalid = new Status[sudoLength + 1];

            // Check for invalid numbers in the row
            for (int x1 = 0; x1 < sudoLength; x1++)
                invalid[sudoku[x1, y]] = Status.Invalid;

            // Check for invalid numbers in the column
            for (int y1 = 0; y1 < sudoLength; y1++)
                invalid[sudoku[x, y1]] = Status.Invalid;

            // Check for invalid numbers in the square
            // x2, y2 = index of top-left of square
            int root = sudoSquareLength;

            int x2 = x - x % root;
            int y2 = y - y % root;
            
            for (int y3 = y2; y3 < y2 + root; y3++)
                for (int x3 = x2; x3 < x2 + root; x3++)
                    invalid[sudoku[x3, y3]] = Status.Invalid;
            
            return invalid;
        }

        static Sudoku BackTrack(Stack sudoStack)
        {
            //Console.WriteLine("Backtracking called");
            if (sudoStack.Count == 0)
            {
                //Console.WriteLine("Stack empty");
                return null;
            }
            else
            {
                //Console.WriteLine("Stack not empty");
                Sudoku t = (Sudoku)sudoStack.Peek();
                //Console.WriteLine(t);
                if (t.IsGoal)
                {
                    //Console.WriteLine("Sudoku is goal");
                    return t;
                }
                else
                {
                    //Console.WriteLine("Sudoku is not goal");
                    while (true)
                    {
                        Sudoku s = NextSuccessor2(t);
                        if (s == null)
                        {
                            //Console.WriteLine("No successor found");
                            break;
                        }
                        //Console.WriteLine("Successor found");
                        sudoStack.Push(s);
                        Sudoku goal = BackTrack(sudoStack);
                        if (goal != null)
                            return goal;
                    }
                    //Console.WriteLine(t);
                    //Console.WriteLine("No goal under current sudoku");
                    sudoStack.Pop();
                    return null;
                }
            }
        }
    }
}
