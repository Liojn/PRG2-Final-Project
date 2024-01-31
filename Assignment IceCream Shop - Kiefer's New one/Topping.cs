using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Project
{
    //attributes and properties
    class Topping
    {
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        //constructors
        public Topping() { }
        public Topping(string t)
        {
            Type = t;
        }

        public override string ToString()
        {
            return "Type: " + Type;
        }

    }
}
