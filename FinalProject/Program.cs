using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace FinalProject
{

    internal class Program
    {
        //Player variable
         
        static Int32 MeSpeed = 1;
        static Int32 MeX = 10;
        static Int32 MeY = 10;
        static Int32 OldMeX = 10;
        static Int32 OldMeY = 10;
        
       

        static Int32 myclock = 0;
        static Int32 Esc = 0;
        ///static Int32 moduloSpeed = 500; was use when the character were gliding

        //Enemy variable
        
        static char EnemyChar = 'x';
        static Enemy[] Aliens;

        //Bonus Score thingies variable
        static Plus[] ScoringCoins = new Plus[1];
        static Int32 Score = 0, time = 0;
        static Plus[] TimePlus = new Plus[4];
        static Plus[] Bonus = new Plus[1];
        
        static Int32 WhichBonus = 0, timeFreeze;

        //Variable that will be change with the level of difficulty
        static Int32 MaxScore = 10, timeAdd = 10, Level = 0, bonusCoins = 4;
        static Int32 NumTarget = 20;           //Enemy variable
        static Int32 BonusPlus = 100;         // How much time the Bonus is gonna appear 
        static Int32 BonusPlusTime = 200;    //And how long
        static Int32 lives = 5;             //Player Variable


        static void Main(string[] args)
        {
            //Start of the game
            Int32 choice =0;
            do
            {
                Menu();
                choice = verrifieinput();

                switch(choice) 
                { 
                    case 1:
                        Console.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Your goal is to raise your score to 10 or 15, depanding on the difficulty, before the timer hit it 20.");
                        Console.WriteLine("Of course, monster will be on your way and you will have to not be touch by them.");
                        Console.WriteLine("To help you, bonus will be disperse all over the screen.");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Enemies(x) will be in red.");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Coins, to add 1 to your score, will be in yellow. (o)");
                        Console.WriteLine();    
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("To reduce the timer, touch the '+' in green.");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("The bonus will be in Blue, sometime it will add coins to your score, while other time are more special. '|' ");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;   
                        Console.WriteLine("If you understood press enter to return to the other screen.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("The control are super easy!");
                        Console.WriteLine();
                        Console.WriteLine("Up arrow to go up the screen.");
                        Console.WriteLine("Down arrow to go down the screen.");
                        Console.WriteLine("Left arrow make you go left.");
                        Console.WriteLine("And the right make you go to the right!");    
                        Console.WriteLine();
                        Console.WriteLine("Plus, escape make you leave the game immediatly.");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("Understood? If you did press enter to go back to the last screen.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("You have enter an invalid input. Press Enter to return to the original screen.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            } while (choice != 1);

             
            do
            {
                MenuLevel();
                choice = verrifieinput();

                switch (choice)
                {
                    case 1:
                        
                        Level = 1;
                        Esc = 0;
                        Console.Clear();
                        break;
                    case 2:
                        
                        Level = 2;
                        lives = 3;
                        NumTarget = 25;
                        bonusCoins = 3;
                        BonusPlus = 85;
                        Esc = 0;
                        Console.Clear();
                        break;
                    case 3:
                        //Level = 3;   //There is a problem with level somewhere in the code, but i havent been able to find it in time
                        //I'll keep trying to figure it out, but for now i have to send it like that.
                        lives = 3;
                        NumTarget = 30;
                        bonusCoins = 3;
                        BonusPlus = 70;
                        BonusPlusTime = 150;
                        timeAdd = 7;
                        MaxScore = 15;
                        Esc = 0;
                        Console.Clear();
                        break;
                    case 4:
                        
                        //Level = 3;  //same here
                        lives = 1;
                        NumTarget = 40;
                        bonusCoins = 2;
                        BonusPlus = 50;
                        BonusPlusTime = 125;
                        timeAdd = 5;
                        MaxScore = 15;
                        Esc = 0;
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("You have enter an invalid input. Press Enter to return to the original screen.");
                        Esc =1 ;
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            } while (Esc != 0);

            Console.CursorVisible = false;
            ConsoleKey k;
            k = ConsoleKey.NoName;  // yes at the start it is "no key"
            Aliens = new Enemy[NumTarget];
            int i;

            //Before the game start, give birth to target since they are empty 
            for (i = 0; i < NumTarget; i++)
            {
                Aliens[i] = new Enemy();
            }
            for (i = 0; i < ScoringCoins.Length; i++)
            {
                ScoringCoins[i] = new Plus();
            }
            for (i = 0; i < TimePlus.Length; i++)
            {
                TimePlus[i] = new Plus();
            }
            for (i = 0; i < Bonus.Length; i++)
            {
                Bonus[i] = new Plus();
            }


            do
            {

                myclock++;
                //This will verifie what bonus is active
                if ((WhichBonus == 2) || (WhichBonus == 3) || (WhichBonus == 0))  //So if the bonus isnt time freeze, then time will advance
                {
                    if ((myclock % 47) == 0)
                    {
                        time++;
                    }
                    if (myclock >= 300)
                    {
                        myclock = 0;
                    }
                }
                if ((WhichBonus ==1) && (myclock >= timeFreeze + BonusPlusTime))
                {
                        WhichBonus = 0;
                }
                if (WhichBonus ==1)   
                {
                    Console.SetCursorPosition(0, 1);
                    Console.WriteLine("Time is now frozen.");


                }
                if (WhichBonus == 2)  
                {
                    Score = Score + bonusCoins;
                    WhichBonus =0;
                }
                if (WhichBonus == 3)    //put the little message
                {
                    if  (myclock <= timeFreeze + BonusPlusTime)    //put the little message
                    {

                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine("You are invicible for a moment.");

                    }
                    if (myclock >= timeFreeze + BonusPlusTime)    //re put to no bonus
                    {
                        WhichBonus = 0;
                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine("                                    ");
                    }

                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, 0);
                Console.Write("Number of live remaining: " + lives + "");
                Console.SetCursorPosition(Console.WindowWidth/2, 0);
                Console.Write("Score: " + Score + "");
                Console.SetCursorPosition(Console.WindowWidth-15, 0);
                Console.Write("Timer: " + time + "");
                k = getkey(k);
                if (Gameover() == true)
                {
                    break;
                }
                Moveme(k, ref MeX, ref MeY);
                
                i++;
                i = i % NumTarget;
                System.Threading.Thread.Sleep(5);

                Aliens[i].move();

                Contact();
                draw();
                

            } while (Gameover() == false) ;

            Console.Clear();
            Console.WriteLine("If nothing appear, keep pressing Enter until the end message appear.");
            Console.ReadKey();   //multiple time so the console doesnt immediately close itself when the player kept pushing keys
            Console.ReadKey();
            Console.ReadKey(); 
            Console.ReadKey();
            Console.WriteLine();

            if (lives <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("It look like you don't have any lives left.");
                Console.WriteLine();
            }
            else if( time >= 20)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("It look like you don't have any times left.");
                Console.WriteLine();
            }
            else 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Congratulation, You won!");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Thank you for playing. Press any key to close the program.");
            
            Console.ReadKey();
        }
        static bool Gameover()
        {
            
            bool gameover = false;
            if ((lives <= 0) || (Score >= MaxScore))
            {
                gameover = true;
            }
            if (time >= 20)   //20 will always be the time max
            {
                gameover = true;
            }

            if (Esc ==1)
            {
                gameover = true;
            }
            return gameover;
        }
        static ConsoleKey getkey(ConsoleKey dk)
        {
                                              //To glide, i would need to comment this
             //dk = ConsoleKey.NoName;         //this 

            //while (dk != ConsoleKey.Escape)   // this will hang your program!!! //this
            {
                if (Console.KeyAvailable)
                {
                    dk = Console.ReadKey(true).Key;
                    //break;                    // And this
                }
                else
                {
                    dk = ConsoleKey.NoName;
                }

                return dk;
            }
        }
        static void Moveme(ConsoleKey dk, ref Int32 MeX, ref Int32 MeY)
        {
            Int32 MeXoff=0, MeYoff=0;
            OldMeX = MeX;
            OldMeY = MeY;
            //if ((myclock % moduloSpeed) == 0)     // your move logic only runs every 100th cycle
            {
                switch (dk)
                {
                    case ConsoleKey.DownArrow:
                        MeY += MeSpeed;
                        break;
                    case ConsoleKey.UpArrow:
                        MeY -= MeSpeed;
                        break;
                    case ConsoleKey.LeftArrow:
                        MeX -= MeSpeed;
                        break;
                    case ConsoleKey.RightArrow:
                        MeX += MeSpeed;
                        break;
                    case ConsoleKey.Escape:
                        Esc = 1;
                        break;
                }
                //Screen limits section
                if (MeX >= Console.WindowWidth)
                {
                    MeX = Console.WindowWidth - 5;

                }
                if (MeY >= Console.WindowHeight)
                {
                    MeY = Console.WindowHeight - 2;
                }
                if (MeY <= 0)
                {
                    MeY += 2;
                }
                if (MeX <= 0)
                {
                    MeX += 3;
                }
                MeX += MeXoff;
                MeY += MeYoff;
            } 

        }
        static void draw()
        {

            Console.SetCursorPosition(OldMeX, OldMeY);
            Console.Write(' ');
            Console.SetCursorPosition(MeX, MeY);
            Console.Write("*");
            
            Int32 i;
            for (i = 0; i < NumTarget; i++)
            {
                Console.SetCursorPosition(Aliens[i].OldX, Aliens[i].OldY);
                Console.Write(" ");

                Console.SetCursorPosition(Aliens[i].x, Aliens[i].y);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(EnemyChar);
            }
       
            Console.SetCursorPosition(ScoringCoins[0].X, ScoringCoins[0].Y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("o");

            for (i = 0; i < TimePlus.Length; i++)
            {
                Console.SetCursorPosition(TimePlus[i].X, TimePlus[i].Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("+");
            }
            if ((myclock >= BonusPlus) && (myclock <= BonusPlus+BonusPlusTime))  //this would leave the mark appear only at specific moments
            {
                
                Console.SetCursorPosition(Bonus[0].X, Bonus[0].Y);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("|");
                Console.SetCursorPosition(Console.WindowWidth/2, Console.WindowHeight-1);
                Console.Write("There is a bonus somewhere!");
            }
            else
            {
                //the problem with my level need to be here because it the only place in the code that use level 3
                /*if (Level ==3)   //this make that if the player chose hard the bonus will move even if they dont touch it (that was the plan)
                {
                    Random newPosition = new Random();
                    Bonus[0].X = newPosition.Next(1, Console.WindowWidth);    //the Bonus you will hit will span somewhere else
                    Bonus[0].Y = newPosition.Next(1, Console.WindowHeight);

                }*/
                Console.SetCursorPosition(Bonus[0].X, Bonus[0].Y);
                Console.WriteLine(" ");
                Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight - 1);
                Console.Write("                                  ");    

            }
            

        }

        public class Enemy
        {
            public Int32 x, y, OldX, OldY;
            public Int32 xoff, yoff;

            //characteristique

            public Enemy()
            {
                //Constructor here
                Random r = new Random();
                x = r.Next(1, Console.WindowWidth);
                y = r.Next(1, Console.WindowHeight);
                OldX = x;
                OldY = y;

            }

            public void move()
            {
                    Random r = new Random();
                    xoff = r.Next(-1, 2);
                    yoff = r.Next(-1, 2);

                     OldX = x;
                     OldY = y;
                     x += xoff;
                     y += yoff;

                    if ((x >= Console.WindowWidth) || (OldX >= Console.WindowWidth))
                    {
                        x = Console.WindowWidth - 5;
                        
                    }
                    if ((y >= Console.WindowHeight) || (OldY >= Console.WindowWidth))
                    {
                        y = Console.WindowHeight - 5;
                        
                    }
                    if (x <= 0)
                    {
                        x += 5;
                    }
                    if (y <= 0)
                    {
                        y += 5;
                    }

            }
        }
        public class Plus
        {
            public Int32 X, Y;

            public Plus()
            {
                //Constructor
                Random r = new Random();
                X = r.Next(1, Console.WindowWidth);
                Y = r.Next(1, Console.WindowHeight);

            }
        }
        static void Contact()
        {
            if ((WhichBonus ==0) || (WhichBonus==1) || (WhichBonus==2))    //make sure that the person cant get hit if they are invicible
            {

                for (int i = 0; i < NumTarget; i++)
                {
                    if ((Aliens[i].x == MeX) && (Aliens[i].y ==MeY))
                    {

                        lives = lives - 1;
                        Console.SetCursorPosition(MeX, MeY + 1);
                        Console.WriteLine("Ouch!");
                        Console.ReadKey();
                        Console.Clear();
                        Random newPosition = new Random();
                        Aliens[i].x = newPosition.Next(1, Console.WindowWidth);  //the monster you will hit will span somewhere else
                        Aliens[i].y = newPosition.Next(1, Console.WindowHeight);
                
                    }
                }

            }
            
            for (int i = 0; i < ScoringCoins.Length;i++)
            {
                if((ScoringCoins[i].X ==MeX)&& (ScoringCoins[i].Y == MeY))
                { 
                    Score = Score + 1;
                    Console.SetCursorPosition(MeX, MeY -1);
                    Console.WriteLine(" +1");
                    Console.ReadKey();
                    Console.Clear();
                    Random newPosition = new Random();
                    ScoringCoins[i].X = newPosition.Next(1, Console.WindowWidth);  //the monster you will hit will span somewhere else
                    ScoringCoins[i].Y = newPosition.Next(1, Console.WindowHeight);
                }

            }
            for (int i = 0; i < TimePlus.Length; i++)
            {
                if ((TimePlus[i].X == MeX) && (TimePlus[i].Y == MeY))
                {
                    time = time - timeAdd;
                    if (time < 0)
                    {
                        time = 0;
                    }
                    Console.SetCursorPosition(Console.WindowWidth-10, 1);
                    Console.WriteLine(" -"+timeAdd+" sec");
                    Console.ReadKey();
                    Console.Clear();
                    Random newPosition = new Random();
                    TimePlus[i].X = newPosition.Next(1, Console.WindowWidth);  //the monster you will hit will span somewhere else
                    TimePlus[i].Y = newPosition.Next(1, Console.WindowHeight);
                }

            }
            if ((Bonus[0].X == MeX) && (Bonus[0].Y == MeY))
            {
                
                Random BonusChoice = new Random();
                WhichBonus = BonusChoice.Next(1, 4);
                

                //this will remember when the bonus was taken
                timeFreeze = myclock;
                //if timefreeze is over 100 then revert it back to 99 because if the thing is higher than 100
                //Then the total will be over the max of myclock
                if (timeFreeze >= 100)
                {
                    timeFreeze = 99;
                }

                //Invicible, coins+, freezeTime are the choice of bonus

                Random newPosition = new Random();
                Bonus[0].X = newPosition.Next(1, Console.WindowWidth);  //the Bonus you will hit will span somewhere else
                Bonus[0].Y = newPosition.Next(1, Console.WindowHeight);
            }
        }
        static Int32 verrifieinput()    //took this from my old lab
        {
            Int32 valid = 0;
            bool goodvalue;
            do
            {
                goodvalue = Int32.TryParse(Console.ReadLine(), out valid);
                if (!goodvalue)
                {
                    Console.WriteLine("You didn't type a valid input. Try again.");
                }
            } while (!goodvalue);
            return valid;
        }
        static void Menu()
        {
            Console.WriteLine("Hello, Welcome to my game!");
            Console.WriteLine();
            Console.WriteLine(" 1. Start the game.");
            Console.WriteLine(" 2. Goal of the game.");
            Console.WriteLine(" 3. Control.");
            Console.WriteLine("     Please enter a choice.");
        }
        static void MenuLevel()
        {
            Console.WriteLine("Before starting the game, you need to chose the level you wish to try.");
            Console.WriteLine();
            Console.WriteLine(" 1. Easy.   5 lives, 20 enemies, and 10 of score.");
            Console.WriteLine(" 2. Medium. 3 lives, 25 enemies, and 10 of score.");
            Console.WriteLine(" 3. Hard.   3 lives, 30 enemies, and 15 of score.");
            Console.WriteLine(" 4. Extreme.1 lives, 40 enemies, and 15 of score.");
            Console.WriteLine();
            Console.WriteLine("Bonus will also become more rare in harder level, and will bring less interessing bonus.");
            Console.WriteLine();
            Console.WriteLine("     Please enter a choice.");
        }
    }
}