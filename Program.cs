using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static List<string> playerDeck = new List<string>();
    static List<string> playerHand = new List<string>();
    static List<string> enemyDeck = new List<string>();
    static List<string> enemyHand = new List<string>();

    static int playerHealth = 100;
    static int playerMana = 100;
    static int playerShield = 0;
    static bool playerHasFireBuff = false;
    static bool playerHasIceShield = false;

    static int enemyHealth = 100;
    static int enemyMana = 100;
    static int enemyShield = 0;
    static bool enemyHasFireBuff = false;
    static bool enemyHasIceShield = false;

    static Random random = new Random();

    static void Main(string[] args)
    {
        Console.WriteLine("=== Card Battle Game ===");
        InitializeDecks();

        while (playerHealth > 0 && enemyHealth > 0)
        {
            // Draw cards if needed
            if (playerHand.Count < 3) PlayerDrawCards();
            if (enemyHand.Count < 3) EnemyDrawCards();

            // Player turn
            DisplayGameState();
            PlayTurn(true);

            if (enemyHealth <= 0) break;

            // Enemy turn
            Console.WriteLine("\nEnemy's turn...");
            Thread.Sleep(1000);
            PlayTurn(false);

            if (playerHealth <= 0) break;

            // End of round effects
            UpdateBuffs(true);
            UpdateBuffs(false);

            Console.WriteLine("\nPress any key for next round...");
            Console.ReadKey();
            Console.Clear();
        }

        Console.WriteLine(playerHealth <= 0 ? "You Lost!" : "You Won!");
        Console.ReadKey();
    }

    static void InitializeDecks()
    {
        // Add cards to player deck
        for (int i = 0; i < 5; i++) playerDeck.Add("FireballCard");
        for (int i = 0; i < 5; i++) playerDeck.Add("IceShieldCard");
        for (int i = 0; i < 3; i++) playerDeck.Add("HealCard");
        for (int i = 0; i < 4; i++) playerDeck.Add("SlashCard");
        for (int i = 0; i < 3; i++) playerDeck.Add("PowerUpCard");

        // Add cards to enemy deck
        for (int i = 0; i < 5; i++) enemyDeck.Add("FireballCard");
        for (int i = 0; i < 5; i++) enemyDeck.Add("IceShieldCard");
        for (int i = 0; i < 3; i++) enemyDeck.Add("HealCard");
        for (int i = 0; i < 4; i++) enemyDeck.Add("SlashCard");
        for (int i = 0; i < 3; i++) enemyDeck.Add("PowerUpCard");

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
        Console.WriteLine($"\nPlayer Health: {playerHealth} | Mana: {playerMana} | Shield: {playerShield}");
        Console.WriteLine($"Enemy Health: {enemyHealth} | Mana: {enemyMana} | Shield: {enemyShield}");

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
        return "Unknown Card";
    }

    static void PlayTurn(bool isPlayer)
    {
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

            //PlayCard(hand[choice - 1], isPlayer); //reference Card.cs
            hand.RemoveAt(choice - 1);
        }
        else
        {
            // Simple AI: randomly play a card if enough mana
            int cardIndex = random.Next(hand.Count);
            string cardToPlay = hand[cardIndex];

            // Check if enough mana
            if ((cardToPlay == "FireballCard" && enemyMana >= 30) ||
                (cardToPlay == "IceShieldCard" && enemyMana >= 20) ||
                (cardToPlay == "HealCard" && enemyMana >= 40) ||
                (cardToPlay == "SlashCard" && enemyMana >= 20) ||
                (cardToPlay == "PowerUpCard" && enemyMana >= 30))
            {
                //PlayCard(cardToPlay, isPlayer); // //reference Card.cs
                hand.RemoveAt(cardIndex);
            }
        }
    }

    static void UpdateBuffs(bool isPlayer)
    {
        if (isPlayer)
        {
            if (playerHasFireBuff) playerHasFireBuff = false;
            if (playerHasIceShield) playerHasIceShield = false;
            playerMana = Math.Min(100, playerMana + 20);
        }
        else
        {
            if (enemyHasFireBuff) enemyHasFireBuff = false;
            if (enemyHasIceShield) enemyHasIceShield = false;
            enemyMana = Math.Min(100, enemyMana + 20);
        }
    }
}
// use an interface to set up card needed variables and methods?
// look at source for ref