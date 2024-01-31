using Assignment_IceCream_Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Project
{
    abstract class IceCream //abstract class
    {
        //attributes and properties
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

        private List<Flavour> flavours;

        public List<Flavour> Flavours
        {
            get { return flavours; }
            set { flavours = value; }
        }

        private List<Topping> toppings;

        public List<Topping> Toppings
        {
            get { return toppings; }
            set { toppings = value; }
        }
        //constructors
        public IceCream() { }
        public IceCream(string o, int s, List<Flavour> f, List<Topping> t)
        {
            Option = o;
            Scoops = s;
            Flavours = f;
            Toppings = t;
        }
        //abstract class
        public abstract double CalculatePrice();
        public override string ToString()
        {
            return "Option: " + Option + "\tScoops: " + Scoops;
        }




    }
}