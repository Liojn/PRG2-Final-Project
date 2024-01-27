using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Project
{
    class PointCard
    {
        private int points;
        private int punchCard;
        private string tier;

        public int Points { get { return points; } set { points = value; } }
        public int PunchCards { get { return punchCard; } set { punchCard = value; } }
        public string Tier { get { return tier; } set { tier = value; } }

        public PointCard() { }

        public PointCard(int p, int pc)
        {
            Points = p;
            PunchCards = pc;
        }

        public void AddPoints(int points)
        {
            Points += points;
            if (Tier != "Gold")
            {
                if (Points >= 100)
                {
                    Tier = "Gold";
                }
                else if (Points >= 50)
                {
                    Tier = "Silver";
                }
            }
        }

        public void ReedemPoints(int reedemPoint)
        {
            while (true)
            {
                if (Points < reedemPoint)
                {
                    Console.WriteLine("Points insufficient.");
                }
                else
                {
                    Points -= reedemPoint;
                }
            }

        }

        public void Punch()
        {
            if (PunchCards <= 10)
            {
                PunchCards++;
            }
            else
            {
                PunchCards = 0;
            }
        }

        public override string ToString()
        {
            return ("Points: " + Points + "\tPunch Card: " + punchCard + "\tTier: " + Tier);
        }


    }
}
