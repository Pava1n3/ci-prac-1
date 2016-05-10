using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ci_prac_1
{
    public class Sudoku
    {
        // Whole sudoku = Field
        // Group of nxn tiles = Square
        // One individual tile = Tile

        public Sudoku(int[,] sudoku)
        {
            this.Field = sudoku;
            this.ExpandedSuccessors = new bool[sudoku.GetLength(0), sudoku.GetLength(0)][];
            for (int y = 0; y < sudoku.GetLength(0); y++)
                for (int x = 0; x < sudoku.GetLength(0); x++)
                    this.ExpandedSuccessors[x, y] = new bool[sudoku.GetLength(0)];
        }

        public int[,] Field;
        public bool[,][] ExpandedSuccessors;
    }
}
