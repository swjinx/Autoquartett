using AutoQuartett.ArtificialInteligence;
using AutoQuartett.Cards;
using AutoQuartett.Player;
using AutoQuartett.Player.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text.Json;


namespace AutoQuartett
{
    public class GameController
    {
        private readonly List<Card> Cards;
        private readonly List<APlayer> Players;
        private readonly Random rand;
        private Dictionary<string, int> history;
        private APlayer Active;
        private readonly HardBrain brain;
        private int cardsInTheGame = 0;

        public GameController()
        {
            Cards = new List<Card>();
            Players = new List<APlayer>();
            rand = new Random();
            brain = new HardBrain();
            SetHistory();
        }
        /// <summary>
        /// Sets up the GameHistory.
        /// </summary>
        private void SetHistory()
        {
            if (File.Exists(".\\history.json"))
            {
                var json = File.ReadAllText(".\\history.json");
                history = JsonSerializer.Deserialize<Dictionary<string,int>>(json);
            }
            else
                history = new Dictionary<string, int>();
        }
        /// <summary>
        /// Writes the history to a json file.
        /// </summary>
        private void WriteHistoryToFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(history,options);
            File.WriteAllText(".\\history.json", json);
        }
        /// <summary>
        /// Displays the history.
        /// </summary>
        private void ShowHistory()
        {
            Console.Clear();
            foreach(var tuple in history)
            {
                Console.WriteLine($"Spieler: {tuple.Key} hat {tuple.Value} Siege!");
            }
            Console.WriteLine("Eine Taste drücken um zurück zum Hauptmenü zu gelangen.");
            Console.ReadKey();
        }
        /// <summary>
        /// Adds the winner to the history or increases the amount of wins by one.
        /// </summary>
        private void AddWinnerToHistory()
        {
            if (history.ContainsKey(Active.Name))
                history[Active.Name] += 1;
            else
                history.Add(Active.Name, 1);
        }
        /// <summary>
        /// Adds all players from the Players list to the history.
        /// </summary>
        private void AddAllPlayersToHistory()
        {
            foreach(var player in Players)
            {
                if (!history.ContainsKey(player.Name))
                    history.Add(player.Name, 0);
            }
        }
        /// <summary>
        /// Logic for running the whole game.
        /// </summary>
        public void RunGame()
        {
            WriteValuesToCards();
            while (true)
            {
                ShowMainMenu();
                AddAllPlayersToHistory();
                ShuffleAllCards();
                SplitCardsToPlayers();
                SetActivePlayerForFirstTurn();
                while (RunTurn())
                    RemoveAllBeatenPlayers();
                AddWinnerToHistory();
                WriteHistoryToFile();
                ShowEndScreen();
                Players.Clear();
            }
        }
        /// <summary>
        /// Removes all beaten players from the list of players.
        /// </summary>
        private void RemoveAllBeatenPlayers()
        {
            Console.Clear();
            var defeated = Players.Where(x => x.Points == 0);
            foreach(APlayer p in defeated)
            {
                Console.WriteLine($"\"{p.Name}\" ist geschlagen und kann nicht weiter spielen!");
                Thread.Sleep(1000);
            } 
            Players.RemoveAll(x => x.Points == 0);
        }
        /// <summary>
        /// Sets the active player for the first turn.
        /// </summary>
        private void SetActivePlayerForFirstTurn()
        {
            int fst = rand.Next(0, Players.Count);
            Active = Players[fst];
        }
        /// <summary>
        /// Displays the endscreen on the console.
        /// </summary>
        private void ShowEndScreen()
        {
            Console.Clear();
            Console.WriteLine(Active.Name + " hat das Spiel gewonnen!");
            Console.WriteLine("Press any key to go back to the main menue.");
            Console.ReadKey();
        }
        /// <summary>
        /// Shuffles all the cards in the Cards-List.
        /// </summary>
        private void ShuffleAllCards()
        {
            var cardArr = Cards.ToArray();
            for (int i = 0; i < 30; i++)
            {
                int rand1 = rand.Next(0, Cards.Count);
                int rand2 = rand.Next(0, Cards.Count);
                var tmp = cardArr[rand1];
                cardArr[rand1] = cardArr[rand2];
                cardArr[rand2] = tmp;
            }
            Cards.Clear();
            Cards.AddRange(cardArr);
        }
        /// <summary>
        /// Gets the data for the cards and pushes the card object to the end of the cardslist.
        /// </summary>
        private void WriteValuesToCards()
        {
            string[] lines = File.ReadAllLines(@".\DataSource\cars.csv");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(';');
                Card c = new Card(data[0], int.Parse(data[1]), int.Parse(data[2]), double.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]));
                Cards.Add(c);
                brain.AddCard(c);
            }
            brain.DeriveAllValues();
        }
        /// <summary>
        /// Splits the cards list to all players.
        /// </summary>
        private void SplitCardsToPlayers()
        {
            int split = 32 / Players.Count;
            cardsInTheGame = Players.Count * split;
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].PushCardsToBottom(Cards.Skip(i * split).Take(split).ToList());
            }
        }
        /// <summary>
        /// Writes the choice for the players to the console and generates the players for the game.
        /// </summary>
        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Hauptmenü:\n" +
                "1)Alleine gegen den Computer\n" +
                "2)Gegen Freunde\n" +
                "3)Gegen Freunde und den Computer\n" +
                "4)Historie ansehen\n" +
                "5)Spiel beenden");
            switch (Console.ReadLine())
            {
                case "1":
                    Players.Add(new Human("Spieler 1"));
                    ChooseNicknames();
                    ChooseDifficulty();
                    break;
                case "2":
                    ChoosePlayerCount();
                    ChooseNicknames();
                    break;
                case "3":
                    ChoosePlayerCount();
                    ChooseNicknames();
                    ChooseDifficulty();
                    break;
                case "4":
                    ShowHistory();
                    ShowMainMenu();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                case "6":
                    Players.Add(new Computer("KI 1", brain));
                    Players.Add(new Computer("KI 2", brain));
                    break;
                default:
                    Console.Clear();
                    ShowMainMenu();
                    break;
            }
        }
        /// <summary>
        /// lets all human players choose a nickname for their players.
        /// </summary>
        private void ChooseNicknames()
        {
            foreach(var player in Players)
            {
                Console.Clear();
                if (player is Human)
                {
                    Console.WriteLine("Bitte geben Sie einen Nickname für " + player.Name + " ein:");
                    player.Name = Console.ReadLine();
                }
            }
        }
        /// <summary>
        /// Gets the amount of players and creates the specified amount as human players.
        /// It adds the players to the players list.
        /// </summary>
        private void ChoosePlayerCount()
        {
            Console.Clear();
            int amount = 0;
            while(amount < 2 || amount > 31)
            {
                Console.WriteLine("Wie viele Spieler spielen mit?(2 - 31)");
                int.TryParse(Console.ReadLine(), out amount);
            }
            for(int i = 0; i < amount; i++)
            {
                Players.Add(new Human("Spieler " + (i+1)));
            }
        }
        /// <summary>
        /// Gets the difficulty for the AI.
        /// </summary>
        private void ChooseDifficulty()
        {
            Console.Clear();
            Console.WriteLine("Wähle eine Schwierigkeit für die KI:\n" +
                "1)Leicht\n" +
                "2)Schwer");
            switch (Console.ReadLine())
            {
                case "1":
                    Players.Add(new Computer("KI", new LowBrain()));
                    break;
                case "2":
                    Players.Add(new Computer("KI", brain));
                    break;
                default:
                    ChooseDifficulty();
                    break;
            }
        }
        /// <summary>
        /// Logic for a single turn.
        /// </summary>
        /// <returns>True if game is over/returns>
        private bool RunTurn()
        {
            SwitchActivePlayerToEnd();
            var stats = GetStats();
            ShowStats(stats);
            var playingCards = new List<Card>() { Active.GetTopCard() };
            playingCards[0].Show();
            return PlayerAction(playingCards,stats);
        }
        /// <summary>
        /// Gets the top card from the stack of all players.
        /// </summary>
        /// <returns>List with all cards for </returns>
        private List<Card> GetAllPlayingCards(List<Card> tmpCards, int choice)
        {
            for (int i = 0; i < Players.Count-1; i++)
                tmpCards.Add(Players[i].GetTopCard(choice));
            return tmpCards;
        }
        /// <summary>
        /// Sets the active player to the end of the playerlist.
        /// </summary>
        private void SwitchActivePlayerToEnd()
        {
            Players.Remove(Active);
            Players.Add(Active);
        }
        /// <summary>
        /// Prints the stats -> the stack count of each player to the console.
        /// </summary>
        private void ShowStats(string stats)
        {
            Console.Clear();
            Console.WriteLine("Anzahl der Karten in den Kartenstapeln:");
            Console.WriteLine(stats);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"\"{Active.Name}\" ist am Zug!");
            Console.WriteLine();
        }

        private string GetStats()
        {
            string s = "";
            foreach (APlayer a in Players)
                s += $"{a.ShowString()} | \t";
            return s;
        }
        /// <summary>
        /// Logic for the actions in a turn of the active player.
        /// </summary>
        /// <param name="tmpCards">List of all cards played</param>
        /// <returns>true if the game is over</returns>
        private bool PlayerAction(List<Card> tmpCards, string stats)
        {
            int choice = Active.ChooseValue();
            if(tmpCards.Count == 1)
            {
                var fst = tmpCards[0];
                tmpCards.RemoveAt(0);
                GetAllPlayingCards(tmpCards, choice);
                tmpCards.Add(fst);
                ShowStats(stats);
                fst.Show();
            }
            List<double> values = new List<double>();
            switch (choice)
            {
                case 1:
                    values.AddRange(tmpCards.Select(x => x.Cylinder));
                    break;
                case 2:
                    values.AddRange(tmpCards.Select(x => x.Gears));
                    break;
                case 3:
                    values.AddRange(tmpCards.Select(x => x.Ccm));
                    break;
                case 4:
                    values.AddRange(tmpCards.Select(x => x.PS));
                    break;
                case 5:
                    values.AddRange(tmpCards.Select(x => x.TopSpeed));
                    break;
                default:
                    return PlayerAction(tmpCards,stats);
            }
            var winner = DetermineWinner(values);
            if (winner.Item1)
            {
                var end = PushCardsToWinner(winner.Item2, tmpCards);
                Console.WriteLine($"\"{Active.Name}\" gewinnt die Runde!");
                Thread.Sleep(1000);
                return end;
            }
            else
            {
                Console.WriteLine("Bitte einen anderen Wert auswählen. Es gab einen Gleichstad!");
                return PlayerAction(tmpCards, stats);
            }
        }
        /// <summary>
        /// Pushes all cards played in a round to the bottom of the winning players stack.
        /// </summary>
        /// <param name="player">index of the winner in the player list</param>
        /// <param name="tmpPards">all cards played this round</param>
        /// <returns>true if game is over</returns>
        private bool PushCardsToWinner(int player, List<Card> tmpPards)
        {
            Active = Players[player];
            Active.PushCardsToBottom(tmpPards);
            if (Active.Points == cardsInTheGame)
                return false;
            return true;
        }
        /// <summary>
        /// Determines the winner of the round.
        /// </summary>
        /// <param name="values">List of selected values from all the cards</param>
        /// <returns>a tuple with (true if there is a winner, index of winner in player list)</returns>
        private (bool, int) DetermineWinner(List<double> values)
        {
            bool opt = true;
            int winner = 0;
            for (int i = 1; i < values.Count; i++)
            {
                if (values[i] > values[winner])
                {
                    winner = i;
                    opt = true;
                }
                else if (values[i] == values[winner])
                {
                    opt = false;
                }
            }
            return (opt, winner);
        }
    }
}