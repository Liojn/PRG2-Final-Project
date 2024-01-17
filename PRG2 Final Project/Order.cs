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
                    modIceCream = (Cup)modIceCream;
                    break;
                case 2 when modIceCream is not Cone:
                    modIceCream = (Cone)modIceCream;
                    break;
                case 3 when modIceCream is not Waffle:
                    modIceCream = (Waffle)modIceCream;
                    break;
            }

            return modIceCream;
        
        }

        private IceCream ModifyScoops(IceCream modIceCream)
        {
            string[] RegularFlavours = { "Vanilla", "Chocolate", "Strawberry" };
            string[] PremiumFlavours = { "Durian", "Ube", "Sea salt" };
        
            while (true)
            {
                Console.WriteLine("Current Scoop: {}", modIceCream.Scoops);
                Console.WriteLine("[1]Remove Scoop");
                Console.WriteLine("[2]Add Scoop");
                Console.WriteLine("[0]Exit.");
                Console.WriteLine("Enter your option: ");
                int Scoops = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
                switch (Scoops)
                {
                    case 0:
                        return modIceCream;
                    case 1:
                        if (Scoops == 0)
                        {
                            Console.WriteLine("Cannot Remove Scoops");
                            return modIceCream;
                        }
                        Console.WriteLine("Your flavour(s): ");
                        foreach (Flavour f in modIceCream.Flavours)
                        {
                            Console.WriteLine("-{}", f);
                        }
                        Console.WriteLine("Enter the flavour: ");
                        string RemoveF = Console.ReadLine();
                        for (int j = 0; j < modIceCream.Flavours.Count(); j++)
                        {
                            if (modIceCream.Flavours[j].Type.ToLower() == RemoveF.ToLower())
                            {
                                if (modIceCream.Flavours[j].Quantity == 0)
                                {
                                    modIceCream.Flavours.RemoveAt(j);
                                    break;
                                }
                                else
                                {
                                    modIceCream.Flavours[j].Quantity -= 1;
                                    break;
                                }

                            }
                        }
                        return modIceCream;
                    case 2:
                        bool premium = false;
                        if (Scoops == 3)
                        {
                            Console.WriteLine("Cannot Add Scoops");
                            return modIceCream;
                        }
                        Console.WriteLine("Regular Flavour List: ");
                        foreach (string x in RegularFlavours)
                        {
                            Console.WriteLine("-{}", (x));
                        }
                        Console.WriteLine("Premium Flavour List: ");
                        foreach (string x in PremiumFlavours)
                        {
                            Console.WriteLine("-{}", (x));
                        }
                        Console.WriteLine("What flavour do you want the new scoop to be?: ");
                        string Chosen = Console.ReadLine();
                        Chosen.ToLower();
                        foreach (string x in PremiumFlavours)
                        {
                            if (x.ToLower() == Chosen)
                            {
                                premium = true;
                                break;
                            }
                        }
                        foreach (Flavour x in modIceCream.Flavours)
                        {
                            if (x.Type.ToLower() == Chosen.ToLower())
                            {
                                x.Quantity += 1;
                                return modIceCream;
                            }     
                            else
                            {
                                Flavour newFlavour = new Flavour(Chosen, premium, 1);
                                modIceCream.Flavours.Add(newFlavour);
                                return modIceCream;
                            }
                        }
                        return modIceCream;
                }

            };
        }

        private IceCream ModifyFlavour(IceCream modIceCream)
        {
            bool premium = false;
            string[] RegularFlavours = { "Vanilla", "Chocolate", "Strawberry" };
            string[] PremiumFlavours = { "Durian", "Ube", "Sea salt" };
            Console.WriteLine("[0]Exit.");
            Console.WriteLine("[1]Change flavour.");
            Console.WriteLine("Enter your option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            switch (option)
            {
                case 0:
                    return modIceCream;
                case 1:
                    Console.WriteLine("Regular Flavour List: ");
                    foreach (string x in RegularFlavours)
                    {
                        Console.WriteLine("-{}", (x));
                    }
                    Console.WriteLine("Premium Flavour List: ");
                    foreach (string x in PremiumFlavours)
                    {
                        Console.WriteLine("-{}", (x));
                    }
                    Console.WriteLine("What flavour do you want?: ");
                    string Chosen = Console.ReadLine();
                    Console.WriteLine();
                    Console.WriteLine("Flavours you currently have: ");
                    foreach (Flavour flavour in modIceCream.Flavours)
                    {
                        Console.WriteLine("-{}", flavour);
                    }
                    Console.WriteLine("Which flavour to change?: ");
                    string Change = Console.ReadLine().ToLower();
                    foreach (string x in PremiumFlavours)
                    {
                        if (x.ToLower() == Chosen.ToLower())
                        {
                            premium = true;
                            break;
                        }
                    }

                    for (int j = 0; j < modIceCream.Flavours.Count(); j++)
                    {
                        if (modIceCream.Flavours[j].Type.ToLower() == Change.ToLower())
                        {
                            int quanity = modIceCream.Flavours[j].Quantity;
                            modIceCream.Flavours[j] = new Flavour(Chosen, premium, quanity);
                            return modIceCream;
                        }
                    }
                    break;
            }
            return modIceCream;

        }

        private void ModifyToppings(IceCream moodIceCream)
        {
            string[] toppings = { "Sprinkles", "Mochi", "Sago", "Oreos" };
            Console.WriteLine("Your Toppings: ");
            
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
                    modIceCream = ModifyFlavour(modIceCream);
                    break;
                case 4:
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
