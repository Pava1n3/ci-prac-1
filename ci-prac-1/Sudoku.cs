﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ci_prac_1
{
    public enum Status { Valid, Invalid, Checked };
    public class Sudoku
    {
        // Whole sudoku = Field
        // Group of nxn tiles = Square
        // One individual tile = Tile

        public Sudoku(int[,] sudoku)
        {
            this.Field = sudoku;
            this.ExpandedSuccessors = new Status[Program.sudoLength, Program.sudoLength][];
        }
        
        public int[,] Field;
        public Status[,][] ExpandedSuccessors;

        public bool IsGoal
        {
            get
            {
                for (int y = 0; y < Program.sudoLength; y++)
                    for (int x = 0; x < Program.sudoLength; x++)
                        if (this.Field[x, y] == 0)
                            return false;
                return true;
            }
        }

        public override string ToString()
        {
            string sudoString = "";
            for (int y = 0; y < Program.sudoLength; y++)
            {
                for (int x = 0; x < Program.sudoLength; x++)
                {
                    int value = this.Field[x, y];
                    sudoString += value + " ";
                }
                sudoString += Environment.NewLine;
            }
            return sudoString;
        }
    }
}
