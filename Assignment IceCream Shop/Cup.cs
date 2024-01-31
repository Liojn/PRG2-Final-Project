using Assignment_IceCream_Shop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PRG2_Final_Project
{
    class Cup : IceCream
    {
        public Cup() { }
        public Cup(string o, int s, List<Flavour> f, List<Topping> t) : base(o, s, f, t) { }

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
