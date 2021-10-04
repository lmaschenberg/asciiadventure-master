
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace asciiadventure {
    public class Screen {
        private List<GameObject>[,] grid;                   //Fix: 2 objects can now occupy the same space.
        private Random random = new Random();

        public int NumRows {
            get;
            private set;
        }
        public int NumCols {
            get;
            private set;
        }
        public int Index
        {
            get;
            private set;
        }

        public Screen(int numRows, int numCols, int index)
        {
            NumRows = numRows;
            NumCols = numCols;
            Index = index;
            this.grid = new List<GameObject>[NumRows, NumCols];
            for(int i = 0; i < numCols; i++){
                for(int j = 0; j < numRows; j++){
                    this.grid[i,j] = new List<GameObject>();
                }
            }
        }

        public Boolean IsLegalMove(int row, int col, int deltaRow, int deltaCol) {
            int newRow = row + deltaRow;
            int newCol = col + deltaCol;
            if (!IsInBounds(newRow, newCol)){
                return false;
            }
            List<GameObject> others = this[newRow, newCol];
            foreach(GameObject other in others){
                if (other != null){
                    return other.IsPassable() || other is Player;
                }
            }
            return true;
            
        }

        public List<Twin<int, int>> GetLegalMoves(int row, int col) {
            List<Twin<int, int>> moves = new List<Twin<int, int>>(); //See Twin class for explanation
            if (IsLegalMove(row, col, -1, 0)) {
                moves.Add(new Twin<int, int>(-1, 0));
            }
            if (IsLegalMove(row, col, 1, 0)){
                moves.Add(new Twin<int, int>(1, 0));
            }
            if (IsLegalMove(row, col, 0, -1)){
                moves.Add(new Twin<int, int>(0, -1));
            }
            if (IsLegalMove(row, col, 0, 1)) {
                moves.Add(new Twin<int, int>(0, 1));
            }
            return moves;
        }

        public List<GameObject> this[int row, int col]
        {
            get { 
                UseRowAndCol(row, col);
                return grid[row, col];
            }
        }

        protected Boolean CheckRow(int row){
            return row >= 0 && row < NumRows;
        }

        protected Boolean CheckCol(int col){
            return col >= 0 && col < NumCols;
        }

        internal Boolean IsInBounds(int row, int col){
            // TODO: Check for obstacles
            return CheckRow(row) && CheckCol(col);
        }

        protected void UseRowAndCol(int row, int col){
            if (!CheckRow(row)){
                throw new ArgumentOutOfRangeException("row", $"{row} is out of range");
            }
            if (!CheckCol(col)){
                throw new ArgumentOutOfRangeException("col", $"{col} is out of range");
            }
        }

        public Boolean IsOtherObject(int row, int col){
            return grid[row, col].Count != 0;
        }

        public override String ToString() {
            // create walls if needed
            StringBuilder result = new StringBuilder();
            result.Append("+");
            result.Append(String.Concat(Enumerable.Repeat("-", NumCols)));
            result.Append("+\n");
            for (int r=0; r < NumRows; r++){
                result.Append('|');
                for (int c=0; c < NumCols; c++){
                    List<GameObject> gameObjects = this[r, c];
                    if (gameObjects.Count == 0){
                        result.Append(' ');
                    } else {
                        result.Append(gameObjects[0].Token);
                    }
                }
                //Console.WriteLine($"newline for {r}");
                result.Append("|\n");
            }
            result.Append('+');
            result.Append(String.Concat(Enumerable.Repeat("-", NumRows)));
            result.Append('+');
            return result.ToString();
        }
    }
}