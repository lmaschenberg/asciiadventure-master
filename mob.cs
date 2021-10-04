using asciiadventure;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace asciiadventure {
    public class Mob : MovingGameObject {
        public Mob(int row, int col, Screen screen) : base(row, col, "#", screen) {}
        public override Boolean IsPassable()
        {
            return true;
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
            Mob end = new Mob(newRow, newCol, this.Screen);
            Screen[originalRow, originalCol].Remove(this);
            return end;
        }
    }

    public enum Direction
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3
        }
    public class Bullet : MovingGameObject
    {
        private Direction direction;
        private static string Direct(Direction dir)
        {
            Direction direction;
            switch (dir)
            {
                case Direction.UP:
                    return "^";
                case Direction.DOWN:
                    return "v";
                case Direction.LEFT:
                    return "<";
                default: //RIGHT. It's an enum. What could possibly go wron--
                    return ">";
            }
        }
        public Bullet(int row, int col, Screen screen, Direction dir) : base(row, col, Direct(dir), screen)
        {
            direction = dir;
        }
        public override Boolean IsPassable()
        {
            return true;
        }
        public override MovingGameObject Move(int deltaRow, int deltaCol)
        {
            int newRow;
            int newCol;
            switch (direction)
            {
                case Direction.UP:
                    newRow = Row - 1;
                    newCol = Col;
                    break;
                case Direction.DOWN:
                    newRow = Row + 1;
                    newCol = Col;
                    break;
                case Direction.LEFT:
                    newRow = Row;
                    newCol = Col - 1;
                    break;
                default: //RIGHT. It's an enum. What could possibly go wron--
                    newRow = Row;
                    newCol = Col + 1;
                    break;
            }
            if (!Screen.IsInBounds(newRow,newCol))
                return this;
            if (Screen.IsOtherObject(newRow, newCol)){
                List<GameObject> others = Screen[newRow,newCol];
                for(int i = 0; i < others.Count; i++){
                    if(!(others[i] is Bullet)){
                        others[i].Delete();
                    }
                }
                return this;
            }
            // Now just make the move
            int originalRow = Row;
            int originalCol = Col;
            // now change the location of the object, if the move was legal
            Bullet end = new Bullet(newRow, newCol, this.Screen, this.direction);
            Screen[originalRow, originalCol].Remove(this);
            return end;
        }
    }
}


