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
        public static int PlayerHealth { get; set; } = 100;
        public static int PlayerMana { get; set; } = 100;
        public static int PlayerShield { get; set; } = 0;
        public static bool PlayerHasFireBuff { get; set; } = false;
        public static bool PlayerHasIceShield { get; set; } = false;

        public static int EnemyHealth { get; set; } = 100;
        public static int EnemyMana { get; set; } = 100;
        public static int EnemyShield { get; set; } = 0;
        public static bool EnemyHasFireBuff { get; set; } = false;
        public static bool EnemyHasIceShield { get; set; } = false;

        //health
        //protected static int playerHealth = 100, playerMaxHealth = 100;
       // public static int PlayerHealth { get => playerHealth; // in individual cards. set values like these at the start, then call general methods
        //    set {
        //        playerHealth = Math.Max(0, Math.Min(playerMaxHealth, value));
        //    }
       // }

        int damage;
        public int Damage { get; protected set; } // protected makes it so the children classses can change it

        //general methods
        // needs to properly give damage using card dmg as base, check in runtime if buffed/shielded
        public static void TakeDamage(int targetHealth, int targetShield, int damage, bool hasFireBuff, bool hasIceShield)
        {
            // Apply buffs or shields to the damage
            if (hasFireBuff) damage *= 2;
            if (hasIceShield) damage /= 2;

            // Shield absorbs damage first
            if (targetShield > 0)
            {
                if (targetShield >= damage)
                {
                    targetShield -= damage;
                    damage = 0;
                }
                else
                {
                    damage -= targetShield;
                    targetShield = 0;
                }
            }

            // Apply remaining damage to health
            targetHealth = Math.Max(0, targetHealth - damage);
        }

        //static int playerMana = 100;
        //public static int PlayerMana { get => playerMana; }
        // needs to check if players current mana is enough to cast, return if not
        public static bool CheckMana(int currentMana, int manaNeeded)
        {
            if (currentMana >= manaNeeded) return true;
            Console.WriteLine("Not enough mana!");
            return false;
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
        public static void UseCard(bool isPlayer)
        {
            const int manaCost = 30;
            const int baseDamage = 40;

            if (isPlayer)
            {
                if (!CheckMana(PlayerMana, manaCost)) return;

                PlayerMana -= manaCost;
                TakeDamage(EnemyHealth, EnemyShield, baseDamage, PlayerHasFireBuff, EnemyHasIceShield);
                Console.WriteLine($"Player casts Fireball for {baseDamage} damage!");
            }
            else
            {
                if (!CheckMana(EnemyMana, manaCost)) return;

                EnemyMana -= manaCost;
                TakeDamage(PlayerHealth, PlayerShield, baseDamage, EnemyHasFireBuff, PlayerHasIceShield);
                Console.WriteLine($"Enemy casts Fireball for {baseDamage} damage!");
            }
        }
    }

    class IceShield : Cards
    {
        public static void UseCard(bool isPlayer)
        {
            const int manaCost = 20;
            const int shieldAmount = 30;

            if (isPlayer)
            {
                if (!CheckMana(PlayerMana, manaCost)) return;

                PlayerMana -= manaCost;
                PlayerShield += shieldAmount;
                PlayerHasIceShield = true;
                Console.WriteLine("Player gains Ice Shield!");
            }
            else
            {
                if (!CheckMana(EnemyMana, manaCost)) return;

                EnemyMana -= manaCost;
                EnemyShield += shieldAmount;
                EnemyHasIceShield = true;
                Console.WriteLine("Enemy gains Ice Shield!");
            }
        }
    }

    class Heal : Cards
    {
        public static void UseCard(bool isPlayer)
        {
            const int manaCost = 40;
            const int healAmount = 40;

            if (isPlayer)
            {
                if (!CheckMana(PlayerMana, manaCost)) return;

                PlayerMana -= manaCost;
                PlayerHealth = Math.Min(100, PlayerHealth + healAmount);
                Console.WriteLine("Player heals 40 health!");
            }
            else
            {
                if (!CheckMana(EnemyMana, manaCost)) return;

                EnemyMana -= manaCost;
                EnemyHealth = Math.Min(100, EnemyHealth + healAmount);
                Console.WriteLine("Enemy heals 40 health!");
            }
        }
    }

    class Slash : Cards
    {
        public static void UseCard(bool isPlayer)
        {
            const int manaCost = 20;
            const int baseDamage = 20;

            if (isPlayer)
            {
                if (!CheckMana(PlayerMana, manaCost)) return;

                PlayerMana -= manaCost;
                TakeDamage(EnemyHealth, EnemyShield, baseDamage, PlayerHasFireBuff, false);
                Console.WriteLine($"Player slashes for {baseDamage} damage!");
            }
            else
            {
                if (!CheckMana(EnemyMana, manaCost)) return;

                EnemyMana -= manaCost;
                TakeDamage(PlayerHealth, PlayerShield, baseDamage, EnemyHasFireBuff, false);
                Console.WriteLine($"Enemy slashes for {baseDamage} damage!");
            }
        }
    }

    class PowerUp : Cards
    {
        public static void UseCard(bool isPlayer)
        {
            const int manaCost = 30;

            if (isPlayer)
            {
                if (!CheckMana(PlayerMana, manaCost)) return;

                PlayerMana -= manaCost;
                PlayerHasFireBuff = true;
                Console.WriteLine("Player gains Fire Buff!");
            }
            else
            {
                if (!CheckMana(EnemyMana, manaCost)) return;

                EnemyMana -= manaCost;
                EnemyHasFireBuff = true;
                Console.WriteLine("Enemy gains Fire Buff!");
            }
        
        }
    }
}
