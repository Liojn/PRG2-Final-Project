using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Project
{
    class Flavour
    {
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private bool premium;

        public bool Premium
        {
            get { return premium; }
            set { premium = value; }
        }

        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public Flavour() { }
        public Flavour(string t, bool p, int q)
        {
            Type = t;
            Premium = p;
            Quantity = q;
        }

        public override string ToString()
        {
            return "Type: " + Type + "\tPremium: " + Premium + "\tQuantity: " + Quantity;
        }


    }
}
