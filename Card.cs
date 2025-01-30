using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProg_OOPGameRefactoring_JuliaC01202025
{
    // ----- CARD BASE CLASS ----- //
        // cards all have a name, a mana cost, and damage, and they are all used by the user on the opponent
        // idv. cards are child classes of this, with unique dmg/mana cost & logic in UseCard
    public abstract class Cards
    {
        // common properties
        public string Name { get; protected set; }
        public int ManaCost { get; protected set; }
        public int Damage { get; protected set; }

        // Virtual property to get the description of a card
           // !added from program to have each card specify their description
           // instead of holding it in a separate method there
        public virtual string Description
        {
            get { return $"{Name} (Costs {ManaCost} mana)"; }
        }

        // method to define how the card is used by the player or enemy
        public abstract void UseCard(GameEntity user, GameEntity target);
    }

    // ----- FIREBALL ----- // 
    public class Fireball : Cards // (all card classes use this general structure)
    {
        public Fireball() // initialize this class with custom properties of parent class
        {
            Name = "Fireball";
            ManaCost = 30; // Fireball costs 30 mana to use
            Damage = 40; // Fireball deals 40 damage unbuffed
        }
        // overriding the description property
        public override string Description => 
            $"{Name} (Costs {ManaCost} mana): Deal {Damage} damage";

        // uses Fireball, dealing 40 damage to the target
        public override void UseCard(GameEntity user, GameEntity target) // what happens when the card is used
        {
            if (!user.SpendMana(ManaCost)) return; // Check if the user has enough mana
            target.TakeDamage(Damage);             // Deal damage to the target
            Console.WriteLine($"{user.Name} casts Fireball for {Damage} damage!");
        }
    }

    // ----- ICE SHIELD ----- //
    public class IceShield : Cards
    {
        public IceShield()
        {
            Name = "Ice Shield";
            ManaCost = 20;
        }
        public override string Description => 
            $"{Name} (Costs {ManaCost} mana): Gain 30 shield and ice protection";
        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            user.GainShield(30);
            user.ApplyIceShield(); // Apply the ice shield effect to the user
            Console.WriteLine($"{user.Name} gains an Ice Shield!");
        }
    }

    // ----- HEAL ----- //
    public class Heal : Cards
    {
        public Heal()
        {
            Name = "HealCard";
            ManaCost = 40;
        }
        public override string Description =>
            $"{Name} (Costs {ManaCost} mana): Restore 40 health";
        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            user.Heal(); // heals the user 40 hp
            Console.WriteLine($"{user.Name} heals 40 health!");
        }
    }

    // ----- SLASH ----- //
    public class Slash : Cards
    {
        public Slash()
        {
            Name = "SlashCard";
            ManaCost = 20;
            Damage = 20;
        }
        public override string Description =>
           $"{Name} (Costs {ManaCost} mana): Deal 20 damage";
        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            target.TakeDamage(Damage); // Deal 20 damage to the target
            Console.WriteLine($"{user.Name} slashes for {Damage} damage!");
        }
    }

    // ----- POWER UP ----- //
    public class PowerUp : Cards
    {
        public PowerUp()
        {
            Name = "PowerUpCard";
            ManaCost = 30;
        }
        public override string Description =>
           $"{Name} (Costs {ManaCost} mana): Gain fire buff for 2 turns";
        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            user.ApplyFireBuff(); // Apply fire buff to the user
            Console.WriteLine($"{user.Name} gains a Fire Buff!");
        }
    }

    // ----- MEDITATE ----- //  !added
    public class Meditate : Cards
    {
        public Meditate()
        {
            Name = "MeditateCard";
            ManaCost = 10;
        }
        public override string Description =>
           $"{Name} (Costs {ManaCost} mana): Meditate to regenerate 50 mana";
        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            user.RegenerateMana(); // Regenerate 50 mana for the user
            Console.WriteLine($"{user.Name} meditates to regenerate 50 mana!");
        }
    }
}
