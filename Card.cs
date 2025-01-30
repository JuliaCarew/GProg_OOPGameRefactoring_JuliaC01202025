using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProg_OOPGameRefactoring_JuliaC01202025
{
    public abstract class Cards
    {
        public string Name { get; protected set; }
        public int ManaCost { get; protected set; }
        public int Damage { get; protected set; }

        public abstract void UseCard(GameEntity user, GameEntity target);
    }

    public class Fireball : Cards
    {
        public Fireball()
        {
            ManaCost = 30;
        }

        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            target.TakeDamage(40);
            Console.WriteLine($"{user.Name} casts Fireball for {Damage} damage!");
        }
    }

    public class IceShield : Cards
    {
        public IceShield()
        {
            ManaCost = 20;
        }

        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            user.GainShield(30);
            user.ApplyIceShield();
            Console.WriteLine($"{user.Name} gains an Ice Shield!");
        }
    }

    public class Heal : Cards
    {
        public Heal()
        {
            ManaCost = 40;
        }

        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            //user.Health = Math.Min(100, user.Health + 40);
            Console.WriteLine($"{user.Name} heals 40 health!");
        }
    }

    public class Slash : Cards
    {
        public Slash()
        {
            ManaCost = 20;
        }

        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            target.TakeDamage(20);
            Console.WriteLine($"{user.Name} slashes for {Damage} damage!");
        }
    }

    public class PowerUp : Cards
    {
        public PowerUp()
        {
            ManaCost = 30;
        }

        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            user.ApplyFireBuff();
            Console.WriteLine($"{user.Name} gains a Fire Buff!");
        }
    }

    public class Meditate : Cards
    {
        public Meditate()
        {
            ManaCost = 10;
        }

        public override void UseCard(GameEntity user, GameEntity target)
        {
            if (!user.SpendMana(ManaCost)) return;
            user.RegenerateMana();
            Console.WriteLine($"{user.Name} meditates to regenerate 50 mana!");
        }
    }
}
