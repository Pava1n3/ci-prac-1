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
            //FileStream ostrm;
            //StreamWriter writer;
            //TextWriter oldOut = //Console.Out;
            //try
            //{
            //    ostrm = new FileStream("./Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    writer = new StreamWriter(ostrm);
            //}
            //catch (Exception e)
            //{
            //    //Console.WriteLine("Cannot open Redirect.txt for writing");
            //    //Console.WriteLine(e.Message);
            //    return;
            //}
            ////Console.SetOut(writer);

            StreamReader stream = File.OpenText(args[0]);
            string line = stream.ReadLine();
            string[] lineChars = line.Split(' ');
            sudoLength = lineChars.Length;
            sudoSquareLength = (int)Math.Sqrt(sudoLength);

            int[,] field = new int[sudoLength, sudoLength];
            for (int x = 0; x < sudoLength; x++)
                field[x, 0] = int.Parse(lineChars[x]);
            int y = 1;

            //Console.WriteLine("Array is a " + field.GetType().IsClass);

            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                lineChars = line.Split(' ');
                for (int x = 0; x < sudoLength; x++)
                    field[x, y] = int.Parse(lineChars[x]);
                y++;
            }

            //Console.WriteLine();
            for (y = 0; y < sudoLength; y++)
            {
                for (int x = 0; x < sudoLength; x++)
                    Console.Write(field[x, y].ToString() + " ");
                Console.WriteLine();
            }

            Stack sudoStack = new Stack();
            sudoStack.Push(new Sudoku(field));
            Sudoku goal = BackTrack(sudoStack);
            //Console.WriteLine(sudoStack.Count + " items on the stack");
            Console.WriteLine();
            Console.WriteLine(goal is Sudoku ? "Goal found!" : "No goal found");
            Console.WriteLine(goal);
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
                        if (sudoku.ExpandedSuccessors[x, y] == null)
                        {
                            //int[] invalidNumbers = GetInvalidNumbers(sudoku.Field, x, y);
                            sudoku.ExpandedSuccessors[x, y] = new Number[sudoLength];
                            //for (int i = 1; i <= sudoLength; i++)
                            //{
                                sudoku.ExpandedSuccessors[x, y] = GetInvalidNumbers(sudoku.Field, x, y);//invalidNumbers[i] == 0 ? Number.Valid : Number.Invalid;
                            //}
                        }

                        for (int i = 1; i <= sudoLength; i++)
                            // Check if we have already expanded this successor
                            if (sudoku.ExpandedSuccessors[x, y][i] == Number.Valid)
                            {
                                sudoku.ExpandedSuccessors[x, y][i] = Number.Checked;
                                Sudoku successor = new Sudoku((int[,])sudoku.Field.Clone());
                                successor.Field[x, y] = i;
                                return successor;
                            }

                        #region poep
                        //// Get the numbers that can validly be put in the tile
                        //int[] validNumbers = GetValidNumbers(sudoku.Field, x, y);
                        //if (validNumbers.Length == 0)
                        //    return null;

                        ////Console.WriteLine("Valid numbers ({0},{1}): {2}", x, y, validNumbers.Length);

                        //for (int i = 0; i < validNumbers.Length; i++)
                        //    // Check if we have already expanded this successor
                        //    if (!sudoku.ExpandedSuccessors[x, y][validNumbers[i] - 1])
                        //    {
                        //        sudoku.ExpandedSuccessors[x, y][validNumbers[i] - 1] = true;
                        //        Sudoku successor = new Sudoku((int[,])sudoku.Field.Clone());
                        //        successor.Field[x, y] = validNumbers[i];
                        //        return successor;
                        //    }
                        #endregion
                    }

            return null;
        }

        static Number[] GetInvalidNumbers(int[,] sudoku, int x, int y)
        {
            // Array invalid is an array with how often
            // a number is seen in a row, column, or square.
            // The index of an item in invalid is the number in the field.
            Number[] invalid = new Number[sudoLength + 1];

            // Check for invalid numbers in the row
            for (int x1 = 0; x1 < sudoLength; x1++)
                invalid[sudoku[x1, y]] = Number.Invalid;

            // Check for invalid numbers in the column
            for (int y1 = 0; y1 < sudoLength; y1++)
                invalid[sudoku[x, y1]] = Number.Invalid;

            // Check for invalid numbers in the square
            // x2, y2 = index of top-left of square
            int root = sudoSquareLength;
            int x2 = x - x % root;
            int y2 = y - y % root;
            
            for (int y3 = y2; y3 < y2 + root; y3++)
                for (int x3 = x2; x3 < x2 + root; x3++)
                    invalid[sudoku[x3, y3]] = Number.Invalid;

            // For every item on a certain index (number) in the invalid array,
            // check if that invalid[number] is not seen (is 0), and then add
            // number to the valid list.
            //List<int> valid = new List<int>();
            //for (int i = 1; i < invalid.Length; i++)
            //    if (invalid[i] == 0)
            //        valid.Add(i);

            //return valid.ToArray();
            return invalid;
        }

        /*
        procedure backtrack(L) returns t:
            if empty(L) 
                then return nil
            else
                t = first(L);
                if goal(t) 
                    then return t
                else
                    while there are unexplored successors of t and not found do
                        t’ = next-successor(t);
                        L = push(L,t’);
                        backtrack(L)
                    endwhile;
                L = pop(L)
        endprocedure
        */
        //static Stack stack = new Stack();

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
                    while (true) //has-successors(t)
                    {
                        Sudoku s = NextSuccessor(t);
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
                    ////Console.WriteLine(t);
                    //Console.WriteLine("No goal under current sudoku");
                    sudoStack.Pop();
                    return null;
                }
            }
        }
    }
}
