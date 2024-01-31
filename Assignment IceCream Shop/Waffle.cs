using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment_IceCream_Shop;

namespace PRG2_Final_Project
{
    class Waffle : IceCream //Cone class inherit method from IceCream class
    {
        //Attributes and Properties
        private string waffleFlavour;

        public string WaffleFlavour
        {
            get { return waffleFlavour; }
            set { waffleFlavour = value; }
        }

        //Constructors
        public Waffle() { }

        public Waffle(string o, int s, List<Flavour> f, List<Topping> t, string w) : base(o, s, f, t)
        {
            WaffleFlavour = w;
        }

        //Method inherited from IceCream 
        public override double CalculatePrice()
        {
            double price = 0;
            //Numbers of ice cream scoops
            if (Scoops == 1)
            {
                price = 7;
            }
            else if (Scoops == 2)
            {
                price = 8.5;
            }
            else if (Scoops == 3)
            {
                price = 9.5;
            }

            //Number of toppings
            price += Toppings.Count * 1;

            //Flavour of ice cream chosen
            for (int i = 0; i < Flavours.Count; i++)
            {
                if (Flavours[i].Premium == true)
                {
                    price += 2;
                }
            }

            //Whether the waffle flavour the customer chosen is original or premium
            if (WaffleFlavour == "Red Velvet" || WaffleFlavour == "Charcoal" || WaffleFlavour == "Pandan")
            {
                price += 3;
            }

            return price;

        }

        public override string ToString()
        {
            return base.ToString() + "Waffle Flavour: " + WaffleFlavour;
        }
    }
}
