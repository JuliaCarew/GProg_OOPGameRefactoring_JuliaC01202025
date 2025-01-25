using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProg_OOPGameRefactoring_JuliaC01202025
{
    class Cards // call variables when card instantiated
    {
        // put list of cards in deck script , but its already in the gamemanager Program.cs

        //generate properties of all card variables? 
        //static int playerHealth = 100;
        //static int playerMana = 100;
        static int playerShield = 0;
        static bool playerHasFireBuff = false;
        static bool playerHasIceShield = false;

        static int enemyHealth = 100;
        static int enemyMana = 100;
        static int enemyShield = 0;
        static bool enemyHasFireBuff = false;
        static bool enemyHasIceShield = false;

        //health
        protected static int playerHealth = 100, playerMaxHealth = 100;
        public static int PlayerHealth { get => playerHealth; // in individual cards. set values like these at the start, then call general methods
            set {
                playerHealth = Math.Max(0, Math.Min(playerMaxHealth, value));
            }
        }

        int damage;
        public int Damage { get; protected set; } // protected makes it so the children classses can change it

        //general methods
        public void TakeDamage(int cardDamage, bool hasFireBuff, bool hasIceShield) // use damage property
        {
            // if this works no need to have individual checks for each in all cards
            PlayerHealth -= cardDamage;
            // needs to take into account fire buff and ice shield
            // firebuff means damage *= 2
            if (hasFireBuff) {
                Damage *= 2; // damage from the player or the enemy?
                Console.WriteLine("player has fire buff");
            }
            // iceshield means damage /= 2
            if (hasIceShield)
            {
                Damage /= 2;
                Console.WriteLine("player has ice shield");
            }
        }

        static int playerMana = 100;
        public static int PlayerMana { get => playerMana; }
        // needs to check if players current mana is enough to cast, return if not
        public void CheckMana(int manaNeeded) //, out bool isManaSufficient) // to replace the checks for each cards mana cost in the classes
            // also include bool to pass true/fanse if mana needed is right or not
        {
            //isManaSufficient = PlayerMana >= manaNeeded;
            //return isManaSufficient;
            if (manaNeeded < PlayerMana) return;
            Console.WriteLine("Not enough mana");
        }

        static void PlayCard(string cardName, bool isPlayer)
        {
            // call whatever card chosen by player or enemy (output of another method?)
        }
    }

    class Fireball : Cards
    {
        // Damage = 40; // child set value of damage
        // something like Damage = (whatever the damage of this card is)
        void FireballCard(string cardName, bool isPlayer)
        {
            if (cardName == "FireballCard")
            {
                if (isPlayer)
                {
                    CheckMana(30); // same as line below (hopefully)
                    if (playerMana >= 30)
                    {
                        //int damage = 40;
                        if (playerHasFireBuff) damage *= 2;
                        if (enemyHasIceShield) damage /= 2;

                        if (enemyShield > 0)
                        {
                            if (enemyShield >= damage)
                            {
                                enemyShield -= damage;
                                damage = 0;
                            }
                            else
                            {
                                damage -= enemyShield;
                                enemyShield = 0;
                            }
                        }

                        enemyHealth -= damage;
                        playerMana -= 30;
                        Console.WriteLine($"Player casts Fireball for {damage} damage!");
                    }
                    else
                    {
                        Console.WriteLine("Not enough mana!");
                        return;
                    }
                }
                else
                {
                    if (enemyMana >= 30)
                    {
                        int damage = 40;
                        if (enemyHasFireBuff) damage *= 2;
                        if (playerHasIceShield) damage /= 2;

                        if (playerShield > 0)
                        {
                            if (playerShield >= damage)
                            {
                                playerShield -= damage;
                                damage = 0;
                            }
                            else
                            {
                                damage -= playerShield;
                                playerShield = 0;
                            }
                        }

                        playerHealth -= damage;
                        enemyMana -= 30;
                        Console.WriteLine($"Enemy casts Fireball for {damage} damage!");
                    }
                    else return;
                }
            }
        }
    }

    class IceShield : Cards
    {
        void IceShieldCardCard(string cardName, bool isPlayer)
        {
            if (cardName == "IceShieldCard")
            {
                if (isPlayer)
                {
                    if (playerMana >= 20)
                    {
                        playerShield += 30;
                        playerHasIceShield = true;
                        playerMana -= 20;
                        Console.WriteLine("Player gains Ice Shield!");
                    }
                    else
                    {
                        Console.WriteLine("Not enough mana!");
                        return;
                    }
                }
                else
                {
                    if (enemyMana >= 20)
                    {
                        enemyShield += 30;
                        enemyHasIceShield = true;
                        enemyMana -= 20;
                        Console.WriteLine("Enemy gains Ice Shield!");
                    }
                    else return;
                }
            }
        }      
    }

    class Heal : Cards
    {
        void HealCard(string cardName, bool isPlayer)
        {
            if (cardName == "HealCard")
            {
                if (isPlayer)
                {
                    if (playerMana >= 40)
                    {
                        playerHealth = Math.Min(100, playerHealth + 40);
                        playerMana -= 40;
                        Console.WriteLine("Player heals 40 health!");
                    }
                    else
                    {
                        Console.WriteLine("Not enough mana!");
                        return;
                    }
                }
                else
                {
                    if (enemyMana >= 40)
                    {
                        enemyHealth = Math.Min(100, enemyHealth + 40);
                        enemyMana -= 40;
                        Console.WriteLine("Enemy heals 40 health!");
                    }
                    else return;
                }
            }
        }
    }

    class Slash : Cards
    {
        void SlashCard(string cardName, bool isPlayer)
        {
            if (cardName == "SlashCard")
            {
                if (isPlayer)
                {
                    if (playerMana >= 20)
                    {
                        int damage = 20;
                        if (playerHasFireBuff) damage *= 2;

                        if (enemyShield > 0)
                        {
                            if (enemyShield >= damage)
                            {
                                enemyShield -= damage;
                                damage = 0;
                            }
                            else
                            {
                                damage -= enemyShield;
                                enemyShield = 0;
                            }
                        }

                        enemyHealth -= damage;
                        playerMana -= 20;
                        Console.WriteLine($"Player slashes for {damage} damage!");
                    }
                    else
                    {
                        Console.WriteLine("Not enough mana!");
                        return;
                    }
                }
                else
                {
                    if (enemyMana >= 20)
                    {
                        int damage = 20;
                        if (enemyHasFireBuff) damage *= 2;

                        if (playerShield > 0)
                        {
                            if (playerShield >= damage)
                            {
                                playerShield -= damage;
                                damage = 0;
                            }
                            else
                            {
                                damage -= playerShield;
                                playerShield = 0;
                            }
                        }

                        playerHealth -= damage;
                        enemyMana -= 20;
                        Console.WriteLine($"Enemy slashes for {damage} damage!");
                    }
                    else return;
                }
            }
        }
    }

    class PowerUp : Cards
    {
        void PowerUpCard(string cardName, bool isPlayer)
        {
            if (cardName == "PowerUpCard")
            {
                if (isPlayer)
                {
                    if (playerMana >= 30)
                    {
                        playerHasFireBuff = true;
                        playerMana -= 30;
                        Console.WriteLine("Player gains Fire Buff!");
                    }
                    else
                    {
                        Console.WriteLine("Not enough mana!");
                        return;
                    }
                }
                else
                {
                    if (enemyMana >= 30)
                    {
                        enemyHasFireBuff = true;
                        enemyMana -= 30;
                        Console.WriteLine("Enemy gains Fire Buff!");
                    }
                    else return;
                }
            }
        }
    }
}
