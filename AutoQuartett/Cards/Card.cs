using System;

namespace AutoQuartett.Cards
{
    public class Card
    { 
        public string Name { get; private set; }
        public double Cylinder { get; private set; }
        public double Gears { get; private set; }
        public double Ccm { get; private set; }
        public double PS { get; private set; }
        public double TopSpeed { get; private set; }
        
        public Card(string name, int cylinder, int gears, double ccm, int ps, int topspeed)
        {
            Name = name;
            Cylinder = cylinder;
            Gears = gears;
            Ccm = ccm;
            PS = ps;
            TopSpeed = topspeed;
        }
        /// <summary>
        /// Prints the card to the Console.
        /// </summary>
        public void Show()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("| | " + Name);
            Console.WriteLine("----------------------------");
            Console.WriteLine("|1|Zylinder: " + Cylinder);
            Console.WriteLine("|2|Gänge: " + Gears);
            Console.WriteLine("|3|CCM: " + Ccm + " cm³");
            Console.WriteLine("|4|Ps: " + PS + " PS");
            Console.WriteLine("|5|Top Speed: " + TopSpeed + " km/h");
            Console.WriteLine("----------------------------");
        }
    }
}
