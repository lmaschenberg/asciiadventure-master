using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace asciiadventure {
    class Player : MovingGameObject
    {
        public Player(int row, int col, Screen screen, string name) : base(row, col, "@", screen){
            Name = name;
        }

        public string Name
        {
            get;
            protected set;
        }
        public override Boolean IsPassable(){
            return true;
        }

        public String Action(int deltaRow, int deltaCol)
        {
            int newRow = Row + deltaRow;
            int newCol = Col + deltaCol;
            if (!Screen.IsInBounds(newRow, newCol))
                return "nope";

            List<GameObject> others = Screen[newRow, newCol];
            foreach (GameObject other in others)
                if (other is Treasure)
                    return "Yay, we got the treasure!"; 
            return "Fire!";
        }
        
        public override MovingGameObject Move(int deltaRow, int deltaCol){
            int newRow = deltaRow + Row;
            int newCol = deltaCol + Col;
            if (!Screen.IsLegalMove(Row, Col, deltaRow, deltaCol))
            {
                return this;
            }
            // Now just make the move
            int originalRow = Row;
            int originalCol = Col;
            // now change the location of the object, if the move was legal
            Player end = new Player(newRow, newCol, this.Screen,this.Name);
            Screen[originalRow, originalCol].Remove(this);
            return end;
        }
    }
}