using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Project
{
    class Waffle : IceCream
    {
        private string waffleFlavour;

        public string WaffleFlavour
        {
            get { return waffleFlavour; }
            set { waffleFlavour = value; }
        }

        public Waffle() { }

        public Waffle(string o, int s, List<Flavour> f, List<Topping> t, string w) : base(o, s, f, t)
        {
            WaffleFlavour = w;
        }

        public override double CalculatePrice()
        {
            double price = 0;
            if (Scoops == 1)
            {
                price = 4;
            }
            else if (Scoops == 2)
            {
                price = 5.5;
            }
            else if (Scoops == 3)
            {
                price = 6.50;
            }

            price += Toppings.Count * 1;

            for (int i = 0; i < Flavours.Count; i++)
            {
                if (Flavours[i].Premium == true)
                {
                    price += 2;
                }
            }

            if (WaffleFlavour == "Durian" || WaffleFlavour == "Ube" || WaffleFlavour == "Sea salt")
            {
                price += 2;
            }

            return price;

        }

        public override string ToString()
        {
            return base.ToString() + "\tWaffle Flavour: " + WaffleFlavour;
        }
    }
}
