using System;
using System.Linq;
using System.Threading;
using AutoQuartett.ArtificialInteligence.Base;
using AutoQuartett.Cards;
using AutoQuartett.Player.Base;

namespace AutoQuartett.Player
{
    public class Computer : APlayer
    {
        private Card played;
        private Card prevCard;
        private readonly IBrain brain;
        public Computer(string n, IBrain b) : base(n)
        {
            brain = b;
        }

        public override Card GetTopCard(int choice = 0)
        {
            Card c;
            if (Points < 4 && Points > 1 && choice > 0)
                c = ChooseCard(choice);
            else
                c = base.GetTopCard(choice);
            played = c;
            prevCard = null;
            return c;
        }

        public override int ChooseValue()
        {
            int choice;
            if (prevCard == played)
                choice = brain.Choose(true);
            else
            {
                choice = brain.Choose(false, played);
                prevCard = played;
            }
            Console.WriteLine("Wahl der KI:" + choice);
            Thread.Sleep(1500);
            return choice;
        }
        /// <summary>
        /// chooses the card best suited to battle with the given choice
        /// </summary>
        /// <param name="choice">the choice</param>
        /// <returns>the chosen card</returns>
        private Card ChooseCard(int choice)
        {
            var cardPile = DrawingPile.ToList();
            var c = brain.ChooseCardWithMaxValue(choice, cardPile);
            DrawingPile.Clear();
            cardPile.ForEach(x => DrawingPile.Enqueue(x));
            return c;
        }
    }
}
