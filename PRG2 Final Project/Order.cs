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
            try
            {
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
                    Console.WriteLine("[{}]: {}", PossibleChoices.Count(), "Cone Dip");
                }
                else if (modIceCream is Waffle)
                {
                    Console.WriteLine("[{}]: {}", PossibleChoices.Count(), "Waffle Flavor");
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Ice Cream dosen't exist");
            }
            finally
            {
                Console.Write("What do you want to modify?: ");
                int option = Convert.ToInt32(Console.ReadLine());

                if (option == 1)
                {

                }

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
