using Assignment_IceCream_Shop;
using System;
using System.Globalization;

Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
Queue<Order> RegularQueue = new Queue<Order>();
Queue<Order> GoldQueue = new Queue<Order>();
List<Flavour> flavourList = new List<Flavour>();
List<Topping> toppingList = new List<Topping>();

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

                //Adding pointCard to customer
                customer.Rewards = pointcard;
                customerDict.Add(customerID, customer);
            }
        }
    };
}

//Listing all customer information (Feature 1)
void ListCustomer(Dictionary<int, Customer> customerDict)
{
    Console.WriteLine("CUSTOMER INFORMATION: ");
    Console.WriteLine("{0, -10} {1, 15} {2, 15} {3, 20} {4, 20} {5, 15}", "Name", "Membership ID", "Date of Birth", "Membership Status", "Membership Points", "Punch Card", "Tier");
    foreach (var kvp in customerDict)
    {
        Customer customer = kvp.Value;
        Console.WriteLine("{0, -10} {1, 15} {2, 15} {3, 20} {4, 20} {5, 15}", customer.Name, customer.MemberId, customer.Dob.ToString("dd/MM/yyyy"), customer.Rewards.Tier, customer.Rewards.Points, customer.Rewards.PunchCards);
    }
}
 
void InitaliseOrder(Queue<Order> RegularQueue, Queue<Order> GoldQueue, Dictionary<int, Customer> customerDict)
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
                       
;                    }
                }
                
                for (int j = 11; j <= 14; j++)
                {
                    string Topping = data[j];
                    if (!string.IsNullOrEmpty(Topping))
                    {
                        Topping top = new Topping(Topping);
                        toppingsList.Add(top);
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
                /*
                foreach (KeyValuePair<int,Customer> kvp in customerDict)
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

//Registering a new customer (Feature 3)
void RegisterNewCustomer(Dictionary<int, Customer> customerDict)
{
    Console.Write("Enter customer's name: ");
    string name = Console.ReadLine();
    Console.Write("Enter Membership ID number: ");
    int memberId = Convert.ToInt32(Console.ReadLine());
    Console.Write("Enter Customer's Date-Of-Birth (dd/MM/yyyy): ");
    DateTime dob = Convert.ToDateTime(Console.ReadLine());

    Customer newCustomer = new Customer(name, memberId, dob);

    PointCard newPointCard = new PointCard(0, 0);
    newPointCard.Tier = "Ordinary";
    newCustomer.Rewards = newPointCard;

    customerDict.Add(memberId, newCustomer);

    Console.WriteLine("Registration successful!");
}

//Creating a customer's order (Feature 4)
void CreateCustomerOrder (Dictionary<int, Customer> customerDict)
{
    Console.WriteLine("CREATING CUSTOMER ORDER: \n");
    ListCustomer(customerDict);
    Console.Write("\nEnter customer's name: ");
    string name = Console.ReadLine();

    Customer currentCustomer = new Customer();
    foreach (var kvp in customerDict)
    {
        if (kvp.Value.Name == name)
        {
            currentCustomer = kvp.Value;
        }
    }

    string[] orderLine = File.ReadAllLines("orders.csv");
    for (int i = 1; i < orderLine.Length; i++)
    {
        string[] orderData = orderLine[i].Split(",");
        int orderId = Convert.ToInt32(orderData[0]);
        int memberId = Convert.ToInt32(orderData[1]);

        if (currentCustomer.MemberId == memberId)
        {
            Order currentIceCreamOrder = IceCreamOptionChoice(flavourList, toppingList);
            Order orderDetails = currentCustomer.MakeOrder();
            Order order = new Order(orderId, orderDetails.TimeReceived);

            Console.WriteLine(currentCustomer);

            Console.Write("Would you like to add another ice cream to the order [Y/N]: ");
            string addOrderChoice = Console.ReadLine();

            while (addOrderChoice != "N")
            {
                Order newCurrentIceCreamOrder = IceCreamOptionChoice(flavourList, toppingList);
                Order newOrderDetails = currentCustomer.MakeOrder();
                Order newOrder = new Order(orderId, orderDetails.TimeReceived);

                Console.Write("Would you like to add another ice cream to the order [Y/N]: ");
                addOrderChoice = Console.ReadLine();
            }
            
        }

    }
}
static Order IceCreamOptionChoice(List<Flavour> flavourList, List<Topping> toppingList)
{
    Console.Write("Enter your ice cream option: ");
    string iceCreamOption = Console.ReadLine();

    Console.Write("Enter the number of ice cream scoops: ");
    int noOfScoops = Convert.ToInt32(Console.ReadLine());

    for (int i = 0; i < noOfScoops; i++)
    {
        Console.Write("Enter the ice cream flavour: ");
        string flavourType = Console.ReadLine();
        bool isPremium = false;
        string[] flavourLine = File.ReadAllLines("flavours.csv");
        for (int j = 1; j < flavourLine.Length; j++)
        {
            string[] flavourData = flavourLine[j].Split(",");
            double flavourCost = Convert.ToDouble(flavourData[1]);
            if (flavourData[0] == flavourType)
            {
                if (flavourCost != 0)
                {
                    isPremium = true;
                }
            }
        }
        Flavour flavour = new Flavour(flavourType, isPremium, 1);
        flavourList.Add(flavour);
    }

    Console.Write("Enter the number of toppings to add: ");
    int noOfToppings = Convert.ToInt32(Console.ReadLine());
    for (int i = 0; i < noOfToppings; i++)
    {
        Console.Write("Enter topping type: ");
        string toppingType = Console.ReadLine();

        Topping topping = new Topping(toppingType);
        toppingList.Add(topping);
    }

    Order order = new Order();

    if (iceCreamOption == "Cup")
    {
        IceCream iceCreamOrder = new Cup(iceCreamOption, noOfScoops, flavourList, toppingList);
        order.AddIceCream(iceCreamOrder);
    }
    else if (iceCreamOption == "Cone")
    {
        Console.Write("Do you want to upgrade to Chocolate-dipped cone? [Y/N]: ");
        string choice = Console.ReadLine();
        bool isDipped = false;
        if (choice == "Y")
        {
            isDipped = true;
        }
        IceCream iceCreamOrder = new Cone(iceCreamOption, noOfScoops, flavourList, toppingList, isDipped);
        order.AddIceCream(iceCreamOrder);
    }
    else if (iceCreamOption == "Waffle")
    {
        Console.Write("Enter Waffle flavour: ");
        string wFlavour = Console.ReadLine();

        IceCream iceCreamOrder = new Waffle(iceCreamOption, noOfScoops, flavourList, toppingList, wFlavour);
        order.AddIceCream(iceCreamOrder);
    }
    return(order);
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

}

IceCream printSelected(Customer wantedCustomer)
{
    List<IceCream> iceList = wantedCustomer.CurrentOrder.iceCreamList;
    string s = "";
    List<string> k = new List<string>();
    List<string> t = new List<string>();
    string mod = "None";
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
    return iceList[1];
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
            ListCustomer(customerDict);
            break;
        case 2:
            ListCurrentOrders(GoldQueue, RegularQueue);
            break;
        case 3:
            RegisterNewCustomer(customerDict);
            break;
        case 4:
            CreateCustomerOrder(customerDict);
            break;
        case 5:
            OrderDetailsCustomer(customerDict);
            break;
        case 6:
            Customer wantedCustomer = printCustomers(customerDict);

            break;
    }
}


