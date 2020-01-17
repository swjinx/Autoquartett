using AutoQuartett.Cards;
using AutoQuartett.Player.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoQuartett.Player
{
    public class Human : APlayer
    {
        public Human(string n) : base(n)
        {
        }

        public override Card GetTopCard(int choice = 0) => Points < 4 && Points > 1 ? ChooseTopCard() : base.GetTopCard();
        public override int ChooseValue()
        {
            Console.WriteLine("Bitte geben sie die Zahl des ausgewählten Wertes ein");
            int.TryParse(Console.ReadLine(), out int value);
            return value;
        }
        /// <summary>
        /// Logic for the last three cards on the stack.
        /// Let's the player choose which card to play.
        /// </summary>
        /// <returns>The chosen card</returns>
        private Card ChooseTopCard()
        {
            Console.Clear();
            var stackCards = GetAllCardsFromStack();
            var choice = GetCardChoice(stackCards.Count);
            var card = stackCards[choice];
            stackCards.Remove(card);
            PushCardsToBottom(stackCards);
            Console.Clear();
            return card;
        }
        /// <summary>
        /// Get's all the cards from the stack as a list and displays them on the console.
        /// </summary>
        /// <returns>List of all the cards from the stack</returns>
        private List<Card> GetAllCardsFromStack()
        {
            int p = Points;
            List<Card> stackCards = DrawingPile.ToList();
            DrawingPile.Clear();
            for (int i = 0; i < p; i++)
            {
                var tmpCard = stackCards[i];
                Console.WriteLine((i + 1) + ")");
                tmpCard.Show();
                Console.WriteLine();
            }
            return stackCards;
        }
        /// <summary>
        /// Gets the players choice on which card to pick.
        /// </summary>
        /// <param name="count">count of how many cards to choose from</param>
        /// <returns>the result of the players choice</returns>
        private int GetCardChoice(int count)
        {
            int choice = 0;
            while (choice < 1 || choice > count)
            {
                Console.WriteLine("\"" + Name + "\" " + "wähle die Karte aus die gespielt werden soll:");
                int.TryParse(Console.ReadLine(), out choice);
            }
            choice--;
            return choice;
        }
    }
}
