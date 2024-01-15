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

        private IceCream ModifyOption(IceCream modIceCream)
        {
            Console.WriteLine("Current Option: {}",modIceCream.GetType());
            Console.WriteLine("[1]Switch to Cup.");
            Console.WriteLine("[2]Switch to Cone.");
            Console.WriteLine("[3]Switch to Waffle.");
            Console.WriteLine("[0]Exit.");

            int swap = Convert.ToInt32(Console.ReadLine());

            switch (swap)
            {
                case 0:
                    break;
                case 1 when modIceCream is not Cup:
                    modIceCream = new Cup();
                    break;
                case 2 when modIceCream is not Cone:
                    modIceCream = new Cone();
                    break;
                case 3 when modIceCream is not Waffle:
                    modIceCream = new Waffle();
                    break;
            }

            return modIceCream;
        
        }

        private IceCream ModifyScoops(IceCream modIceCream)
        {
            Console.WriteLine("Current Scoop: {}", modIceCream.Scoops);
            Console.WriteLine("Choose number of Scoops (1 , 2 , or 3) :");
            int NewScoops = Convert.ToInt32(Console.ReadLine());
            modIceCream.Scoops = NewScoops;
            return modIceCream;
        }

        private IceCream ModifyFlavour(IceCream modIceCream)
        {

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

            for (int x = 0; x < PossibleChoices.Count(); x++)
            {
                Console.Write("[{}]: {}", x + 1, PossibleChoices[i]);
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
            switch (option)
            {
                case 1:
                    modIceCream = ModifyOption(modIceCream);
                    break;
                case 2:
                    modIceCream = ModifyScoops(modIceCream);
                    break;
                case 3:
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
