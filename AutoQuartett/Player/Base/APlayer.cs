using AutoQuartett.Cards;
using System.Collections.Generic;

namespace AutoQuartett.Player.Base
{
    public abstract class APlayer
    {
        public Queue<Card> DrawingPile { get; set; }
        public int Points { get => DrawingPile.Count; }
        public string Name { get; set; }

        public APlayer(string n)
        {
            Name = n;
            DrawingPile = new Queue<Card>();
        }
        /// <summary>
        /// Logic for choosing the value of the card played.
        /// </summary>
        /// <returns>the coice as int</returns>
        public abstract int ChooseValue();
        /// <summary>
        /// Gets the top card from the drawing pile.
        /// </summary>
        /// <returns>Card</returns>
        public virtual Card GetTopCard(int choice = 0) => 
            DrawingPile.Dequeue();
        /// <summary>
        /// Pushes cards to the bottom of the drawing pile.
        /// </summary>
        /// <param name="karten">The cards to push to the stack</param>
        public void PushCardsToBottom(List<Card> karten) => 
            karten.ForEach(x => DrawingPile.Enqueue(x));
        /// <summary>
        /// Gets the string to display the player on the console.
        /// </summary>
        /// <returns></returns>
        public string ShowString() => 
            Name + " " + Points;
    }
}
