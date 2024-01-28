using Assignment_IceCream_Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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




        private Dictionary<string, int> InitaliseFlavour()
        {
            Dictionary<string, int> FlavoursFile = new Dictionary<string, int>();
            string line;
            int count = 0;
            using StreamReader sr = new StreamReader("flavours.csv");
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (count == 0)
                    {
                        count++;
                    }
                    else
                    {
                        string[] data = line.Split(",");
                        FlavoursFile.Add(data[0], Convert.ToInt32(data[1]));
                    }
                }
            }
            return FlavoursFile;
        }

        private List<string> InitaliseToppings()
        {
            List<string> ToppingsFile = new List<string>();
            string line;
            int count = 0;
            using StreamReader sr = new StreamReader("toppings.csv");
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (count == 0)
                    {
                        count++;
                    }
                    else
                    {
                        string[] data = line.Split(",");
                        ToppingsFile.Add(data[0]);
                    }
                }
            };
            return ToppingsFile;
        }

        private IceCream ModifyOption(IceCream modIceCream)
        {
            string[] waf = { "Original", "Red velvet", "Charcoal", "Pandan" };
           
            while (true)
            {
                try
                {
                    Console.WriteLine("Current Option: {0}", modIceCream.Option);
                    Console.WriteLine("[1]Switch to Cup.");
                    Console.WriteLine("[2]Switch to Cone.");
                    Console.WriteLine("[3]Switch to Waffle.");
                    Console.WriteLine("[0]Exit.");
                    Console.Write("Enter your option: ");
                    int swap = Convert.ToInt32(Console.ReadLine());
                    switch (swap)
                    {
                        case 0:
                            break;
                        case 1 when modIceCream is not Cup:
                            Cup CupIceCream = new Cup("Cup", modIceCream.Scoops, modIceCream.Flavours, modIceCream.Toppings);
                            Console.WriteLine("Option changed to Cup!");
                            return CupIceCream;
                        case 2 when modIceCream is not Cone:
                            bool dipped = false;
                            while (true)
                            {
                                Console.Write("Dipped? [Y/N]: ");
                                string dip = Console.ReadLine();
                                if (dip.ToLower() == "y")
                                {
                                    dipped = true;
                                    break;
                                }
                                else if (dip.ToLower() == "n")
                                {
                                    break;
                                } 
                                else
                                {
                                    Console.WriteLine("Enter Y/N.\n");
                                }
                            }
                        
                            Cone ConeIceCream = new Cone("Cone", modIceCream.Scoops, modIceCream.Flavours, modIceCream.Toppings, dipped);
                            Console.WriteLine("Option changed to Cone!");
                            return ConeIceCream;
                        case 3 when modIceCream is not Waffle:
                            string[] WFlavours = { "Original", "Red velvet", "Charcoal", "Pandan" };
                            Console.WriteLine("Available Flavours: ");
                            foreach (string s in WFlavours)
                            {
                                Console.Write("{0,-15}", s);
                            }
                            Console.WriteLine();
                            Console.Write("What flavour of waffle?: ");
                            string wFlav = Console.ReadLine();
                            foreach (string s in WFlavours)
                            {
                                if (s.ToLower() == wFlav.ToLower())
                                {
                                    wFlav = s; break;
                                }
                            }
                            Waffle WafIceCream = new Waffle("Waffle", modIceCream.Scoops, modIceCream.Flavours, modIceCream.Toppings, wFlav);
                            Console.WriteLine("Option changed to Waffle!");
                            return WafIceCream;
                        default:
                            Console.WriteLine("Enter a valid option.\n");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter a valid number");
                }
            }

            return modIceCream;

        }

        private IceCream ModifyScoops(IceCream modIceCream)
        {
            Dictionary<string, int> Flavours = InitaliseFlavour();
            int Scoops = -1;

            while (true)
            {
                try
                {
                    Console.WriteLine($"Current Scoop: {modIceCream.Scoops}");
                    Console.WriteLine("[1] Remove Scoop");
                    Console.WriteLine("[2] Add Scoop");
                    Console.WriteLine("[0] Exit.");
                    Console.Write("Enter your option: ");
                    Scoops = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    switch (Scoops)
                    {
                        case 0:
                            return modIceCream;

                        case 1:
                            while (true)
                            {
                                if (modIceCream.Scoops == 1)
                                {
                                    Console.WriteLine("Cannot Remove Scoops");
                                    break;
                                }

                                Console.WriteLine("Your flavour(s): ");
                                foreach (Flavour f in modIceCream.Flavours)
                                {
                                    string Premium = "No";
                                    string s = "";
                                    s += "Flavour: " + f.Type + "\tPremium: " + Premium + "\tQuantity: " + f.Quantity;
                                    if (f.Premium)
                                    {
                                        Premium = "Yes";
                                    }
                                    Console.WriteLine($"-{s}");
                                }

                                try
                                {
                                    Console.Write("Enter the flavour: ");
                                    string RemoveF = Console.ReadLine();
                                    bool found = false;

                                    for (int j = 0; j < modIceCream.Flavours.Count(); j++)
                                    {
                                        if (modIceCream.Flavours[j].Type.ToLower() == RemoveF.ToLower())
                                        {
                                            if (modIceCream.Flavours[j].Quantity == 1)
                                            {
                                                Console.WriteLine("Flavour removed {0}", modIceCream.Flavours[j].Type);
                                                modIceCream.Scoops--;
                                                modIceCream.Flavours.RemoveAt(j);
                                                found = true;
                                                break;
                                            }
                                            else
                                            {
                                                modIceCream.Scoops--;
                                                modIceCream.Flavours[j].Quantity -= 1;
                                                found = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!found)
                                    {
                                        Console.WriteLine("Flavour not found. Enter a valid flavour.");
                                        continue;
                                    }
                                    Console.WriteLine("Successfully Removed!");
                                    return modIceCream;
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Enter a valid flavour");
                                }
                            }
                            return modIceCream;

                        case 2:
                            while (true)
                            {
                                try
                                {
                                    if (modIceCream.Scoops == 3)
                                    {
                                        Console.WriteLine("Cannot Add Scoops");
                                        break;
                                    }
                                    modIceCream.Scoops += 1;
                                    bool premium = false;
                                    Console.WriteLine("Flavour List: ");
                                    foreach (KeyValuePair<string, int> kvp in Flavours)
                                    {
                                        string y = kvp.Key;
                                        if (kvp.Value != 0)
                                        {
                                            y += "(Premium)";
                                        }
                                        Console.WriteLine($"-{y}");
                                    }

                                    Console.Write("What flavour do you want the new scoop to be?: ");
                                    string Chosen = Console.ReadLine();

                                    foreach (KeyValuePair<string, int> kvp in Flavours)
                                    {
                                        if (Chosen.ToLower() == kvp.Key.ToLower())
                                        {
                                            Chosen = kvp.Key;
                                            if (kvp.Value != 0)
                                            {
                                                premium = true;
                                                break;

                                            }
                                        }

                                    }

                                    bool existingFlavour = false;

                                    foreach (Flavour x in modIceCream.Flavours)
                                    {
                                        if (x.Type.ToLower() == Chosen.ToLower())
                                        {
                                            x.Quantity += 1;
                                            existingFlavour = true;
                                            Console.WriteLine("Quantity Increased!");
                                            break;
                                        }
                                    }

                                    if (!existingFlavour)
                                    {
                                        Flavour newFlavour = new Flavour(Chosen, premium, 1);
                                        modIceCream.Flavours.Add(newFlavour);
                                        Console.WriteLine("New Flavour Added!");
                                    }
                                    return modIceCream;
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Give a valid flavour");
                                }
                            }
                            Console.WriteLine("Successfully Added!");
                            return modIceCream;

                        default:
                            Console.WriteLine("Give a valid option.");
                            break;
                    }
                }

                catch (FormatException)
                {
                    Console.WriteLine("Give a valid number");
                }

                return modIceCream;
            }
        }


        private IceCream ModifyFlavour(IceCream modIceCream)
        {
            Dictionary<string, int> Flavours = InitaliseFlavour();
            bool premium = false;
            while (true)
            {
                try
                {
                    Console.WriteLine("[0]Exit.");
                    Console.WriteLine("[1]Change flavour.");
                    Console.Write("Enter your option: ");
                    int option = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    switch (option)
                    {
                        case 0:
                            return modIceCream;
                        case 1:
                            while (true)
                            {
                                Console.WriteLine("Flavour List: ");
                            foreach (KeyValuePair<string, int> kvp in Flavours)
                            {
                                string y = kvp.Key;
                                if (kvp.Value != 0)
                                {
                                    y += "(Premium)";
                                }
                                Console.WriteLine($"-{y}");
                            }
                            Console.Write("What flavour do you want?: ");
                                try
                                {
                                    bool ChosenExists = false;
                                    string Chosen = Console.ReadLine();
                                    foreach (KeyValuePair<string, int> x in Flavours)
                                    {
                                        if (x.Key.ToLower() == Chosen.ToLower())
                                        {
                                            if (x.Value != 0)
                                            {
                                                premium = true;
                                            }
                                            ChosenExists = true;
                                            Chosen = x.Key;
                                            break;
                                        }
                                    }
                                    if (!ChosenExists)
                                    {
                                        Console.WriteLine("Enter a valid flavour.");
                                        continue;
                                    }
                                    Console.WriteLine();
                                    Console.WriteLine("Flavours you currently have: ");
                                    foreach (Flavour f in modIceCream.Flavours)
                                    {
                                        string Premium = "No";
                                        string s = "";
                                        s += "Flavour: " + f.Type + "\tPremium: " + Premium + "\tQuantity: " + f.Quantity;
                                        if (f.Premium)
                                        {
                                            Premium = "Yes";
                                        }
                                        Console.WriteLine($"-{s}");
                                    }
                                    Console.Write("Which flavour to change?: ");
                                    bool ChangeExists = false;
                                    string Change = Console.ReadLine();
                                    foreach (Flavour f in modIceCream.Flavours)
                                    {
                                        if (f.Type.ToLower() == Change.ToLower())
                                        {
                                            Change = f.Type;
                                            ChangeExists = true;
                                        }
                                    }
                                    if (!ChangeExists)
                                    {
                                        Console.WriteLine("Enter a valid flavour to change");
                                        continue;
                                    }
                                    while (true)
                                    {
                                        try
                                        {
                                            Flavour newFlavour = new Flavour(Chosen, premium, 1);
                                            foreach (Flavour f in modIceCream.Flavours)
                                            {
                                                if (f.Type == Change && f.Quantity > 1)
                                                {
                                                    f.Quantity -= 1;
                                                    foreach (Flavour flav in modIceCream.Flavours)
                                                    {
                                                        if (flav.Type == Chosen)
                                                        {
                                                            flav.Quantity += 1;
                                                        }
                                                    }
                                                    modIceCream.Flavours.Add(newFlavour);
                                                    Console.WriteLine("Flavour Modified.");
                                                    return modIceCream;
                                                }
                                                else if (f.Type == Change && f.Quantity <= 1)
                                                {
                                                    modIceCream.Flavours.Remove(f);
                                                    foreach (Flavour flav in modIceCream.Flavours)
                                                    {
                                                        if (flav.Type == Chosen)
                                                        {
                                                            flav.Quantity += 1;
                                                            Console.WriteLine("Flavour Modified.");
                                                            return modIceCream;
                                                        }
                                                    }
                                                    modIceCream.Flavours.Add(newFlavour);
                                                    return modIceCream;
                                                }
                                            }
                                            Console.WriteLine("Flavour Modified.");
                                            return modIceCream;
                                        }
                                        catch (FormatException)
                                        {
                                            Console.WriteLine("Give a valid flavour to change");
                                        }
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Give a valid flavour");
                                }
                            }
                    }
                    return modIceCream;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter a valid flavour");
                }
            }

        }

        private IceCream ModifyToppings(IceCream modIceCream)
        {
            List<string> toppings = InitaliseToppings();
            int option = -1;
            while (true)
            {
                try
                {
                    Console.WriteLine("Your Toppings: ");
                    Console.WriteLine("[0] Exit");
                    Console.WriteLine("[1] Add Toppings");
                    Console.WriteLine("[2] Remove Toppings");
                    Console.Write("Enter your option: ");
                    option = Convert.ToInt32(Console.ReadLine());

                    switch (option)
                    {
                        case 0:
                            return modIceCream;
                        case 1:

                            if (modIceCream.Toppings.Count == 4)
                            {
                                Console.WriteLine("Max Toppings!!");
                                Console.WriteLine();
                                return modIceCream;
                            }
                            else
                            {
                                Console.WriteLine("Toppings Available: ");
                                foreach (string topping in toppings)
                                {
                                    Console.WriteLine("-{0}", topping);
                                }
                                while (true)
                                {
                                    try
                                    {
                                        bool ToppingExist = false;
                                        Console.Write("Which topping to add: ");
                                        string chosen = Console.ReadLine();
                                        foreach (string topping in toppings)
                                        {

                                            if (topping.ToLower() == chosen.ToLower())
                                            {
                                                
                                                chosen = topping;
                                                ToppingExist = true;
                                                break;
                                            }
                                        }
                                        foreach (Topping t in modIceCream.Toppings)
                                        {
                                            if (t.Type == chosen)
                                            {
                                                Console.WriteLine("Topping already added.");
                                                Console.WriteLine();continue;
                                            }
                                        }
                                        if (!ToppingExist)
                                        {
                                            Console.WriteLine("Give a valid topping");
                                            continue;
                                        }
                                        Topping top = new Topping(chosen);
                                        modIceCream.Toppings.Add(top);
                                        Console.WriteLine("Topping Added.");
                                        Console.WriteLine();
                                        return modIceCream;
                                    }
                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Give a valid topping.");
                                    }
                                }
                            }


                        case 2:
                            if (modIceCream.Toppings.Count == 0)
                            {
                                Console.WriteLine("No Toppings to remove!");
                                return modIceCream;
                            }
                            else
                            {
                                Console.WriteLine("Your toppings: ");
                                foreach (Topping t in modIceCream.Toppings)
                                {
                                    Console.WriteLine("-{0}", t.Type);
                                }
                                while (true)
                                {
                                    try
                                    {   bool ToppingExist = false;
                                        Console.Write("Which topping to remove?: ");
                                        string removeTop = Console.ReadLine().ToLower();
                                        foreach (Topping t in modIceCream.Toppings)
                                        {
                                            if (t.Type.ToLower() == removeTop)
                                            {
                                                Console.WriteLine("Toppign Removed.");
                                                modIceCream.Toppings.Remove(t);
                                                return modIceCream;
                                            }
                                        }
                                        if (!ToppingExist)
                                        {
                                            Console.WriteLine("Give a valid topping");
                                        }
                                    }
                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Give a valid topping");
                                    }
                                }
                            }
                        default:
                            Console.WriteLine("Give a valid choice");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Give a valid option");
                }

            }
        }

        private Cone ModifyDippedCone(Cone modIceCream)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("[0] Exit.");
                    Console.WriteLine("[1] Dip Cone");
                    Console.WriteLine("[2] Undip Cone");
                    Console.Write("Enter your option: ");
                    int Choice = Convert.ToInt32(Console.ReadLine());
                    switch (Choice)
                    {
                        case 0:
                            return modIceCream;
                        case 1:
                            if (modIceCream.Dipped == true)
                            {
                                Console.WriteLine("Ice Cream already dipped.\n");
                                return modIceCream;
                            }
                            modIceCream.Dipped = true;
                            Console.WriteLine("Cone has been dipped!\n");
                            return modIceCream;
                        case 2:
                            if (modIceCream.Dipped == false)
                            {
                                Console.WriteLine("Cone already not dipped.\n");
                                return modIceCream;
                            }
                            modIceCream.Dipped = false;
                            Console.WriteLine("Cone undipped!\n");
                            return modIceCream;
                        default:
                            Console.WriteLine("Enter a valid option");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Give a valid number");
                }
            }
        }

        private Waffle ModifyWaffle(Waffle modIceCream)
        {
            string[] WFlavours = { "Original", "Red velvet", "Charcoal", "Pandan" };
            int option = -1;
            while (true)
            {
                try
                {
                    Console.WriteLine("[0]Exit.");
                    Console.WriteLine("[1]Change flavour.");
                    Console.Write("Enter your option: ");
                    option = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Give a valid choice.");
                }
            }
            switch (option)
            {
                case 0:
                    return modIceCream;
                case 1:
                    Console.WriteLine("Current Flavour: {0}", modIceCream.WaffleFlavour);
                    Console.WriteLine();
                    Console.WriteLine("Available Flavours: ");
                    foreach (string Flav in WFlavours)
                    {
                        if (Flav != "Original")
                        {
                            Console.WriteLine("-{0}", Flav + "(+$3)");continue;
                        }
                        Console.WriteLine("-{0}", Flav);
                    }
                    while (true)
                    {
                        Console.Write("Choose a flavour: ");
                        string ChosenFlavour = Console.ReadLine();
                        foreach (string flav in WFlavours)
                        {
                            if (flav.ToLower() == ChosenFlavour.ToLower())
                            {
                                modIceCream.WaffleFlavour = flav;
                                Console.WriteLine("Flavour Changed!.");
                                return modIceCream;
                            }
                        }
                        if (true)
                        {
                            Console.WriteLine("Enter a valid flavour");
                        }
                    }
                default:
                    Console.WriteLine("Enter a valid option");
                    break;
            }
            return modIceCream;
        }





        public void ModifyIceCream(int i)
        {

            string[] PossibleChoices = { "Option", "Scoops", "Flavors", "Toppings" };
            IceCream modIceCream = iceCreamList[i - 1];
            if (iceCreamList.Count <= 0)
            {
                Console.WriteLine("No Ice Cream in the order.");
                return;
            }

            for (int x = 0; x < PossibleChoices.Count(); x++)
            {
                Console.WriteLine("[{0}]: {1}", x + 1, PossibleChoices[x]);
            }
            if (modIceCream is Cone)
            {
                Console.WriteLine("[{0}]: {1}", PossibleChoices.Length + 1, "Cone Dip");
            }
            else if (modIceCream is Waffle)
            {
                Console.WriteLine("[{0}]: {1}", PossibleChoices.Length + 1, "Waffle Flavor");
            }
            while (true)
            {
                try
                {
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
                            modIceCream = ModifyToppings(modIceCream);
                            break;
                        case 5:
                            if (modIceCream is Cone)
                            {
                                modIceCream = ModifyDippedCone((Cone)modIceCream);
                            }
                            else if (modIceCream is Waffle)
                            {
                                modIceCream = ModifyWaffle((Waffle)modIceCream);
                            }
                            break;
                        default:
                            Console.WriteLine("Give a valid choice");
                            break;
                    }
                    iceCreamList[i - 1] = modIceCream;
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Give a valid option");
                }
            }
            return;
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
                icecreams += ice.ToString() + "\n";
            }

            return ("Id: " + Id + "\tTime Received: " + TimeReceived + "\tTime Fulfilled: " + TimeFulfilled + "\nIce Creams: " + icecreams);
        }


    }
}
