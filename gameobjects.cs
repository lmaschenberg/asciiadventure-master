using asciiadventure;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace asciiadventure {
    public abstract class GameObject {
        
        public int Row {
            get;
            protected set;
        }
        public int Col {
            get;
            protected set;
        }

        public String Token {
            get;
            protected internal set;
        }

        public Screen Screen {
            get;
            protected set;
        }

        public GameObject(int row, int col, String token, Screen screen){
            Row = row;
            Col = col;
            Token = token;
            Screen = screen;
            Screen[row, col].Add(this);
        }

        public virtual Boolean IsPassable() {
            return false;
        }

        public override String ToString() {
            return this.Token;
        }

        public virtual void Delete()
        {
           bool removed = Screen[Row, Col].Remove(this);
           //Does not function.
        }
    }

    public abstract class MovingGameObject : GameObject {
        public bool Removed {get; private set;}
        public MovingGameObject(int row, int col, String token, Screen screen) : base(row, col, token, screen) {
            Removed = false;
        }
        public override void Delete()
        {
            Removed = true;
            base.Delete();
        }
        public abstract MovingGameObject Move(int deltaRow, int deltaCol);
        /*
        {
            int newRow = deltaRow + Row;
            int newCol = deltaCol + Col;
            if (!Screen.IsInBounds(newRow, newCol))
            {
                return "";
            }
            List<GameObject> gameObjects = Screen[newRow, newCol];
            foreach(GameObject gameObject in gameObjects){
                if (gameObject != null && !gameObject.IsPassable())
                {
                    // TODO: How to handle other objects?
                    // walls just stop you
                    // objects can be picked up
                    // people can be interactd with
                    // also, when you move, some things may also move
                    // maybe i,j,k,l can attack in different directions?
                    // can have a "shout" command, so some objects require shouting
                    return "TODO: Handle interaction";
                }
            }
            // Now just make the move
            int originalRow = Row;
            int originalCol = Col;
            // now change the location of the object, if the move was legal
            Screen[newRow, newCol].Add(this);
            Screen[originalRow, originalCol].Remove(this);
            return "";
        */
    }

    class Wall : GameObject {
        public Wall(int row, int col, Screen screen) : base(row, col, "=", screen) {}

        /*public override void Delete() //DOES NOT WORK. 
        {
            if(Token == "=")
            {
                Token = "-";//Does this work?
            }
            else if(Token == "-")
            {
                Token = " ";
                Screen[Row, Col].Remove(this);
            }
            else
            {
                throw new Exception("Wall Delete Method Invalid");
            }
        }*/ 
    }

    class Treasure : GameObject {
        public Treasure(int row, int col, Screen screen) : base(row, col, "T", screen) {}

        public override Boolean IsPassable() {
            return true;
        }
}
}