using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Project
{
    class Order
    {
        private int id;
        private DateTime timeReceived;



        public int Id { get { return id; } set { id = value; } }
        public DateTime TimeReceived { get { return timeReceived; } set { timeReceived = value; } }
        public DateTime? TimeFulfilled { get; set; }

        public List<IceCream> iceCreamList { get; set; } = new List<IceCream>();

        public Order() { }

        public Order(int i, DateTime TR)
        {
            Id = i;
            TimeReceived = TR;
            TimeFulfilled = null;
        }

        public void ModifyIceCream(int i)
        {
            string[] PossibleChoices = { "Option", "Scoops", "Flavors","Toppings" };
            IceCream modIceCream = iceCreamList[i - 1];
            if (iceCreamList.Count <= 0)
            {
                Console.WriteLine("No Ice Cream in the order.");
                return;
            }
            for (i = 0; i < PossibleChoices.Count(); i++)
            {
                Console.Write("[{}]: {}", i + 1, PossibleChoices[i]);
            }
            if (modIceCream is Cone)
            {
                Console.WriteLine("[{}]: {}", PossibleChoices.Count() + 1, "Cone Dip");
            }
            else if (modIceCream is Waffle)
            {
                Console.WriteLine("[{}]: {}", PossibleChoices.Count() + 1, "Waffle Flavor");
            }

            Console.Write("What do you want to modify?: ");
            int option = Convert.ToInt32(Console.ReadLine());

            switch(option)
            {
                case 1:
                    if (modIceCream is Cup)
                    {
                        Console.WriteLine("Current Option: {}","Cup");
                        Console.WriteLine("[1]Switch to {}.", "Cone");
                        Console.WriteLine("[2]Switch to {}", "Wafle");
                        Console.WriteLine("[0]Exit.");
                        int swap = Convert.ToInt32(Console.ReadLine());
                        switch (swap)
                        {
                            case 0:
                                return;
                            case 1:
                                modIceCream = (Cone)modIceCream;
                                break;
                            case 2:
                                modIceCream = (Waffle)modIceCream;
                                break;
                        }
                    }
                    else if (modIceCream is Cone)
                    {
                        Console.WriteLine("Current Option: {}", "Come");
                        Console.WriteLine("[1]Switch to {}.", "Cup");
                        Console.WriteLine("[2]Switch to {}", "Wafle");
                        int swap = Convert.ToInt32(Console.ReadLine());
                        switch (swap)
                        {
                            case 0:
                                return;
                            case 1:
                                modIceCream = (Cup)modIceCream;
                                break;
                            case 2:
                                modIceCream = (Waffle)modIceCream;
                                break;
                        }
                    }
                    else if (modIceCream is Waffle)
                    {
                        Console.WriteLine("Current Option: {}", "Waffle");
                        Console.WriteLine("[1]Switch to {}.", "Cup");
                        Console.WriteLine("[2]Switch to {}", "Cone");
                        int swap = Convert.ToInt32(Console.ReadLine());
                        switch (swap)
                        {
                            case 0:
                                return;
                            case 1:
                                modIceCream = (Cup)modIceCream;
                                break;
                            case 2:
                                modIceCream = (Cone)modIceCream;
                                break;
                        }
                    }
                    iceCreamList[i - 1] = modIceCream;
                    break;


                case 2:
                    break;
            }
        }

        public void AddIceCream(IceCream iceCream)
        {
            iceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int index)
        {
            iceCreamList.RemoveAt(index - 1);
        }

        public double CalculateTotal()
        {
            double mostExpensive = 0;
            double TotalPrice = 0;
            foreach (IceCream ice in iceCreamList)
            {
                if (ice.CalculatePrice() > mostExpensive)
                {
                    mostExpensive = ice.CalculatePrice();
                }
                double price = ice.CalculatePrice();
                TotalPrice += price;
            }
            return TotalPrice;
        }

        public override string ToString()
        {
            string icecreams = "";
            foreach (IceCream ice in iceCreamList)
            {
                icecreams += ice.ToString() +"\n";
            }

            return ("Id: " + Id + "\tTime Received: " + TimeReceived + "\tTime Fulfilled: " + TimeFulfilled + "\nIce Creams: " + icecreams);
        }


    }
}
