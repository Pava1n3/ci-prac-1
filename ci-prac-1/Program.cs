﻿using System;
using System.IO;
using System.Collections.Generic;

namespace ci_prac_1
{
    class Program
    {
        static int[,] startdoku = new int[2, 2];
        static Stack sudoStack = new Stack();

        static void Main(string[] args)
        {
            StreamReader stream = File.OpenText(args[0]);
            string line = stream.ReadLine();
            string[] lineChars = line.Split(' ');
            int length = lineChars.Length;

            int[,] sudoku = new int[length, length];
            for (int x = 0; x < length; x++)
                sudoku[x, 0] = int.Parse(lineChars[x]);
            int y = 1;

            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                lineChars = line.Split(' ');
                for (int x = 0; x < length; x++)
                    sudoku[x, y] = int.Parse(lineChars[x]);
                y++;
            }

            Console.WriteLine();
            for (y = 0; y < length; y++)
            {
                for (int i = 0; i < length; i++)
                    Console.Write(sudoku[x, y].ToString() + " ");
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        static Sudoku NextSuccessor(Sudoku sudoku)
        {
            // Loop through all of the tiles in the field
            for (int y = 0; y < sudoku.Field.GetLength(0); y++)
                for (int x = 0; x < sudoku.Field.GetLength(0); x++)
                    // If the tile is empty
                    if (sudoku.Field[x, y] == 0)
                    {
                        // Get the numbers that can validly be put in the tile
                        int[] validNumbers = GetValidNumbers(sudoku.Field, x, y);

                        for (int i = 0; i < validNumbers.Length; i++)
                            // Check if we have already expanded this successor
                            if (!sudoku.ExpandedSuccessors[x, y][validNumbers[i] - 1])
                            {
                                Sudoku successor = new Sudoku(sudoku.Field);
                                successor.Field[x, y] = validNumbers[i];
                                sudoku.ExpandedSuccessors[x, y][validNumbers[i] - 1] = true;
                                return successor;
                            }
                    }

            return null;
        }

        static int[] GetValidNumbers(int[,] sudoku, int x, int y)
        {
            int[] invalid = new int[sudoku.GetLength(0) + 1];
            for (int x1 = 0; x1 < sudoku.GetLength(0); x1++)
                invalid[sudoku[x1, y]]++;

            for (int y1 = 0; y1 < sudoku.GetLength(0); y1++)
                invalid[sudoku[x, y1]]++;

            // x2, y2 = index of square
            int root = (int)Math.Sqrt(sudoku.GetLength(0));
            int x2 = x - x % root;
            int y2 = y / root;

            for (int y3 = y2; y3 < y2 + root; y3++)
                for (int x3 = x2; x3 < x2 + root; x3++)
                    invalid[sudoku[x3, y3]]++;

            List<int> valid = new List<int>();
            for (int i = 0; i < invalid.Length; i++)
                if (invalid[i] == 0)
                    valid.Add(i);

            return valid.ToArray();
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

        //static int[,] BackTrack(Stack soduStack)
        //{
        //    int[,] t = (int[,])sudoStack.Peek();
        //    if (false)  //if(goal(t))
        //        return t;
        //    else

        //        while(true) //has-successors(t)
        //        {
        //            //int[,] s = next-successor(t);
        //            int[,] s = new int[2, 2];
        //            sudoStack.Push(s);
        //            BackTrack(sudoStack);
        //        }
        //    sudoStack.Pop();
        //}

    }
}
