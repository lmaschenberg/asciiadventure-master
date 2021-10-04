using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/*
 Cool ass stuff people could implement:
 > jumping
 > attacking
 > randomly moving monsters
 > smarter moving monsters
*/
namespace asciiadventure {
    public class Game {
        private Random random = new Random();
        private static Boolean Eq(char c1, char c2) {
            return c1.ToString().Equals(c2.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static string Menu() {
            return "WASD to move\nIJKL to attack/interact\n\nYou are here: -->(@)\nInteract with the treasure(T) to win the game\nAttack walls(=), or you shall not pass\nAttack or avoid mobs(#), or be eliminated\n";
        }

        private static void PrintScreen(Screen screen, string message, string menu) {
            Console.Clear();
            Console.WriteLine($"Player {screen.Index}'s board:");
            Console.WriteLine(screen);
            Console.WriteLine($"\n{message}");
            Console.WriteLine($"\n{menu}");
        }
        int pNum;
        public void Run() {
            Console.ForegroundColor = ConsoleColor.Green;
            bool valid = false;
            String message = "Welcome to Ascii Adventure! Please enter the number of people that will be playing(1-4)";
            String curMessage = "";
            
            while (!valid)
            {
                Console.WriteLine(message);
                char pNumChar= Console.ReadKey(true).KeyChar;
                pNum = pNumChar - '0';
                if (pNum < 1 || pNum > 4)
                    message = $"{pNumChar} is not a valid number. Please enter the number of people that will be playing(1-4)";
                else
                    valid = true;
            }

            
            List<Screen> screens = new List<Screen>();
            /*for (int i = 0; i < pNum *2; i+=2) {
                //screens.Add(new Screen(0, 0, i));//P[i] Menu Screen
                screens.Add(new Screen(10, 10, i + 1));//P[i] Run Screen
            }*/
            for (int i = 0; i < pNum; i++)
            {
                screens.Add(new Screen(10, 10, i));//P[i] Run Screen, and P[i+1] Menu Screen
            }

            List<Player> players = new List<Player>();                                          //MULTI-PLAYER FUNCTIONALITY
            List<MovingGameObject>[] mobs = new List<MovingGameObject>[pNum];
            for(int i = 0; i < pNum; i++){
                mobs[i] = new List<MovingGameObject>();
            }
            /*
            for (int i = 1; i < screens.Count; i += 2) {
                // add a couple of walls to each player's screen
                for (int j = 0; j < 3; j++) {
                    new Wall(1, 2 + j, screens[i]);
                }
                for (int j = 0; j < 4; j++) {
                    new Wall(3 + j, 4, screens[i]);
                }
                // add a player to each player's screen
                players.Add(new Player(0, 0, screens[i], $"P{i / 2}"));
                // add a treasure to each player's screen
                Treasure treasure = new Treasure(6, 2, screens[i]);
                // add some mobs to each screen
                mobs[screen.Index].Add(new Mob(9, 9, screens[i]));
            }
            */
            foreach(Screen screen in screens){                                                //PROCEDUREALLY GENERATED MAP
                // add a player to each player's screen
                players.Add(new Player(0, 0, screen, $"P{screen.Index}"));
                new Treasure(9,9, screen);
                int randNum;
                for(int j = 1; j < screen.NumRows; j++){
                    for(int k = 0; k < screen.NumCols; k++){
                        if(!screen.IsOtherObject(j, k)){
                            randNum = random.Next() % 100;
                            if(randNum < 40){
                                new Wall(j, k, screen);
                            }else if(randNum < 35){
                                mobs[screen.Index].Add(new Mob(j, k, screen));
                            }
                         }
                     }
                 }
            }
            Console.Clear();
            message = ($"Enjoy your game of {pNum} players! You will take turns, and make 1 move each turn. Press any key to continue!");
            Boolean gameOver = false;
            char input;
            List<int> lostIndexes = new List<int>();
            while (!gameOver) {
                foreach (Screen screen in screens) {
                    if(!lostIndexes.Contains(screen.Index)){
                        message += $"Player {screen.Index}'s turn!  \nYou can only move once, so be careful.";
                        PrintScreen(screen, message, Menu());
                        input = Console.ReadKey(true).KeyChar;
                        message = "";
                        curMessage = "";
                        if (Eq(input, 'q')){
                            gameOver = true;
                            break;
                        }else if (Eq(input, 'w'))
                            players[screen.Index] = (Player)(players[screen.Index].Move(-1, 0));
                        else if (Eq(input, 's'))
                            players[screen.Index] = (Player)(players[screen.Index].Move(1, 0));
                        else if (Eq(input, 'a'))
                            players[screen.Index] = (Player)(players[screen.Index].Move(0, -1));
                        else if (Eq(input, 'd'))
                            players[screen.Index] = (Player)(players[screen.Index].Move(0, 1));
                        else if (Eq(input, 'v'))// TODO: handle inventory
                            message = "You have nothing\n";
                        else 
                        {
                            curMessage = "";                                                 //NEW OBJECT: BULLET. NEW IJKL FUNCTIONALITY: FIRE
                            if (Eq(input, 'i'))
                            {
                                curMessage = players[screen.Index].Action(-1, 0);
                                if(curMessage == "Fire!")
                                    mobs[screen.Index].Add(new Bullet(players[screen.Index].Row, players[screen.Index].Col, screen, Direction.UP));
                            }
                            else if (Eq(input, 'k'))
                            {
                                curMessage = players[screen.Index].Action(1, 0);
                                if(curMessage == "Fire!")
                                    mobs[screen.Index].Add(new Bullet(players[screen.Index].Row, players[screen.Index].Col, screen, Direction.DOWN));
                            }
                            else if (Eq(input, 'j'))
                            {
                                curMessage = players[screen.Index].Action(0, -1);
                                if(curMessage == "Fire!")
                                    mobs[screen.Index].Add(new Bullet(players[screen.Index].Row, players[screen.Index].Col, screen, Direction.LEFT));
                            }
                            else if (Eq(input, 'l'))
                            {
                                curMessage = players[screen.Index].Action(0, 1);
                                if(curMessage == "Fire!")
                                    mobs[screen.Index].Add(new Bullet(players[screen.Index].Row, players[screen.Index].Col, screen, Direction.RIGHT));
                            }
                            else
                                message = $"Unknown command: {input}";
                            message += curMessage + "\n";
                        }
                        if (curMessage == "Yay, we got the treasure!"){
                            message += $"Player {screen.Index} won! Press any key to exit the game!";
                            PrintScreen(new Screen(0,0,0), message, Menu());
                            gameOver = true;
                            break;
                        }
                        curMessage = "";

                        // OK, now move the mobs
                        for(int i = mobs[screen.Index].Count - 1; i >= 0 ; i--){
                            // TODO: Make mobs smarter, so they jump on the player, if it's possible to do so
                            List<Twin<int, int>> moves = screen.GetLegalMoves(mobs[screen.Index][i].Row, mobs[screen.Index][i].Col); //See Twin class for explanation
                            if (moves.Count == 0)
                            {
                                continue;
                            }
                            // mobs move randomly            
                            var (deltaRow, deltaCol) = moves[random.Next(moves.Count)];
                            List<GameObject> others = screen[mobs[screen.Index][i].Row + deltaRow, mobs[screen.Index][i].Col + deltaCol];
                            foreach (GameObject other in others){
                                if (other is Player)
                                {
                                   // the mob got the player!
                                   mobs[screen.Index][i].Token = "*";
                                   message += "A MOB GOT YOU! GAME OVER\n";
                                   lostIndexes.Add(screen.Index);
                                }
                            }
                            MovingGameObject temp = (mobs[screen.Index][i].Move(deltaRow, deltaCol));
                            if(temp !=  mobs[screen.Index][i] || temp is Mob){
                                mobs[screen.Index][i] = temp;
                            }else{
                                screen[mobs[screen.Index][i].Row, mobs[screen.Index][i].Col].Remove(mobs[screen.Index][i]);
                                mobs[screen.Index].RemoveAt(i);
                            }
                            
                        }
                        for (int i = 0; i < mobs[screen.Index].Count; i++)
                            if(mobs[screen.Index][i].Removed)
                                mobs[screen.Index].RemoveAt(i);

                        if(pNum > 1){
                            message += $"Player {(screen.Index + 1) % pNum}'s turn next. Press any key to continue onto player {(screen.Index + 1) % pNum}'s board,\nthen press one of the following keys to move.";
                            PrintScreen(screen, message, Menu());
                            input = Console.ReadKey(true).KeyChar;
                        }
                        message = "";
                    }
                    else{
                        if(lostIndexes.Count >= pNum - 1){                                  //Alternate Win-Con: ELIMINATION
                            if(pNum == 1){
                                message += "A Mob got you. You Lost!\n";
                                PrintScreen(new Screen(0,0,0), message, Menu());
                                gameOver = true;
                                break;
                            }else{
                            for(int i = 0; i < pNum; i++){
                                if(!lostIndexes.Contains(i)){
                                    message += $"Player {i} Wins due to Elimination!";
                                    PrintScreen(new Screen(0,0,0), message, Menu());
                                    gameOver = true;
                                    break;
                                }
                            }
                            }
                        }
                        message += $"Player {screen.Index}, as well as {lostIndexes.Count - 1} other players eliminated. Screen skipped.\n";
                        continue;
                    }
                }
            }
            char exit = Console.ReadKey(true).KeyChar;
        }

        public static void Main(string[] args) {
            Game game = new Game();
            game.Run();
        }
    }
}