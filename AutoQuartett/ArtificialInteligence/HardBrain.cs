using AutoQuartett.ArtificialInteligence.Base;
using AutoQuartett.Cards;
using System.Collections.Generic;
using System.Linq;

namespace AutoQuartett.ArtificialInteligence
{
    public class HardBrain : IBrain
    {
        public double MaxZylinder { get; private set; }
        public double MaxGears { get; private set; }
        public double MaxCcm { get; private set; }
        public double MaxPs { get; private set; }
        public double MaxTopSpeed { get; private set; }

        private double[] prevValues;
        private int cardCount = 0;
        /// <summary>
        /// derive all properties through the amount of recieved cards
        /// </summary>
        public void DeriveAllValues()
        {
            MaxZylinder /= cardCount;
            MaxGears /= cardCount;
            MaxCcm /= cardCount;
            MaxPs /= cardCount;
            MaxTopSpeed /= cardCount;
        }
        /// <summary>
        /// Add all values from the parameter to the properties
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card)
        {
            MaxZylinder += card.Cylinder;
            MaxGears += card.Gears;
            MaxCcm += card.Ccm;
            MaxPs += card.PS;
            MaxTopSpeed += card.TopSpeed;
            cardCount++;
        }

        public int Choose(bool again, Card card = null)
        {
            double[] values;
            if (!again)
            {
                values = new double[5];
                values[0] = card.Cylinder / MaxZylinder;
                values[1] = card.Gears / MaxGears;
                values[2] = card.Ccm / MaxCcm;
                values[3] = card.PS / MaxPs;
                values[4] = card.TopSpeed / MaxTopSpeed;
            }
            else
                values = prevValues;
            int res = 0;
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > values[res])
                    res = i;
            }
            values[res] = 0;
            prevValues = values;
            return res + 1;
        }
        public Card ChooseCardWithMaxValue(int choice, List<Card> tmpCards)
        {
            Card card;
            switch (choice)
            {
                case 1:
                    card = tmpCards.First(x => x.Cylinder == tmpCards.Max(y => y.Cylinder));
                    break;
                case 2:
                    card = tmpCards.First(x => x.Gears == tmpCards.Max(y => y.Gears));
                    break;
                case 3:
                    card = tmpCards.First(x => x.Ccm == tmpCards.Max(y => y.Ccm));
                    break;
                case 4:
                    card = tmpCards.First(x => x.PS == tmpCards.Max(y => y.PS));
                    break;
                case 5:
                    card = tmpCards.First(x => x.TopSpeed == tmpCards.Max(y => y.TopSpeed));
                    break;
                default:
                    return null;
            }
            tmpCards.Remove(card);
            return card;
        }
    }
}
