using GProg_OOPGameRefactoring_JuliaC01202025;
using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    // create player & enemy entities
    static GameEntity player;
    static GameEntity enemy;

    static Random random = new Random();

    static void Main(string[] args)
    {
        // Initializes player and enemy entities
        InitializeEntities();

        Console.WriteLine("=== Card Battle Game ===");
        // Initializes decks with predefined cards
        InitializeDecks();

        // --------------- Main game loop --------------- //
        while (player.Health > 0 && enemy.Health > 0)
        {
            // Draw cards if needed (always sure to have 3)
            player.DrawCards();
            enemy.DrawCards();

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

    // Initializes player and enemy entities (called in Main())
    static void InitializeEntities()
    {
        player = new GameEntity("Player");
        enemy = new GameEntity("Enemy");
    }

    // Fills player and enemy decks with cards and shuffles them
    static void InitializeDecks()
    {
        // Add cards to player deck, made into list of card objects for ease of use/reducing if's
        player.Deck.AddRange(new List<Cards>
        {
            new Fireball(), new IceShield(), new Heal(), new Slash(), new PowerUp(), new Meditate()
        });

        enemy.Deck.AddRange(new List<Cards>
        {
            new Fireball(), new IceShield(), new Heal(), new Slash(), new PowerUp(), new Meditate()
        });


        // Shuffle decks
        ShuffleDeck(player.Deck);
        ShuffleDeck(enemy.Deck);
    }

    // Shuffles a deck (called for player/enemy in InitializeDeck())
    static void ShuffleDeck(List<Cards> deck)
    {
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Cards temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    // Handles a turn for either the player or enemy
    static void PlayTurn(bool isPlayer)
    {
        var entity = isPlayer ? player : enemy;
        var opponent = isPlayer ? enemy : player;
        var hand = isPlayer ? player.Hand : enemy.Hand;

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

            Cards selectedCard = hand[choice - 1];
            selectedCard.UseCard(entity, opponent);
            hand.RemoveAt(choice - 1);
        }
        else
        {
            // randomly play a card if enough mana
            int cardIndex = random.Next(hand.Count);
            Cards selectedCard = hand[cardIndex];

            // Check if enough mana
            if (selectedCard != null && entity.Mana >= selectedCard.ManaCost)
            {
                selectedCard.UseCard(entity, opponent);
                hand.RemoveAt(cardIndex);
            }
        }
    }

    // Updates buffs and regenerates mana for player or enemy
    static void UpdateBuffs(bool isPlayer)
    {
        var entity = isPlayer ? player : enemy;
        entity.Mana = Math.Min(100, entity.Mana + 20);
        entity.HasFireBuff = false;
        entity.HasIceShield = false;
    }

    // Displays the current game state including health, mana, and hand
    static void DisplayGameState()
    {
        Console.WriteLine($"\nPlayer Health: {player.Health} | Mana: {player.Mana} | Shield: {player.Shield}");
        Console.WriteLine($"Enemy Health: {enemy.Health} | Mana: {enemy.Mana} | Shield: {enemy.Shield}");

        Console.WriteLine("\nYour hand:");
        for (int i = 0; i < player.Hand.Count; i++)
        {
            var card = player.Hand[i];
            Console.WriteLine($"{i + 1}. {card.Description}");
        }
    }

}