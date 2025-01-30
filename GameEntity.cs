using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProg_OOPGameRefactoring_JuliaC01202025
{
    public class GameEntity
    {
        //shared properties between player/enemy (cards need to reference these)
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Mana { get; set; }
        public int Shield { get; private set; }
        public bool HasFireBuff { get; set; }
        public bool HasIceShield { get; set; }


        public GameEntity(string name)
        {
            Name = name;
            Health = 100;
            Mana = 100;
            Shield = 0;
            HasFireBuff = false;
            HasIceShield = false;
        }

        public void TakeDamage(int damage)
        {
            if (HasFireBuff) damage *= 2;
            if (HasIceShield) damage /= 2;

            if (Shield > 0)
            {
                if (Shield >= damage)
                {
                    Shield -= damage;
                    damage = 0;
                }
                else
                {
                    damage -= Shield;
                    Shield = 0;
                }
            }

            Health = Math.Max(0, Health - damage);
            Console.WriteLine($"{Name} takes {damage} damage! remaining HP: {Health}");
        }

        public bool SpendMana(int cost)
        {
            if (Mana >= cost)
            {
                Mana -= cost;
                return true;
            }
            Console.WriteLine($"{Name} doesn't have enough mana!");
            return false;
        }

        public void GainShield(int amount)
        {
            Shield += amount;
        }

        public void ApplyFireBuff()
        {
            HasFireBuff = true;
        }

        public void ApplyIceShield()
        {
            HasIceShield = true;
        }

        public void RegenerateMana()
        {
            Mana = Math.Min(100, Mana + 50);
        }

        public void ResetBuffs()
        {
            HasFireBuff = false;
            HasIceShield = false;
        }
    }
}
