using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld
{
    class Game
    {
        bool _gameOver = false;
        string _playerName = "Hero";
        int _playerHealth = 120;
        int _playerDamage = 20;
        int _playerDefense = 10;
        int levelScaleMax = 5;
        int _healMax = 200;
        //Run the game
        public void Run()
        {
            Start();

            while(_gameOver == false)
            {
                Update();
            }
            End();
        }
        //This function handles the battles for our ladder. roomNum is used to update the our opponent to be the enemy in the current room. 
        //turnCount is used to keep track of how many turns it took the player to beat the enemy
        bool StartBattle(int roomNum, ref int turnCount)
        {
            //initialize default enemy stats
            int enemyHealth = 100;
            int enemyAttack = 10;
            int enemyDefense = 20;
            string enemyName = " ";
            //Changes the enemy's default stats based on our current room number. 
            //This is how we make it seem as if the player is fighting different enemies
            switch (roomNum)
            {
                case 1:
                    {
                        enemyHealth = 110;
                        enemyAttack = 30;
                        enemyDefense = 10;
                        enemyName = "Goblin";
                        break;
                    }
                case 2:
                    {
                        enemyHealth = 80;
                        enemyAttack = 40;
                        enemyDefense = 5;
                        enemyName = "Wizard";
                        break;
                    }
                case 3:
                    {
                        
                        enemyHealth = 200;
                        enemyAttack = 40;
                        enemyDefense = 20;
                        enemyName = "Golem";
                        break;
                    }
                default:
                    {
                        enemyHealth = 100;
                        enemyAttack = 5;
                        enemyDefense = 1;
                        enemyName = "Crook";
                        break;
                    }
            }

            //Loops until the player or the enemy is dead
            while(_playerHealth > 0 && enemyHealth > 0)
            {
                //Displays the stats for both charactersa to the screen before the player takes their turn
                PrintStats(_playerName, _playerHealth, _playerDamage, _playerDefense);
                PrintStats(enemyName, enemyHealth, enemyAttack, enemyDefense);

                //Get input from the player
                char input;
                Console.Write("> ");
                Console.ReadKey();
                GetInput(out input, "Attack", "Defend", "heal (+20 HP)");
                //If input is 1, the player wants to attack. By default the enemy blocks any incoming attack
                if(input == '1')
                {
                    _playerDamage -= enemyDefense;
                    enemyHealth -= _playerDamage;
                    Console.WriteLine("\nYou dealt " + _playerDamage + " damage.");
                    _playerDamage += enemyDefense;
                    Console.WriteLine("the " + enemyName + " has " + enemyHealth + " remaining");
                    Console.Write("> ");
                    Console.ReadKey();
                    if (enemyHealth <= 0)
                    {
                        Console.WriteLine("\nyou have defeated " + enemyName + " you may now climb the ladder");
                        Console.WriteLine(">");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
                    enemyAttack -= _playerDefense;
                    _playerHealth -= enemyAttack;
                    Console.WriteLine("\n" + enemyName + " dealt " + enemyAttack + " damage.");
                    enemyAttack += _playerDefense;
                    Console.Write("> ");
                    Console.ReadKey();
                }
                //If the player decides to defend the enemy just takes their turn. However this time the block attack function is
                //called instead of simply decrementing the health by the enemy's attack value.
                else if(input == '2')
                {
                    _playerDefense += 10; 
                    enemyAttack -= _playerDefense;
                    _playerHealth -= enemyAttack;
                    Console.WriteLine("\nYou prepare for the enemy's attack, you block for " + _playerDefense + " points of defence");
                    Console.WriteLine("\n" + enemyName + " dealt " + enemyAttack + " damage.");
                    enemyAttack += _playerDefense;
                    _playerDefense -= 10;
                    Console.WriteLine("\nyou have " + _playerHealth + "  health remaining");
                    Console.Write("> ");
                    Console.ReadKey();
                }
                else if(input == '3')
                {
                    Console.WriteLine("\nYou heal 25 HP");
                    _playerHealth += 25;
                    enemyAttack -= _playerDefense;
                    _playerHealth -= enemyAttack;
                    Console.WriteLine("\n" + enemyName + " dealt " + enemyAttack + " damage.");
                    enemyAttack += _playerDefense;
                    Console.Write("> ");
                    Console.ReadKey();
                }
                Console.Clear();
                //After the player attacks, the enemy takes its turn. Since the player decided not to defend, the block attack function is not called.
                
               
                
            }
            //Return whether or not our player died
            return _playerHealth <= 0;

        }
        //attack code for both ai and player
        void Attack(int opponentHealth, int attackVal, int opponentDefense)
        {
            opponentHealth -= attackVal;
        }
        //Decrements the health of a character. The attack value is subtracted by that character's defense
        void BlockAttack(int opponentHealth, int attackVal, int opponentDefense)
        {
            attackVal -= opponentDefense;
            opponentHealth -= attackVal;
            attackVal += opponentDefense;
        }
        //Scales up the player's stats based on the amount of turns it took in the last battle
        void LevelUp(int exp)
        {
            //Subtract the amount of turns from our maximum level scale to get our current level scale
            int scale = levelScaleMax += exp;
            if(scale <= 0)
            {
                scale = 1;
            }
            _playerHealth += 10 * scale;
            _playerDamage += scale;
            _playerDefense += scale;
        }
        //Gets input from the player
        //Out's the char variable given. This variables stores the player's input choice.
        //The parameters option1 and option 2 displays the players current chpices to the screen
        void GetInput(out char input,string option1, string option2, string option3)
        {
            //Initialize input
            input = ' ';
            
            //Loop until the player enters a valid input
            while (input != '1' && input != '2' && input != '3')
            {
                
                Console.WriteLine("\n1." + option1);
                Console.WriteLine("2." + option2);
                Console.WriteLine("3." + option3);
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
            }   
        }

        //Prints the stats given in the parameter list to the console
        void PrintStats(string name, int health, int damage, int defense)
        {
            Console.WriteLine("\n" + name);
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Damage: " + damage);
            Console.WriteLine("Defense: " + defense);
        }

        //This is used to progress through our game. A recursive function meant to switch the rooms and start the battles inside them.
        void ClimbLadder(int roomNum)
        {
            
            //Displays context based on which room the player is in
            switch (roomNum)
            {
                case 1:
                    {
                        Console.Clear();
                        Console.WriteLine("A Goblin blocks the next ladder");
                        break;
                    }
                case 2:
                    {
                        Console.Clear();
                        Console.WriteLine("An evil Wizard stands before you");
                        break;
                    }
                case 3:
                    {
                        Console.Clear();
                        Console.WriteLine("A Golem forms in front of you!");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("You enter a castle with three corridors pick one");
                        return;
                    }
            }
            int exp = 0;
            //Starts a battle. If the player survived the battle, level them up and then proceed to the next room.
            if(StartBattle(roomNum, ref exp))
            {
                LevelUp(exp);
                ClimbLadder(roomNum++);
            }
            _gameOver = true;

        }

        //Displays the character selection menu. 
        void SelectCharacter()
        {
            char input = ' ';
            //Loops until a valid option is choosen
            while(input != '1' && input != '2' && input != '3')
            {

                //Prints options
                Console.WriteLine("Welcome! Please select a character.");
                Console.WriteLine("1.Sir Kibble");
                Console.WriteLine("2.Gnojoel");
                Console.WriteLine("3.Joedazz");
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
                //Sets the players default stats based on which character was picked
                if (input == '1')
                {
                    _playerName = "Sir Kibble";
                    _playerHealth = 120;
                    _playerDefense = 10;
                    _playerDamage = 40;

                }
                else if (input == '2')
                {
                    _playerName = "Gnojoel";
                    _playerHealth = 70;
                    _playerDefense = 20;
                    _playerDamage = 70;
                    break;
                }
                else if (input == '3')
                {
                    _playerName = "Joedazz";
                    _playerHealth = 200;
                    _playerDefense = 10;
                    _playerDamage = 30;
                    break;
                }
                //If an invalid input is selected display and input message and input over again.
                else
                {
                    Console.WriteLine("Invalid input. Press any key to continue.");
                    Console.Write("> ");
                    Console.ReadKey();
                }
                
                Console.Clear();
            }
            //Prints the stats of the choosen character to the screen before the game begins to give the player visual feedback
            PrintStats(_playerName,_playerHealth,_playerDamage,_playerDefense);
            Console.WriteLine("Press any key to continue.");
            Console.Write("> ");
            Console.ReadKey();
            Console.Clear();
        }

        //chamber after each room to give the player buffs
        void HealingChamber()
        {
            char input;
            PrintStats(_playerName, _playerHealth, _playerDamage, _playerDefense);

            Console.WriteLine("\nDo you wish to heal or gain stregth?");
            GetInput(out input, "Heal for 100 HP", "gain 20 attack", "Gain 20 defence");
            if(input == '1')
            {
                Console.WriteLine("\nYou dranked a healing potion(+100 HP gained)");
                _playerHealth += 100;
                PrintStats(_playerName, _playerHealth, _playerDamage, _playerDefense);
                Console.WriteLine(">");
                Console.ReadKey();
                return;
            }
            else if(input == '2')
            {
                Console.WriteLine("\nYou sharpen your weapon(+20 attack gained)");
                _playerDamage += 20;
                PrintStats(_playerName, _playerHealth, _playerDamage, _playerDefense);
                Console.WriteLine(">");
                Console.ReadKey();
                return;
            }
            else if(input == '3')
            {
                Console.WriteLine("You find a sturdy sheild (+20 defence)");
                _playerDefense += 20;
                PrintStats(_playerName, _playerHealth, _playerDamage, _playerDefense);
                Console.WriteLine(">");
                Console.ReadKey();
            }

        }
        //Performed once when the game begins
        public void Start()
        {
            SelectCharacter();
        }

        //Repeated until the game ends
        public void Update()
        {
            ClimbLadder(1);
            HealingChamber();
            ClimbLadder(2);
            HealingChamber();
            ClimbLadder(3);
        }

        //Performed once when the game ends
        public void End()
        {
            //If the player died print death message
            if(_playerHealth <= 0)
            {
                Console.WriteLine("Failure");
                return;
            }
            if(_playerHealth > 0)
            {
                Console.WriteLine("You've completed the ladder!");
                Console.WriteLine(">");
                Console.ReadKey();
            }
            //Print game over message
            Console.WriteLine("Try Again");
        }
    }
}
