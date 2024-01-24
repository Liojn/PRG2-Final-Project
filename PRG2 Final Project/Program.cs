using PRG2_Final_Project;
using System;
using System.Globalization;

Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
Queue<Order> RegularQueue = new Queue<Order>();
Queue<Order> GoldQueue = new Queue<Order>();
void InitialiseCustomers(Dictionary<int,Customer> customerDict)
{
    string line;
    int count = 0;
    using StreamReader sr = new StreamReader("customers.csv");
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
                string customerName = data[0];
                int customerID = Convert.ToInt32(data[1]);

                //Change data[2] into correct DateTime Format
                DateTime dob = DateTime.ParseExact(data[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //Creating customer object
                Customer customer = new Customer(customerName, customerID, dob);

                //Creating PointCard object
                string tier = data[3];
                int points = Convert.ToInt32(data[4]);
                int punch = Convert.ToInt32(data[5]);
                PointCard pointcard = new PointCard(points, punch);
                pointcard.Tier = tier;

                //Adding pointCard to custoemr
                customer.Rewards = pointcard;
                customerDict.Add(customerID, customer);
            }
        }
    };
}

void InitaliseOrder(Dictionary<int, Customer> customerDict)
{

    string line;
    int count = 0;
    using StreamReader sr = new StreamReader("orders.csv");
    {
        while ((line = sr.ReadLine()) != null)
        {
            List<Flavour> flavourList = new List<Flavour>();
            List<Topping> toppingsList = new List<Topping>();
            if (count == 0)
            {
                count++;
            }
            else
            {
                //27/10/2023 13:28
                string[] data = line.Split(",");

                //Extracting Data
                bool dippedCone = false;
                int id = Convert.ToInt32(data[0]);
                int MemberID = Convert.ToInt32(data[1]);
                string Option = data[4];
                int Scoops = Convert.ToInt32(data[5]);
                DateTime TimeRecevied = DateTime.ParseExact(data[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime TimeFulfilled  = DateTime.ParseExact(data[3], "dd/MM/yyyy HH:mm",CultureInfo.InvariantCulture);
                bool premium = false;
                string waffleFlavour = data[7];
                if (Option == "Cone")
                {
                    dippedCone = Convert.ToBoolean(data[6].ToLower());
                }


                Order order = new Order(id, TimeRecevied);
                order.TimeFulfilled = TimeFulfilled;
                for (int i = 8; i <= 10; i++)
                {
                    bool noExist = false;
                    string Flavour = data[i];
                    if (!string.IsNullOrEmpty(Flavour))
                    {
                        if (Flavour == "Durian" || Flavour == "Ube" || Flavour == "Sea Salt")
                        {
                            premium = true;
                        }
                        Flavour newFlavour = new Flavour(Flavour, premium, 1);
                        foreach (Flavour f in flavourList)
                        {

                            if (f.Type == newFlavour.Type)
                            {
                                f.Quantity += 1;
                                noExist = true;
                            }
                        }
                        if (noExist == false)
                        {
                            flavourList.Add(newFlavour);
                        }
;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int j = 11; j <= 14; j++)
                {
                    string Topping = data[j];
                    if (!string.IsNullOrEmpty(Topping))
                    {
                        Topping top = new Topping(Topping);
                        toppingsList.Add(top);
                    }
                    else
                    {
                        break;
                    }
                }


                if (Option == "Waffle")
                {
                    IceCream ice = new Waffle(Option, Scoops, flavourList, toppingsList, waffleFlavour);
                    order.iceCreamList.Add(ice);
                    ice.Flavours = flavourList;
                    ice.Toppings = toppingsList;
                }
                else if (Option == "Cone")
                {
                    IceCream ice = new Cone(Option, Scoops, flavourList, toppingsList, dippedCone);
                    order.iceCreamList.Add(ice);
                    ice.Flavours = flavourList;
                    ice.Toppings = toppingsList;
                }
                else
                {
                    IceCream ice = new Cup(Option, Scoops, flavourList, toppingsList);
                    order.iceCreamList.Add(ice);
                    ice.Flavours = flavourList;
                    ice.Toppings = toppingsList;
                }

               /* foreach (KeyValuePair<int,Customer> kvp in customerDict)
                {
                    if (MemberID == kvp.Key)
                    {
                        if (kvp.Value.Rewards.Tier == "Gold")
                        {
                            kvp.Value.orderHistory.Add(order);
                            GoldQueue.Enqueue(order);
                        }
                        else
                        {
                            kvp.Value.orderHistory.Add(order);
                            RegularQueue.Enqueue(order);
                        }
                    }
                }*/
            }
        }
    }
}


void Menu()
{
    Console.WriteLine("==========Menu==========");
    Console.WriteLine("[1]List all customers.");
    Console.WriteLine("[2]List all current orders.");
    Console.WriteLine("[3]Register a new customer.");
    Console.WriteLine("[4]Create a new customer's order.");
    Console.WriteLine("[5]Display order details of a customer.");
    Console.WriteLine("[6]Modify Order Detail.");
}

//Printing orders
void OrderPrint(Queue<Order> queue)
{
    foreach (Order o in queue)
    {
        string s = "";
        List<string> k = new List<string>();
        List<string> t = new List<string>();
        string mod = "None";
        foreach (IceCream ice in o.iceCreamList)
        {
            foreach (Flavour f in ice.Flavours)
            {
                if (f.Premium == true)
                {
                    string y = f.Type + "(Premium)" + " Quantity:  " + f.Quantity;
                    k.Add(y);
                }
                else
                {
                    string y = f.Type + " Quantity: " + f.Quantity;
                    k.Add(y);
                }

            }
            foreach (Topping i in ice.Toppings)
            {
                t.Add(i.Type);
            }
            if (ice.Toppings.Count == 0)
            {
                t.Add("None");
            }

            if (ice is Waffle)
            {
                Waffle waf = (Waffle)ice;
                mod = waf.WaffleFlavour;
            }
            else if (ice is Cone)
            {
                Cone co = (Cone)ice;
                mod = Convert.ToString(co.Dipped);
            }
            string joinedFlavour = String.Join(",", k.ToArray());
            string joinedTopping = String.Join(",", t.ToArray());
            s += "Option: " + ice.Option + "\tScoops: " + ice.Scoops + "\nModifications: " + mod + "\nFlavours: " + joinedFlavour + "\tToppings: " + joinedTopping;
        }

        Console.WriteLine("ID :{0,-5} Time Received: {1,-22} Time Fulfilled: {2,-22}{3,-22}",o.Id, o.TimeReceived, o.TimeFulfilled, s);
        Console.WriteLine();
    }
}


//List Current orders Feature 2
void ListCurrentOrders(Queue<Order> GoldQueue, Queue<Order> RegularQueue)
{
    Console.WriteLine("==========Gold Queue Orders==========");
    OrderPrint(GoldQueue);
    Console.WriteLine("==========Regular Queue Orders==========");
    OrderPrint(RegularQueue);

}


//Print customers
Customer printCustomers(Dictionary<int, Customer> customerDict)
{
    Console.WriteLine("Customers: ");
    foreach (Customer customer in customerDict.Values)
    {
        Console.WriteLine("-{0}", customer.Name);
    }
    Console.Write("Select a customer: ");
    string option = Console.ReadLine().ToLower();
    Customer wantedCustomer = new Customer();
    foreach (KeyValuePair<int, Customer> kvp in customerDict)
    {
        if (kvp.Value.Name.ToLower() == option)
        {
            wantedCustomer = kvp.Value;
        }
    }
    return wantedCustomer;
}

//Display Order details of a customer
void OrderDetailsCustomer(Dictionary<int, Customer> customerDict)
{
  
    Customer wantedCustomer = printCustomers(customerDict);
    Console.WriteLine("==========Current and Past Order Details==========");
    foreach (Order o in wantedCustomer.orderHistory)
    {
        string s = "";
        List<string> k = new List<string>();
        List<string> t = new List<string>();
        string mod = "None";
        foreach (IceCream ice in o.iceCreamList)
        {
            foreach (Flavour f in ice.Flavours)
            {
                if (f.Premium == true)
                {
                    string y = f.Type + "(Premium)" + " Quantity:  " + f.Quantity;
                    k.Add(y);
                }
                else
                {
                    string y = f.Type + " Quantity: " + f.Quantity;
                    k.Add(y);
                }

            }
            foreach (Topping i in ice.Toppings)
            {
                t.Add(i.Type);
            }
            if (ice.Toppings.Count == 0)
            {
                t.Add("None");
            }

            if (ice is Waffle)
            {
                Waffle waf = (Waffle)ice;
                mod = waf.WaffleFlavour;
            }
            else if (ice is Cone)
            {
                Cone co = (Cone)ice;
                mod = Convert.ToString(co.Dipped);
            }
            string joinedFlavour = String.Join(",", k.ToArray());
            string joinedTopping = String.Join(",", t.ToArray());
            s += "Option: " + ice.Option + "\tScoops: " + ice.Scoops + "\nModifications: " + mod + "\nFlavours: " + joinedFlavour + "\tToppings: " + joinedTopping;
        }

        Console.WriteLine("ID: {0,-5}{1,-22}{2,-22}{3,-22}", o.Id, o.TimeReceived, o.TimeFulfilled, s);
        Console.WriteLine();
    }

}

int printSelected(Customer wantedCustomer)
{
    List<IceCream> iceList = wantedCustomer.CurrentOrder.iceCreamList;
    string s = "";
    List<string> k = new List<string>();
    List<string> t = new List<string>();
    string mod = "None";
    Console.WriteLine("=================================Current Orders==========================================");
    foreach (IceCream ice in iceList)
    {
        foreach (Flavour f in ice.Flavours)
        {
            if (f.Premium == true)
            {
                string y = f.Type + "(Premium)" + " Quantity:  " + f.Quantity;
                k.Add(y);
            }
            else
            {
                string y = f.Type + " Quantity: " + f.Quantity;
                k.Add(y);
            }

        }
        foreach (Topping i in ice.Toppings)
        {
            t.Add(i.Type);
        }
        if (ice.Toppings.Count == 0)
        {
            t.Add("None");
        }

        if (ice is Waffle)
        {
            Waffle waf = (Waffle)ice;
            mod = waf.WaffleFlavour;
        }
        else if (ice is Cone)
        {
            Cone co = (Cone)ice;
            mod = Convert.ToString(co.Dipped);
        }
        string joinedFlavour = String.Join(",", k.ToArray());
        string joinedTopping = String.Join(",", t.ToArray());
        s += "Option: " + ice.Option + "\tScoops: " + ice.Scoops + "\nModifications: " + mod + "\nFlavours: " + joinedFlavour + "\tToppings: " + joinedTopping;
    }

    Console.WriteLine("ID :{0,-5} Time Received: {1,-22} Time Fulfilled: {2,-22}{3,-22}", wantedCustomer.CurrentOrder.Id, wantedCustomer.CurrentOrder.TimeReceived, wantedCustomer.CurrentOrder.TimeFulfilled, s);
    Console.WriteLine();
    return 1;
}

InitialiseCustomers(customerDict);
InitaliseOrder(RegularQueue, GoldQueue, customerDict);







int option = -1;
while (option != 0)
{
    Menu();
    Console.Write("Enter your option: ");
    option = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine();
    switch (option)
    {
        case 1:

            break;
        case 2:
            ListCurrentOrders(GoldQueue, RegularQueue);
            break;
        case 3:
            break;
        case 4:
            break;
        case 5:
            OrderDetailsCustomer(customerDict);
            break;
        case 6:
            Customer wantedCustomer = printCustomers(customerDict);
            int index = printSelected(wantedCustomer);
            break;
    }
}


