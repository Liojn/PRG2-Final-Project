using Assignment_IceCream_Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PRG2_Final_Project
{

    class Cone : IceCream //inherit from ice cream class
    {
        //attributes and properties
        private bool dipped;

        public bool Dipped
        {
            get { return dipped; }
            set { dipped = value; }
        }

        //Constructors
        public Cone() { }
        public Cone(string o, int s, List<Flavour> f, List<Topping> t, bool d) : base(o, s, f, t)
        {
            Dipped = d;
        }
        //calculate price method to calculate the price of the ice cream
        public override double CalculatePrice()
        {
            double price = 0;
            if (Scoops == 1)//1 scoop
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
            price += Toppings.Count * 1; //adding in toppings price

            for (int i = 0; i < Flavours.Count; i++)
            {
                if (Flavours[i].Premium == true) //if the flavour is premium
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

            if (Dipped == true) //if chocolate dipped cone is chosen
            {
                price += 2;
            }

            return price;

        }

        public override string ToString()
        {
            return base.ToString() + "\tDipped: " + Dipped;
        }


    }
}
