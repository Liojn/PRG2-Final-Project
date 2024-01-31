using Assignment_IceCream_Shop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PRG2_Final_Project
{
    class Cup : IceCream //inherit from ice cream class
    {
        //constructors
        public Cup() { }
        public Cup(string o, int s, List<Flavour> f, List<Topping> t) : base(o, s, f, t) { }

        //overriding the calculate price method to calculate price of ice cream
        public override double CalculatePrice()
        {
            double price = 0;
            if (Scoops == 1) //1 scoop
            {
                price = 4;
            }
            else if (Scoops == 2) //2 scoops
            {
                price = 5.5;
            }
            else if (Scoops == 3) //3 scoops
            {
                price = 6.50;
            }
            price += Toppings.Count * 1; //adding in topping price

            for (int i = 0; i < Flavours.Count; i++)
            {
                if (Flavours[i].Premium == true) //if flavour is premiium
                {
                    if (Flavours[i].Quantity > 1)
                    {
                        price += 2 * Flavours[i].Quantity;
                    }
                    else
                    {
                        price += 2;
                    }
                }
            }
            return price;

        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
