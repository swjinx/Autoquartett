using AutoQuartett.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoQuartett.ArtificialInteligence.Base
{
    public interface IBrain
    {
        /// <summary>
        /// Chooses the next value to pick from a card.
        /// </summary>
        /// <param name="again">determins if its the same card as before</param>
        /// <param name="card">the card to choose the value from</param>
        /// <returns>the coice as int</returns>
        int Choose(bool again, Card card = null);
        /// <summary>
        /// Chooses the card with the maximum value for the choice.
        /// </summary>
        /// <param name="choice">choice</param>
        /// <param name="tmpCards">List of cards to choose from</param>
        /// <returns>the chosen card</returns>
        Card ChooseCardWithMaxValue(int choice, List<Card> tmpCards);
    }
}
