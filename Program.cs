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

            PlayCard(hand[choice - 1], isPlayer);
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
                PlayCard(cardToPlay, isPlayer);
                hand.RemoveAt(cardIndex);
            }
        }
    }

    // static void PlayCard(string cardName, bool isPlayer)
    // {
    //     // Huge if-else chain for card effects
    //     if (cardName == "FireballCard")
    //     {
    //         if (isPlayer)
    //         {
    //             if (playerMana >= 30)
    //             {
    //                 int damage = 40;
    //                 if (playerHasFireBuff) damage *= 2;
    //                 if (enemyHasIceShield) damage /= 2;

    //                 if (enemyShield > 0)
    //                 {
    //                     if (enemyShield >= damage)
    //                     {
    //                         enemyShield -= damage;
    //                         damage = 0;
    //                     }
    //                     else
    //                     {
    //                         damage -= enemyShield;
    //                         enemyShield = 0;
    //                     }
    //                 }

    //                 enemyHealth -= damage;
    //                 playerMana -= 30;
    //                 Console.WriteLine($"Player casts Fireball for {damage} damage!");
    //             }
    //             else
    //             {
    //                 Console.WriteLine("Not enough mana!");
    //                 return;
    //             }
    //         }
    //         else
    //         {
    //             if (enemyMana >= 30)
    //             {
    //                 int damage = 40;
    //                 if (enemyHasFireBuff) damage *= 2;
    //                 if (playerHasIceShield) damage /= 2;

    //                 if (playerShield > 0)
    //                 {
    //                     if (playerShield >= damage)
    //                     {
    //                         playerShield -= damage;
    //                         damage = 0;
    //                     }
    //                     else
    //                     {
    //                         damage -= playerShield;
    //                         playerShield = 0;
    //                     }
    //                 }

    //                 playerHealth -= damage;
    //                 enemyMana -= 30;
    //                 Console.WriteLine($"Enemy casts Fireball for {damage} damage!");
    //             }
    //             else return;
    //         }
    //     }
    //     else if (cardName == "IceShieldCard")
    //     {
    //         if (isPlayer)
    //         {
    //             if (playerMana >= 20)
    //             {
    //                 playerShield += 30;
    //                 playerHasIceShield = true;
    //                 playerMana -= 20;
    //                 Console.WriteLine("Player gains Ice Shield!");
    //             }
    //             else
    //             {
    //                 Console.WriteLine("Not enough mana!");
    //                 return;
    //             }
    //         }
    //         else
    //         {
    //             if (enemyMana >= 20)
    //             {
    //                 enemyShield += 30;
    //                 enemyHasIceShield = true;
    //                 enemyMana -= 20;
    //                 Console.WriteLine("Enemy gains Ice Shield!");
    //             }
    //             else return;
    //         }
    //     }
    //     else if (cardName == "HealCard")
    //     {
    //         if (isPlayer)
    //         {
    //             if (playerMana >= 40)
    //             {
    //                 playerHealth = Math.Min(100, playerHealth + 40);
    //                 playerMana -= 40;
    //                 Console.WriteLine("Player heals 40 health!");
    //             }
    //             else
    //             {
    //                 Console.WriteLine("Not enough mana!");
    //                 return;
    //             }
    //         }
    //         else
    //         {
    //             if (enemyMana >= 40)
    //             {
    //                 enemyHealth = Math.Min(100, enemyHealth + 40);
    //                 enemyMana -= 40;
    //                 Console.WriteLine("Enemy heals 40 health!");
    //             }
    //             else return;
    //         }
    //     }
    //     else if (cardName == "SlashCard")
    //     {
    //         if (isPlayer)
    //         {
    //             if (playerMana >= 20)
    //             {
    //                 int damage = 20;
    //                 if (playerHasFireBuff) damage *= 2;

    //                 if (enemyShield > 0)
    //                 {
    //                     if (enemyShield >= damage)
    //                     {
    //                         enemyShield -= damage;
    //                         damage = 0;
    //                     }
    //                     else
    //                     {
    //                         damage -= enemyShield;
    //                         enemyShield = 0;
    //                     }
    //                 }

    //                 enemyHealth -= damage;
    //                 playerMana -= 20;
    //                 Console.WriteLine($"Player slashes for {damage} damage!");
    //             }
    //             else
    //             {
    //                 Console.WriteLine("Not enough mana!");
    //                 return;
    //             }
    //         }
    //         else
    //         {
    //             if (enemyMana >= 20)
    //             {
    //                 int damage = 20;
    //                 if (enemyHasFireBuff) damage *= 2;

    //                 if (playerShield > 0)
    //                 {
    //                     if (playerShield >= damage)
    //                     {
    //                         playerShield -= damage;
    //                         damage = 0;
    //                     }
    //                     else
    //                     {
    //                         damage -= playerShield;
    //                         playerShield = 0;
    //                     }
    //                 }

    //                 playerHealth -= damage;
    //                 enemyMana -= 20;
    //                 Console.WriteLine($"Enemy slashes for {damage} damage!");
    //             }
    //             else return;
    //         }
    //     }
    //     else if (cardName == "PowerUpCard")
    //     {
    //         if (isPlayer)
    //         {
    //             if (playerMana >= 30)
    //             {
    //                 playerHasFireBuff = true;
    //                 playerMana -= 30;
    //                 Console.WriteLine("Player gains Fire Buff!");
    //             }
    //             else
    //             {
    //                 Console.WriteLine("Not enough mana!");
    //                 return;
    //             }
    //         }
    //         else
    //         {
    //             if (enemyMana >= 30)
    //             {
    //                 enemyHasFireBuff = true;
    //                 enemyMana -= 30;
    //                 Console.WriteLine("Enemy gains Fire Buff!");
    //             }
    //             else return;
    //         }
    //     }
    // }

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