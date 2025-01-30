using GProg_OOPGameRefactoring_JuliaC01202025;
using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static GameEntity player;
    static GameEntity enemy;

    static List<string> playerDeck = new List<string>();
    static List<string> playerHand = new List<string>();
    static List<string> enemyDeck = new List<string>();
    static List<string> enemyHand = new List<string>();

    static Random random = new Random();

    static void Main(string[] args)
    {
        InitializeEntities();
        Console.WriteLine("=== Card Battle Game ===");
        InitializeDecks();

        while (player.Health > 0 && enemy.Health > 0)
        {
            // Draw cards if needed
            if (playerHand.Count < 3) PlayerDrawCards();
            if (enemyHand.Count < 3) EnemyDrawCards();

            // Player turn
            DisplayGameState();
            PlayTurn(true);

            if (enemy.Health <= 0) break;

            // Enemy turn
            Console.WriteLine("\nEnemy's turn...");
            Thread.Sleep(1000);
            PlayTurn(false);

            if (player.Health <= 0) break;

            // End of round effects
            UpdateBuffs(true);
            UpdateBuffs(false);

            Console.WriteLine("\nPress any key for next round...");
            Console.ReadKey();
            Console.Clear();
        }

        Console.WriteLine(player.Health <= 0 ? "You Lost!" : "You Won!");
        Console.ReadKey();
    }
    static void InitializeEntities()
    {
        player = new GameEntity("Player");
        enemy = new GameEntity("Enemy");
    }
    static void InitializeDecks()
    {
        // Add cards to player deck
        for (int i = 0; i < 5; i++) playerDeck.Add("FireballCard");
        for (int i = 0; i < 5; i++) playerDeck.Add("IceShieldCard");
        for (int i = 0; i < 3; i++) playerDeck.Add("HealCard");
        for (int i = 0; i < 4; i++) playerDeck.Add("SlashCard");
        for (int i = 0; i < 3; i++) playerDeck.Add("PowerUpCard");
        for (int i = 0; i < 3; i++) playerDeck.Add("MeditateCard");

        // Add cards to enemy deck
        for (int i = 0; i < 5; i++) enemyDeck.Add("FireballCard");
        for (int i = 0; i < 5; i++) enemyDeck.Add("IceShieldCard");
        for (int i = 0; i < 3; i++) enemyDeck.Add("HealCard");
        for (int i = 0; i < 4; i++) enemyDeck.Add("SlashCard");
        for (int i = 0; i < 3; i++) enemyDeck.Add("PowerUpCard");
        for (int i = 0; i < 3; i++) playerDeck.Add("MeditateCard");

        // Shuffle decks
        ShuffleDeck(playerDeck);
        ShuffleDeck(enemyDeck);
    }

    static void ShuffleDeck(List<string> deck)
    {
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            string temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    static void PlayerDrawCards()
    {
        while (playerHand.Count < 3 && playerDeck.Count > 0)
        {
            playerHand.Add(playerDeck[0]);
            playerDeck.RemoveAt(0);
        }
    }

    static void EnemyDrawCards()
    {
        while (enemyHand.Count < 3 && enemyDeck.Count > 0)
        {
            enemyHand.Add(enemyDeck[0]);
            enemyDeck.RemoveAt(0);
        }
    }

    static void DisplayGameState()
    {
        Console.WriteLine($"\nPlayer Health: {player.Health} | Mana: {player.Mana} | Shield: {player.Shield}");
        Console.WriteLine($"Enemy Health: {enemy.Health} | Mana: {enemy.Mana} | Shield: {enemy.Shield}");

        Console.WriteLine("\nYour hand:");
        for (int i = 0; i < playerHand.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {GetCardDescription(playerHand[i])}");
        }
    }

    static string GetCardDescription(string cardName)
    {
        // Long if-else chain for card descriptions
        if (cardName == "FireballCard")
            return "Fireball (Costs 30 mana): Deal 40 damage";
        else if (cardName == "IceShieldCard")
            return "Ice Shield (Costs 20 mana): Gain 30 shield and ice protection";
        else if (cardName == "HealCard")
            return "Heal (Costs 40 mana): Restore 40 health";
        else if (cardName == "SlashCard")
            return "Slash (Costs 20 mana): Deal 20 damage";
        else if (cardName == "PowerUpCard")
            return "Power Up (Costs 30 mana): Gain fire buff for 2 turns";
        else if (cardName == "MeditateCard")
            return "Meditate (Costs 10 mana): Meditate to regenerate 50 mana";
        return "Unknown Card";
    }

    static Cards CreateCard(string cardName)
    {
        if (cardName == "FireballCard") return new Fireball();
        if (cardName == "IceShieldCard") return new IceShield();
        if (cardName == "HealCard") return new Heal();
        if (cardName == "SlashCard") return new Slash();
        if (cardName == "PowerUpCard") return new PowerUp();
        if (cardName == "MeditateCard") return new Meditate();
        return null;
    }

    static void PlayTurn(bool isPlayer)
    {
        var entity = isPlayer ? player : enemy;
        var opponent = isPlayer ? enemy : player;
        var hand = isPlayer ? playerHand : enemyHand;

        if (isPlayer)
        {
            Console.Write("\nChoose a card to play (1-3) or 0 to skip: ");
            if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int choice) || choice < 0 || choice > hand.Count)
            {
                Console.WriteLine($" {choice} Invalid choice! Turn skipped.");
                return;
            }
            Console.WriteLine(choice.ToString());
            if (choice == 0) return;

            Cards selectedCard = CreateCard(hand[choice - 1]);
            if (selectedCard != null)
            {
                selectedCard.UseCard(entity, opponent);
                hand.RemoveAt(choice - 1);
            }
        }
        else
        {
            // Simple AI: randomly play a card if enough mana
            int cardIndex = random.Next(hand.Count);
            Cards selectedCard = CreateCard(hand[cardIndex]);

            // Check if enough mana
            if (selectedCard != null && entity.Mana >= selectedCard.ManaCost)
            {
                selectedCard.UseCard(entity, opponent);
                hand.RemoveAt(cardIndex);
            }
        }
    }

    static void UpdateBuffs(bool isPlayer)
    {
        if (isPlayer)
        {
            if (player.HasFireBuff) player.HasFireBuff = false;
            if (player.HasIceShield) player.HasIceShield = false;
            player.Mana = Math.Min(100, player.Mana + 20);
        }
        else
        {
            if (enemy.HasFireBuff) enemy.HasFireBuff = false;
            if (enemy.HasIceShield) enemy.HasIceShield = false;
            enemy.Mana = Math.Min(100, enemy.Mana + 20);
        }
    }
}