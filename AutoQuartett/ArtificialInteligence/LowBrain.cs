using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoQuartett.ArtificialInteligence.Base;
using AutoQuartett.Cards;

namespace AutoQuartett.ArtificialInteligence
{
    class LowBrain : IBrain
    {
        private readonly Random rand;
        public LowBrain()
        {
            rand = new Random();
        }
        public int Choose(bool again, Card card = null) =>
            rand.Next(1, 6);

        public Card ChooseCardWithMaxValue(int choice, List<Card> tmpCards) =>
            tmpCards.First();
    }
}
