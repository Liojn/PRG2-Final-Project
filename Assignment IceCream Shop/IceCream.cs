using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_IceCream_Shop
{
    abstract class IceCream
    {
		//Attributes and Properties
		private string option;

		public string Option
		{
			get { return option; }
			set { option = value; }
		}

		private int scoops;

		public int Scoops
		{
			get { return scoops; }
			set { scoops = value; }
		}

		//one to many multiplicity from IceCream to Flavour class
		private List<Flavour> flavours;

		public List<Flavour> Flavours
		{
			get { return flavours; }
			set { flavours = value; }
		}

		//one to many multiplicity from IceCream to Topping class
		private List<Topping> toppings;

		public List<Topping> Toppings
		{
			get { return toppings; }
			set { toppings = value; }
		}

		//Constructors
		public IceCream() { }
		public IceCream(string o, int s, List<Flavour> f, List<Topping> t)
		{
			Option = o;
			Scoops = s;
			Flavours = f;
			Toppings = t;
		}

		//Abstract method CalculatePrice()
		public abstract double CalculatePrice();
        public override string ToString()
        {
			return "Option: " + Option + "Scoops: " + Scoops;
        }




    }
}
