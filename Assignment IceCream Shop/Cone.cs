using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_IceCream_Shop
{
    class Cone : IceCream //Cone class inherit method from IceCream class
    {
        //Attributes and Properties
		private bool dipped;

		public bool Dipped
		{
			get { return dipped; }
			set { dipped = value; }
		}

        //Constructors
		public Cone() { }
		public Cone(string o, int s, List<Flavour> f, List<Topping> t, bool d) : base (o, s, f, t)
		{
			Dipped = d;
		}

        //Method inherited from IceCream 
        public override double CalculatePrice()
        {
            double price = 0; 
            //Numbers of ice cream scoops 
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

            //Numbers of toppings
            price += Toppings.Count * 1;

            //Flavours of ice cream chosen
            for (int i = 0; i < Flavours.Count; i++)
            {
                if (Flavours[i].Premium == true)
                {
                    price += 2;
                }
            }

            //Whether the customer choose a chocolate dipped cone
            if (Dipped == true)
            {
                price += 2;
            }

            return price;

        }
        public override string ToString()
        {
            return base.ToString() + "Dipped: " + Dipped;
        }


    }
}
