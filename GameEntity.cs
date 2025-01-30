using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProg_OOPGameRefactoring_JuliaC01202025
{
    public class GameEntity
    {
        // Entities are PLAYER and ENEMY,
        // they have access to the same 'actions'
        // like taking turns, using mana, taking damage, and applying/using cards and their effects.
        // The cards then specify which 'action' to take in their respective classes.
        // this prevents hadrcoding cards and allows flexibility & expandability

        //shared properties between player/enemy (cards need to reference these)
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Mana { get; set; }
        public int Shield { get; private set; }
        public bool HasFireBuff { get; set; }
        public bool HasIceShield { get; set; }

        // Deck related properties
        public List<Cards> Deck { get; private set; }
        public List<Cards> Hand { get; private set; }

        // Constructor to initialize the entity's attributes
        public GameEntity(string name)
        {
            Name = name;
            Health = 100;
            Mana = 100;
            Shield = 0;
            HasFireBuff = false;
            HasIceShield = false;
            Hand = new List<Cards>();
            Deck = new List<Cards>();
        }

        // Method to draw cards (used by both player and enemy)
        // merged both individual player/enemy methods from Program.cs
        public void DrawCards()
        {
            while (Hand.Count < 3 && Deck.Count > 0)
            {
                Hand.Add(Deck[0]);
                Deck.RemoveAt(0);
            }
            if (Deck.Count == 0 && Hand.Count < 3)
            {
                Console.WriteLine($"{Name} has no more cards in the deck to draw!");
            }
        }

        // Reduces health based on incoming damage, considering buffs and shield protection
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

        // Deducts mana for an action, returns true if successful, false if insufficient mana
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

        // Increases shield value by a specified amount
        public void GainShield(int amount)
        {
            Shield += amount;
        }

        // Applies a fire buff, making the opponent take double damage
        public void ApplyFireBuff()
        {
            HasFireBuff = true;
        }

        // Applies an ice shield, making the entity take half damage
        public void ApplyIceShield()
        {
            HasIceShield = true;
        }

        // Heals the user 40 hp
        public void Heal()
        {
            Health = Math.Min(100, Health + 40);
        }

        // Regenerates 50 mana, up to a maximum of 100
        public void RegenerateMana()
        {
            Mana = Math.Min(100, Mana + 50);
        }
    }
}
