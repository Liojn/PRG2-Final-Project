using Assignment_IceCream_Shop;
using Microsoft.VisualBasic;
using PRG2_Final_Project;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;



//==========================================================
// Student Number : S10259865
// Student Name : Kiefer Yew
// Partner Name : Thet Mon
//=========================================================


Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); // Dictionary of MemberID to Customer object
List<Order> orders = new List<Order>(); // A list to store all the orders ever made
Queue<Order> RegularQueue = new Queue<Order>(); // 
Queue<Order> GoldQueue = new Queue<Order>();
Dictionary<string, int> FlavoursFile = new Dictionary<string, int>(); // Dictionary of flavours of Flavour Name : Cost
List<string> ToppingsFile = new List<string>(); // List of topping names


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void InitaliseFlavour(Dictionary<string, int> FlavoursFile) /// Reading Flavour.csv
{
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
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void InitaliseToppings(List<string> ToppingsFile) /// Reading Toppings.csv
{
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
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////// Reading the customer.csv file
void InitialiseCustomers(Dictionary<int, Customer> customerDict, List<Order> orders)
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
                string[] dataFormats = { "dd/MM/yyy", "d/MM/yyy", "dd/M/yyyy" , "d/M/yyyy"}; //// Possible formats the csv time could be.
                DateTime dob = DateTime.ParseExact(data[2], dataFormats, CultureInfo.InvariantCulture); //// CultureInfo.invariantCulture means not  time is not culture sensitive
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


////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Reading the order.csv file
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

                string[] data = line.Split(",");

                //Extracting Data
                bool dippedCone = false;

                int id = Convert.ToInt32(data[0]);

                int MemberID = Convert.ToInt32(data[1]);

                string Option = data[4];

                int Scoops = Convert.ToInt32(data[5]);


                string[] dateFormats = {  "MM/dd/yyyy h:mm:ss tt", "M/dd/yyyy h:mm:ss tt" , "MM/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm:ss tt" , "dd/MM/yyyy HH:mm", "dd/M/yyyy HH:mm", "d/MM/yyyy HH:mm", "d/M/yyyy HH:mm" };//////////// Possible formats.
                DateTime TimeRecevied = DateTime.ParseExact(data[2], dateFormats, CultureInfo.InvariantCulture); /// Use possible formats to check whether data[2] lies within any of those formats
                DateTime TimeFulfilled = DateTime.ParseExact(data[3], dateFormats, CultureInfo.InvariantCulture); /// Same for data[3]


                bool premium = false;

                string waffleFlavour = data[7];



                if (Option == "Cone")
                {
                    dippedCone = Convert.ToBoolean(data[6].ToLower());
                }

                Order order = new Order(id, TimeRecevied); // Create new order objecy

                order.TimeFulfilled = TimeFulfilled;

                for (int i = 8; i <= 10; i++)
                {
                    bool noExist = false;
                    string Flavour = data[i];
                    if (!string.IsNullOrEmpty(Flavour)) // Checking if its NOT empty or null
                    {
                        if (Flavour == "Durian" || Flavour == "Ube" || Flavour == "Sea Salt") // Checking if premium
                        {
                            premium = true;
                        }
                        Flavour newFlavour = new Flavour(Flavour, premium, 1);
                        foreach (Flavour f in flavourList)
                        {

                            if (f.Type == newFlavour.Type) // If Flavour exists already inside the list
                            {
                                f.Quantity += 1;
                                noExist = true; // Increment Quantity by 1
                            }
                        }
                        if (noExist == false)
                        {
                            flavourList.Add(newFlavour);  // Else add to the list 
                        }
;
                    }
                    else // Break cause if null or empty, every subsequent flavour is also null or empty
                    {
                        break;
                    }
                }

                for (int j = 11; j < data.Count(); j++)
                {
                    string Topping = data[j];
                    if (!string.IsNullOrEmpty(Topping)) // Check if  NOT null or empty
                    {
                        Topping top = new Topping(Topping);
                        toppingsList.Add(top);
                    }
                    else 
                    {
                        break; // Break cause, if null or empty all toppings behind also null or empty
                    }
                }

                int OrdersIndex = -1;
                bool exists = false;

                foreach (Order o in orders)
                {
                    if (o.Id == id) // Checking if the ID exists already inside main order list.
                    {
                        OrdersIndex = orders.IndexOf(o); 
                        exists = true;
                    }
                }

                if (exists) // Cause if it does exists, we have to add iceCream to its IceCream List.
                {
                    if (Option == "Waffle")
                    {
                        IceCream ice = new Waffle(Option, Scoops, flavourList, toppingsList, waffleFlavour);
                        ice.Flavours = flavourList;
                        ice.Toppings = toppingsList;
                        orders[OrdersIndex].iceCreamList.Add(ice);
                    }
                    else if (Option == "Cone")
                    {
                        IceCream ice = new Cone(Option, Scoops, flavourList, toppingsList, dippedCone);
                        ice.Flavours = flavourList;
                        ice.Toppings = toppingsList;
                        orders[OrdersIndex].iceCreamList.Add(ice);
                    }
                    else
                    {
                        IceCream ice = new Cup(Option, Scoops, flavourList, toppingsList);
                        ice.Flavours = flavourList;
                        ice.Toppings = toppingsList;
                        orders[OrdersIndex].iceCreamList.Add(ice);
                    }
                }
                else // Else if it dosent exist we use the order object we created and add to its ice cream list 
                {
                    if (Option == "Waffle")
                    {
                        IceCream ice = new Waffle(Option, Scoops, flavourList, toppingsList, waffleFlavour);
                        ice.Flavours = flavourList;
                        ice.Toppings = toppingsList;
                        order.iceCreamList.Add(ice);
                    }
                    else if (Option == "Cone")
                    {
                        IceCream ice = new Cone(Option, Scoops, flavourList, toppingsList, dippedCone);
                        ice.Flavours = flavourList;
                        ice.Toppings = toppingsList;
                        order.iceCreamList.Add(ice);
                    }
                    else
                    {
                        IceCream ice = new Cup(Option, Scoops, flavourList, toppingsList);
                        ice.Flavours = flavourList;
                        ice.Toppings = toppingsList;
                        order.iceCreamList.Add(ice);
                    }
                    foreach (KeyValuePair<int, Customer> kvp in customerDict)
                    {
                        if (MemberID == kvp.Key)
                        {
                            kvp.Value.orderHistory.Add(order); // Find its corresponding customer and add it their history.
                            break;
                        }
                    }
                    orders.Add(order); // Then we append it to main order list.
                }
                
            }
        }
    }
}



/////////////////////////////////////////////////////////////////////////////////////////////////////////// Appending to customer.csv File

void UpdateCustomerCSV(Dictionary<int, Customer> customerDict)
{
    string FilePath = "customers.csv";
    string dataline = "";
    string Header = "Name,MemberId,DOB,MembershipStatus,MembershipPoints,PunchCard"; // Header for the file
    using StreamWriter writer = new StreamWriter(FilePath, false); // Overwriting whole file 
    {
        writer.WriteLine(Header);
        foreach (Customer customer in customerDict.Values) // Loop through customer list, writing them in order
        {
            dataline += customer.Name + "," + customer.MemberId + "," + customer.Dob.ToString("dd/MM/yyyy") + "," + customer.Rewards.Tier + "," + customer.Rewards.Points + "," + customer.Rewards.PunchCards;
            writer.WriteLine(dataline);
            dataline = "";
        }
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////// Appending to order.csv file

void UpdateOrderCSV(List<Order> orders, Dictionary<int,Customer> customerDict)
{

    string filepath = "orders.csv";
    using StreamWriter writer = new StreamWriter(filepath, false);
    {
        writer.WriteLine("Id,MemberId,TimeReceived,TimeFulfilled,Option,Scoops,Dipped,WaffleFlavour,Flavour1,Flavour2,Flavour3,Topping1,Topping2,Topping3,Topping4"); // Header for orders.csv
        foreach (Order o in orders)
        {
            string MemberID = "";
            foreach (KeyValuePair<int,Customer> customer in customerDict)
            {
                bool found = false;
                 foreach (Order temp in customer.Value.orderHistory) // Loop through customers orderHistory for find ID 
                {
                    if (temp.Id == o.Id)
                    {
                        MemberID = Convert.ToString(customer.Key);
                        found = true;
                        break;
                    }
                }
                 if (found)
                {
                    break;
                }
            }
            string TimeFulfilled = "";
            if (!string.IsNullOrEmpty(Convert.ToString(o.TimeFulfilled)))
            {
                TimeFulfilled = o.TimeFulfilled?.ToString("MM/dd/yyyy HH:mm"); // Convert into format
            }

            foreach (IceCream ice in o.iceCreamList)
            {
                string joinedFlavour = "";
                string joinedTopping = "";
                string WaffleFlavour = "";
                string Dipped = "";
                List<string> fList = new List<string>();

                foreach (Flavour f in ice.Flavours)
                {
                    if (f.Quantity > 1)
                    {
                        for (int i = 0; i < f.Quantity; i++) // Since when we append to order.csv if theres more than 1 quantity, we need to write multiple instances of that.
                        {
                            fList.Add(f.Type);  
                        }
                    }else // If not we add.
                    {
                        fList.Add(f.Type);
                    }
                }
                if (fList.Count() < 3)
                {
                    for (int i = 0; i <= (3 - fList.Count()); i++) // Since there must be 3 flavours. We will fill in the missing flavours with empty strings to join ltr on
                    {
                        fList.Add("");
                    }
                }
                List<string> tList = new List<string>();
                foreach (Topping t in ice.Toppings) // Add toppings to a lis to join ltr 
                {
                    tList.Add(t.Type);
                }
                if (tList.Count < 4)
                {
                    for (int i = 0; i <= (4 - tList.Count()); i++) // since there must be 4 toppings. We will in the missing toppings with empty strings.
                    {
                        tList.Add("");
                    }
                }

                if (ice is Waffle)
                {
                    Waffle waf = (Waffle)ice;
                    WaffleFlavour = waf.WaffleFlavour;
                }
                else if (ice is Cone)
                {
                    Cone cone = (Cone)ice;
                    if (cone.Dipped)
                    {
                        Dipped = "True";
                    }
                    else
                    {
                        Dipped = "False";
                    }
                }
                joinedFlavour = String.Join(",", fList); // Combining the flavour list into a string. seperated by commas
                joinedTopping = String.Join(",", tList);// Combining the topping 
                writer.Write("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n", o.Id, MemberID, o.TimeReceived, o.TimeFulfilled, ice.Option, ice.Scoops, Dipped, WaffleFlavour, joinedFlavour, joinedTopping);
            }
        }
        
    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void Menu() // menu
{
    Console.WriteLine("==========Menu==========");
    Console.WriteLine("[0]Exit.");
    Console.WriteLine("[1]List all customers.");
    Console.WriteLine("[2]List all current orders.");
    Console.WriteLine("[3]Register a new customer.");
    Console.WriteLine("[4]Create a new customer's order.");
    Console.WriteLine("[5]Display order details of a customer.");
    Console.WriteLine("[6]Modify Order Detail.");
    Console.WriteLine("[7]Process an order and checkout");
    Console.WriteLine("[8]Display monthly charged amounts breakdown & total charged amounts for the year");
}


//Listing all customer information (Feature 1)
void ListCustomer(Dictionary<int, Customer> customerDict)
{
    Console.WriteLine("CUSTOMER INFORMATION: ");
    //Table header
    Console.WriteLine("{0, -10} {1, 15} {2, 15} {3, 20} {4, 20} {5, 15}", "Name", "Membership ID", "Date of Birth", "Membership Status", "Membership Points", "Punch Card", "Tier");
    //Displaying each customer's information
    foreach (var kvp in customerDict)
    {
        Customer customer = kvp.Value;
        Console.WriteLine("{0, -10} {1, 15} {2, 15} {3, 20} {4, 20} {5, 15}", customer.Name, customer.MemberId, customer.Dob.ToString("dd/MM/yyyy"), customer.Rewards.Tier, customer.Rewards.Points, customer.Rewards.PunchCards);
    }
}

//////////////////////////////////////////////////////////////////////////////////////
string FormattingPrint(IceCream ice) // generalised method for formatting string 
{
    string s = "";
    List<string> k = new List<string>();
    List<string> t = new List<string>();
    string mod = "None";
    string joinedFlavour = "";
    string joinedTopping = "";
    foreach (Flavour f in ice.Flavours)
    {
        if (f.Premium == true)
        {
            string y = f.Type + "(Premium)" + " Quantity:  " + f.Quantity; // Adding a premium tag beside the Flavour name is it is a premium flavour
            k.Add(y); // Add to list to join ltr
        }
        else
        {
            string y = f.Type + " Quantity: " + f.Quantity;
            k.Add(y);// Add to list to join ltr
        }

    }
    foreach (Topping i in ice.Toppings)
    {
        t.Add(i.Type); // Add to list to join ltr
    }
    if (ice.Toppings.Count == 0) // if 0 
    {
        t.Add("None"); // We print None
    }

    if (ice is Waffle)
    {
        Waffle waf = (Waffle)ice;
        mod = waf.WaffleFlavour;
    }
    else if (ice is Cone)
    {
        Cone co = (Cone)ice;
        if (co.Dipped == true)
        {
            mod = "Dipped";
        }
    }
    joinedFlavour = String.Join(",", k.ToArray()); // Joing the arrays seperated by ,
    joinedTopping = String.Join(",", t.ToArray()); // joining the list seperated by ,
    s += "Option: " + ice.Option + "\tScoops: " + ice.Scoops + "\nModifications: " + mod + "\nFlavours: " + joinedFlavour + "\tToppings: " + joinedTopping + "\n\n";
    return s; // Return the string to print in what ever functionn
}

void OrderPrint(Queue<Order> queue)///// Print orders. Geenralised method for prinintg the orders in the queue 
{
    foreach (Order o in queue)
    {
        string TimeFulfilled = "";
        string s = "";
        foreach (IceCream ice in o.iceCreamList)
        {
            s += FormattingPrint(ice);
        }
        if (string.IsNullOrEmpty(Convert.ToString(o.TimeFulfilled))) // Check if Null or Empty the TimeFulfilled
        {
            TimeFulfilled = "Unfulfilled"; // If it is , set it to unfulfilled
            Console.WriteLine("ID :{0,-5} Time Received: {1,-22} Time Fulfilled: {2,-22}{3,-22}", o.Id, o.TimeReceived, TimeFulfilled, s);
        }
        else
        {
            Console.WriteLine("ID :{0,-5} Time Received: {1,-22} Time Fulfilled: {2,-22}{3,-22}", o.Id, o.TimeReceived, o.TimeFulfilled, s);
        }
        Console.WriteLine();
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////  List Current orders Feature 2
void ListCurrentOrders(Queue<Order> GoldQueue, Queue<Order> RegularQueue)
{
    Console.WriteLine("==========Gold Queue Orders==========");
    OrderPrint(GoldQueue); // Call geenralised method for prinint orders in queue 
    if (GoldQueue.Count == 0)
    {
        Console.WriteLine("No orders yet."); // no orders
    }
    Console.WriteLine("==========Regular Queue Orders==========");
    OrderPrint(RegularQueue);// Call geenralised method for prinint orders in queue 
    if (RegularQueue.Count == 0)
    {
        Console.WriteLine("No orders yet."); // no orders
    }

}
////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////////////Print customers
Customer printCustomers(Dictionary<int, Customer> customerDict) // Method for printing customers names and choosing one 
{
    Console.WriteLine("Customers: ");
    foreach (Customer customer in customerDict.Values)
    {
        Console.WriteLine("-{0}", customer.Name);
    }
    while (true)
    {
        Console.Write("Select a customer: ");
        string option = Console.ReadLine().ToLower();
        Customer wantedCustomer = new Customer();
        bool found = false;
        foreach (KeyValuePair<int, Customer> kvp in customerDict) // check dictionary for name
        {
            if (kvp.Value.Name.ToLower() == option)
            {
                wantedCustomer = kvp.Value;
                found = true;
            }
        }
        if (found)
        {
            return wantedCustomer;
        }
        else
        {
            Console.WriteLine("Customer not found. Try again"); // Exception handling 
        }
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////      Registering a new customer (Feature 3)
void RegisterNewCustomer(Dictionary<int, Customer> customerDict)
{
    while (true)
    {
        try
        {
            Console.Write("Enter customer's name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Enter a valid name");
                continue;
            }
            Console.Write("Enter Membership ID number: ");
            int memberId = Convert.ToInt32(Console.ReadLine());
            if (customerDict.ContainsKey(memberId))
            {
                Console.WriteLine("Membership ID already exists. Please enter a unique ID.");
                continue;
            }
            Console.Write("Enter Customer's Date-Of-Birth (dd/MM/yyyy): ");
            string DateOfBirth = Console.ReadLine();
            string[] dataFormats = { "dd/MM/yyy", "d/MM/yyy", "dd/M/yyyy","d/M/yyyy" };
            DateTime dob = DateTime.ParseExact(DateOfBirth, dataFormats, CultureInfo.InvariantCulture);
            Customer newCustomer = new Customer(name, memberId, dob);

            PointCard newPointCard = new PointCard(0, 0);
            newPointCard.Tier = "Ordinary";
            newCustomer.Rewards = newPointCard;

            customerDict.Add(memberId, newCustomer);
            Console.WriteLine("Registration successful!");
            return;
        }
        catch(FormatException)
        {
            Console.WriteLine("Invalid Format.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Creating a customer's order (Feature 4)
void CreateCustomerOrder(Dictionary<int, Customer> customerDict, List<Order> orders, Dictionary<string, int> FlavoursFile, List<string> ToppingsFile)
{
    string name = "";
    Console.WriteLine("CREATING CUSTOMER ORDER: \n");
    //displaying all the current members
    ListCustomer(customerDict);
    while (true)
    {
        bool NameExists = false;
        Console.Write("\nEnter customer's name: "); //asking for customer's name to add order into 
        name = Console.ReadLine();
        //checking if the name exists
        foreach (Customer cus in customerDict.Values)
        {
            if (name.ToLower() == cus.Name.ToLower())
            {
                name = cus.Name;
                NameExists = true;
            }
        }
        if (!NameExists)
        {
            Console.WriteLine("Enter a valid name.\n");
            continue;
        }
        else
        {
            break;
        }
    }
    int id = 0;
    Order currentOrder = new Order(); //creating new order object
    //initializing order id 
    if (orders.Count > 0)
    {
        id = orders[orders.Count - 1].Id + 1;
    }
    foreach (var kvp in customerDict)
    {
        if (kvp.Value.Name == name)
        {
            if (kvp.Value.CurrentOrder != null) //message shown when order for the customer has already been created
            {
                Console.WriteLine("Order has already been created.");
                break;
            }
            else
            {
                kvp.Value.MakeOrder(); //calling the method that make order for the specific customer
                kvp.Value.CurrentOrder.Id = id; //adding in the customer's order id 
                currentOrder = kvp.Value.CurrentOrder; //connect customer's current order with the order made now

                IceCream newIce = IceCreamOptionChoice(FlavoursFile, ToppingsFile, currentOrder); //calling method to take in ice cream order 
                currentOrder.iceCreamList.Add(newIce); //adding the ice cream to the order object created


                while (currentOrder.iceCreamList.Count() < 3)
                {
                    while (true)
                    {
                        //asking if customer want more than one ice cream order in the order
                        Console.Write("Do you want to add another ice cream to the order? [Y/N]: ");
                        string addIceCreamChoice = Console.ReadLine();

                        if (addIceCreamChoice.ToUpper() == "Y") //repeat if Y
                        {

                            newIce = IceCreamOptionChoice(FlavoursFile, ToppingsFile, currentOrder);
                            currentOrder.iceCreamList.Add(newIce);
                        }
                        else if (addIceCreamChoice.ToUpper() == "N") //break if N
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Enter [Y/N].");
                        }
                    }
                    break;
                }
                // add to the respective queues
                if (kvp.Value.Rewards.Tier == "Gold")
                {
                    GoldQueue.Enqueue(currentOrder);
                }
                else
                {
                    RegularQueue.Enqueue(currentOrder);
                }
                kvp.Value.CurrentOrder = currentOrder;
                orders.Add(currentOrder);
                Console.WriteLine("Order has been placed successfully!\n");

                break;
            }
        }
    }
}
static IceCream IceCreamOptionChoice(Dictionary<string, int> FlavoursFile, List<string> ToppingsFile, Order currentOrder)
{
    string iceCreamOption = "";
    List<Flavour> newFlavoursList = new List<Flavour>(); //to store flavour
    List<Topping> newToppingList = new List<Topping>(); //to store toppings
    bool isDipped = false;
    string wFlavour = "";
    //displaying choices of ice cream options
    string[] option = { "Cup", "Cone", "Waffle" };
    Console.WriteLine("Available Ice Cream Options: ");
    foreach (string o in option)
    {
        Console.Write("| {0, -15} |", o);
    }
    Console.WriteLine();
    while (true)
    {
        //asking for ice cream option 
        Console.Write("Enter your ice cream option: ");
        iceCreamOption = Console.ReadLine();


        if (iceCreamOption.ToLower() == "waffle") //if option is waffle
        {
            //displaying waffle flavours 
            iceCreamOption = "Waffle";
            string[] waffleFlavourOptions = { "Original", "Pandan", "Red Velvet", "Charcoal" };
            Console.WriteLine("Possible Options: ");
            foreach (string w in waffleFlavourOptions)
            {
                if (w != "Original")
                {
                    Console.Write("| {0,-15} |", w + "(+$3)");
                    continue;
                }
                Console.Write("| {0,-15} |", w);
            }
            Console.WriteLine();
            bool WExists = false;
            while (true)
            {
                Console.Write("Enter the waffle flavour: ");
                wFlavour = Console.ReadLine();
                foreach (string s in waffleFlavourOptions)
                {
                    if (s.ToLower() == wFlavour.ToLower())
                    {
                        WExists = true;
                        wFlavour = s; break;
                    }
                }
                if (!WExists) //if waffle flavour is not valid
                {
                    Console.WriteLine("Give a valid Waffle Flavour");
                }
                else
                {
                    break;
                }
            }
            break;
        }
        else if (iceCreamOption.ToLower() == "cone") //if option is cone
        {
            iceCreamOption = "Cone";
            while (true)
            {
                Console.Write("Do you want to upgrade to a chocolate-dipped cone(+$2)? [Y/N]: "); //asking for chocolate dipped cone
                string chocoDippedChoice = Console.ReadLine();
                if (chocoDippedChoice.ToUpper() == "Y")
                {
                    isDipped = true; break;
                }
                else if (chocoDippedChoice.ToUpper() == "N")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter [Y/N].\n");
                }
            }
            break;
        }
        else if (iceCreamOption.ToLower() == "cup") //if cup
        {
            iceCreamOption = "Cup"; break;
        }
        else
        {
            Console.WriteLine("Invalid option.");
            Console.WriteLine();
        }
    }
    int noOfScoops = -1;
    while (true)
    {
        Console.Write("Enter the number of ice cream scoops (Min 1, Max 3): "); //asking for numbers of scoops of ice cream
        try
        {
            noOfScoops = Convert.ToInt32(Console.ReadLine());
            if (noOfScoops <= 0 || noOfScoops > 3) //min 1 scoop and max 3 scoops
            {
                Console.WriteLine("Enter a valid number of scoops.");
                continue;
            }
            break;
        }
        catch (FormatException) //if format doesn't match
        {
            Console.WriteLine("Enter a valid number.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Message: {0}", ex);
        }
    }
    //displaying the available ice cream flavours
    Console.WriteLine("Flavours Available:");
    foreach (KeyValuePair<string, int> flav in FlavoursFile)
    {
        string y = "";
        y += flav.Key;
        if (flav.Value != 0!)
        {
            y += "(Premium)";
        }
        Console.Write("{0,-20}", y);
    }

    Console.WriteLine();
    for (int i = 0; i < noOfScoops; i++) //asking for the ice cream flavour based on the number of scoops chosen
    {
        bool ValidFlavour = false;
        bool Exists = false;
        bool isPremium = false;
        string flavourType = "";
        while (true)
        {
            Console.Write("Enter the ice cream flavour: ");
            flavourType = Console.ReadLine();
            foreach (string s in FlavoursFile.Keys)
            {
                if (flavourType.ToLower() == s.ToLower())
                {
                    flavourType = s;
                    ValidFlavour = true;
                }
            }
            if (ValidFlavour == false) //if flavour unavailable
            {
                Console.WriteLine("Enter a valid flavour.");
            }
            else
            {
                break;
            }
        }
        foreach (KeyValuePair<string, int> kvp in FlavoursFile) //checking if the flavour chosen is premium for cost calculation
        {
            if (flavourType.ToLower() == kvp.Key.ToLower() && kvp.Value != 0)
            {
                isPremium = true;
            }
        }
        Flavour flavour = new Flavour(flavourType, isPremium, 1); //creating flavour object
        foreach (Flavour flav in newFlavoursList) //checking if there are same flavours chosen
        {
            if (flavourType.ToLower() == flav.Type.ToLower())
            {
                flav.Quantity += 1;
                Exists = true;
            }
        }
        if (Exists == false)
        {
            newFlavoursList.Add(flavour);
        }
    }

    List<string> toppingOptions = ToppingsFile;
    Console.WriteLine();
    int noOfToppings = -1;
    while (true)
    {
        Console.Write("Enter the number of toppings to add (Min 0, Max 4): "); //asking for numbers of toppings
        try
        {
            noOfToppings = Convert.ToInt32(Console.ReadLine());
            if (noOfToppings < 0 || noOfToppings > 4) //min 0 max 4 toppings 
            {
                Console.WriteLine("Enter a valid number of toppings.");
                continue;
            }
            if (noOfToppings == 0)
            {
                break;
            }
            Console.WriteLine("Toppings Available: "); //displaying the toppings options
            foreach (string t in toppingOptions)
            {
                Console.Write("| {0, -15} |", t);
            }
            Console.WriteLine();
            break;
        }
        catch (FormatException) //checking for format match
        {
            Console.WriteLine("Enter a valid number.");
        }
        catch (Exception ex)//other exception
        {
            Console.WriteLine("Error Message: {0}", ex);
        }
    }
    for (int i = 0; i < noOfToppings; i++) //asking for each type of toppings based on the numbers of topping customer want
    {
        while (true)
        {
            bool ToppingInList = false;
            bool exists = false;
            Console.Write("Enter topping type: ");
            string toppingType = Console.ReadLine();
            foreach (string t in toppingOptions)
            {
                if (t.ToLower() == toppingType.ToLower())
                {
                    ToppingInList = true;
                }
            }
            if (!ToppingInList)
            {
                Console.WriteLine("Enter a valid topping");
                continue;
            }
            foreach (Topping t in newToppingList)
            {
                if (toppingType.ToLower() == t.Type.ToLower()) //one topping type can only be chosen once
                {
                    Console.WriteLine("Topping already exists.");
                    exists = true;
                }
            }
            foreach (string t in toppingOptions)
            {
                if (t.ToLower() == toppingType.ToLower())
                {
                    toppingType = t;
                }
            }
            Topping topping = new Topping(toppingType); //creating new topping object
            if (exists == false)
            {
                newToppingList.Add(topping); break; //adding to list
            }
        }
    }
    //creating new ice cream object based on the ice cream objects chosen
    if (iceCreamOption == "Cone")
    {
        IceCream newIce = new Cone(iceCreamOption, noOfScoops, newFlavoursList, newToppingList, isDipped); return newIce;
    }
    else if (iceCreamOption == "Waffle")
    {
        IceCream newIce = new Waffle(iceCreamOption, noOfScoops, newFlavoursList, newToppingList, wFlavour); return newIce;
    }
    else
    {
        IceCream newIce = new Cup(iceCreamOption, noOfScoops, newFlavoursList, newToppingList);
        return newIce;
    }
}
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  Display Order details of a customer ( Feature 5 )
    void OrderDetailsCustomer(Dictionary<int, Customer> customerDict)
{
    Customer wantedCustomer = printCustomers(customerDict); // Print customers first 
    Console.WriteLine("==========Current and Past Order Details==========");
    Console.WriteLine("Current Orders: ");
    if (wantedCustomer.CurrentOrder != null && wantedCustomer.CurrentOrder.iceCreamList.Count != 0) // Check that the currentOrder is not null.
    {
        string s = "";
        foreach (IceCream ice in wantedCustomer.CurrentOrder.iceCreamList)
        {
            s = FormattingPrint(ice); // Use generalised method to format the string
            Console.WriteLine("ID: {0,-5} Time Received: {1,-22} Time Fulfilled: {2,-22}{3,-22}", wantedCustomer.CurrentOrder.Id, wantedCustomer.CurrentOrder.TimeReceived, wantedCustomer.CurrentOrder.TimeFulfilled, s);
            Console.WriteLine();
        }
    }
    else
    {
        Console.WriteLine("Customer has no current orders");
    }


    Console.WriteLine("Past Orders: ");
    if (wantedCustomer.orderHistory.Count == 0) /// No order history means no past orders
    {
        Console.WriteLine("Customer has no past orders");
    }
    else
    {
        string c = "";
        foreach (Order o in wantedCustomer.orderHistory)
        {

            foreach (IceCream ice in o.iceCreamList)
            {
                c = FormattingPrint(ice);// Use generalised method to format the string
                Console.WriteLine("ID: {0,-5} Time Received: {1,-22} Time Fulfilled: {2,-22}{3,-22}", o.Id, o.TimeReceived, o.TimeFulfilled, c);
                Console.WriteLine();
            }
        }
    }

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  Getting index for option 6, modify ice cream
int printSelected(Customer wantedCustomer)
{
    List<IceCream> iceList = new List<IceCream>();
    try
    {
        if (wantedCustomer != null && wantedCustomer.CurrentOrder != null && wantedCustomer.CurrentOrder.iceCreamList != null)// Check all 3 in case any of them is null. The order is important.
        {
            iceList = wantedCustomer.CurrentOrder.iceCreamList; // Extract the iceCreamList of the wanted customer 
        }
        else
        {
            Console.WriteLine("No Current order.");
            return -1;
        }
    }
    catch(NullReferenceException)
    {
        Console.WriteLine("No Current Order.");
        return -1;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error Message: {0}", ex);
        return -1;
    }
    if (iceList.Count == 0)
    {
        if (wantedCustomer.orderHistory.Count == 0)
        {
            Console.WriteLine("Customer has no orders");
            return -1;
        }
    }

    int count = 1;
    string TimeFulfilled = "";
    Console.WriteLine("=================================Current Orders==========================================");
    foreach (IceCream ice in iceList)
    {
        string s = "";
        s += FormattingPrint(ice); // use generalised method to format string 
        if (string.IsNullOrEmpty(Convert.ToString(wantedCustomer.CurrentOrder.TimeFulfilled))) // Check if TimeFulfilled is null.
        {
            TimeFulfilled = "Unfulfilled";
            Console.WriteLine("[{0}] ID :{1,-5} Time Received: {2,-22} Time Fulfilled: {3,-22}{4,-22}", count,wantedCustomer.CurrentOrder.Id, wantedCustomer.CurrentOrder.TimeReceived, TimeFulfilled, s); // Print details along with index
        }
        else
        {
            Console.WriteLine("[{0} ID :{1,-5} Time Received: {2,-22} Time Fulfilled: {3,-22}{4,-22}", count,wantedCustomer.CurrentOrder.Id, wantedCustomer.CurrentOrder.TimeReceived, wantedCustomer.CurrentOrder.TimeFulfilled, s); // Print details along with index
        }
        Console.WriteLine();
        count++;
    }
    int index = -1;
    while (true)
    {
        try
        {
            Console.Write("Which Order to Modify?: ");
            index = Convert.ToInt32(Console.ReadLine()); // Get the index of the order they want
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Enter a valid order;");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Message: {0}", ex);
            ;
        }
    }
    return index;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Advanced Features 
//Feature a

//Process order and checkout
void Checkout(Dictionary<int, Customer> customerDict, Queue<Order> GoldQueue, Queue<Order> RegularQueue)
{
    try
    {
        //to dequeue order based on the queue a customer is in
        Order iceCreamOrder = DequeueOrder(GoldQueue, RegularQueue);
        if (iceCreamOrder != null)
        {
            //displaying the ice cream in the order and showing 
            double totalPrice = DisplayOrder(iceCreamOrder);
            bool birthdayPromoGiven = false; //checking if birthday promotion has already been given
            foreach (Customer customer in customerDict.Values)
            {
                if (customer.CurrentOrder != null && customer.CurrentOrder.Id == iceCreamOrder.Id) //working with customer with the same order id
                {
                    // displaying customer information
                    Console.WriteLine("Customer's Name: {0}", customer.Name);
                    Console.WriteLine("Membership Status: {0}", customer.Rewards.Tier);
                    Console.WriteLine("Membership Points: {0}", customer.Rewards.Points);
                    //checking whether to apply birthday promo
                    if (customer.Dob.Month == DateTime.Now.Month && customer.Dob.Day == DateTime.Now.Day && !birthdayPromoGiven)
                    {
                        totalPrice = BirthdayPromo(customer, iceCreamOrder, totalPrice);
                        birthdayPromoGiven = true;
                    }
                    //incrementing and applying promotion if punch card is full
                    totalPrice = PunchCardPromo(customer, iceCreamOrder, totalPrice);
                    if (totalPrice > 0) //if the total price is not 0
                    {
                        //customer will be able to redeem points 
                        totalPrice = PointsRedemption(customer, totalPrice);
                    }
                    //making payment and adding time fulfilled
                    MakePayment(customer, totalPrice);
                    string fileName = "orderPrice.csv";
                    if (!File.Exists(fileName))
                    {
                        using (StreamWriter writer = new StreamWriter(fileName, false))//creating new csv file
                        {
                            writer.WriteLine("{0},{1}", "Order ID", "Total Price");
                            writer.WriteLine("{0},{1}", customer.CurrentOrder.Id, totalPrice); //adding id and total price
                        }
                    }
                    else
                    {
                        using StreamWriter writer = new StreamWriter(fileName, true); //if csv file already created
                        {
                            writer.WriteLine("{0},{1}", customer.CurrentOrder.Id, totalPrice);
                        }
                    }
                    //adding the order to order history
                    customer.orderHistory.Add(iceCreamOrder);
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        //update order.csv
        UpdateOrderCSV(orders, customerDict);
    }
    catch (Exception e) //capturing any exception
    {
        Console.WriteLine($"An error occurred. {e.Message}");
    }

}

Order DequeueOrder(Queue<Order> GoldQueue, Queue<Order> RegularQueue)
{
    Order iceCreamOrder = null;
    if (GoldQueue.Count != 0) //dequeuing gold queue
    {
        iceCreamOrder = GoldQueue.Dequeue();
    }
    else if (GoldQueue.Count == 0 && RegularQueue.Count != 0) //dequeuing regular queue
    {
        iceCreamOrder = RegularQueue.Dequeue();
    }
    else
    {
        Console.WriteLine("There are no orders currently.");
    }
    return iceCreamOrder;
}

static double DisplayOrder(Order iceCreamOrder)
{
    double totalPrice = 0;
    Console.WriteLine("Ice Cream(s) in Order: ");
    foreach (IceCream ice in iceCreamOrder.iceCreamList) //displaying ice cream in the order
    {
        Console.WriteLine(ice);
        totalPrice += ice.CalculatePrice();
    }
    Console.WriteLine("The total amount is: ${0}", totalPrice); //display initial total price
    return totalPrice;
}

static double BirthdayPromo(Customer customer, Order iceCreamOrder, double totalPrice)
{
    double highestPrice = 0;

    Console.WriteLine("Birthday Promotion Valid.");

    foreach (IceCream ice in iceCreamOrder.iceCreamList) //comparing prices to find the highest priced ice cream
    {
        if (ice.CalculatePrice() > highestPrice)
        {
            highestPrice = ice.CalculatePrice();
        }
    }
    totalPrice -= highestPrice;//deducting highest priced ice creamm from the total price
    Console.WriteLine("Total Amount (after birthday promotion): ${0}", totalPrice);

    return totalPrice;
}

static double PunchCardPromo(Customer customer, Order iceCreamOrder, double totalPrice)
{
    int memberPunchCard = customer.Rewards.PunchCards;
    Console.WriteLine("Current Punch Card value: {0}", memberPunchCard);
    if (memberPunchCard < 10) //incrementing the punchcard 
    {
        memberPunchCard++;
        Console.WriteLine("New Punch Card value: {0}", memberPunchCard);
    }
    else if (memberPunchCard == 10)
    {
        memberPunchCard = 0;//reseting punch card
        double firstIceCream = iceCreamOrder.iceCreamList[0].CalculatePrice(); //the first ice cream given free
        totalPrice -= firstIceCream; //deducting the first ice cream price
        Console.WriteLine("Punch Card is completed and has been reset.");
        Console.WriteLine("Total Amount (after completion of Punch Card): ${0}", totalPrice);
    }
    customer.Rewards.PunchCards = memberPunchCard; //updating the punch card
    return totalPrice;
}

static double PointsRedemption(Customer customer, double totalPrice)
{
    string membershipStatus = customer.Rewards.Tier;
    int membershipPoints = customer.Rewards.Points;
    if (membershipStatus == "Silver" || membershipStatus == "Gold") //validation for redeemption of points for silver and above
    {
        Console.WriteLine("Redemption of points is valid.");
        Console.Write("Enter the numbers of points to redeem: ");
        int redeemPoints = Convert.ToInt32(Console.ReadLine());

        if (redeemPoints > 0)
        {
            if (redeemPoints > membershipPoints) //if points not sufficient
            {
                Console.WriteLine("Points insufficient.");
            }
            else
            {
                membershipPoints -= redeemPoints; //deducting the points after redemption
                Console.WriteLine("Membership Points (after redemption): {0}", membershipPoints);
                totalPrice -= redeemPoints * 0.02; //applying discount after redemption of points
                Console.WriteLine("Total Amount (after Points redemption): ${0}", totalPrice);
            }
            customer.Rewards.Points = membershipPoints; //updating membership Points
        }

        Console.WriteLine("Membership status (before): {0}", membershipStatus);
        //updating the membership status
        if (membershipStatus != "Gold")
        {
            if (membershipPoints >= 100)
            {
                membershipStatus = "Gold";
            }
            else if (membershipPoints >= 50)
            {
                membershipStatus = "Silver";
            }
        }
        Console.WriteLine("Membership status (after): {0}", membershipStatus);
    }
    else
    {
        Console.WriteLine("Redemption of points is not valid");
    }
    return totalPrice;
}

void MakePayment(Customer customer, double totalPrice)
{
    Console.Write("Enter any key to make payment: ");
    string payKey = Console.ReadLine();

    if (payKey != null)
    {
        //adding points to customer 
        int pointsToEarn = Convert.ToInt32(Math.Floor(totalPrice * 0.72));
        customer.Rewards.AddPoints(pointsToEarn);

        //setting the timeFulfilled of order for the current order
        DateTime timeFulfilled = DateTime.Now;
        customer.CurrentOrder.TimeFulfilled = timeFulfilled;
    }

    Console.WriteLine("Payment successful!");
}




//////////////




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  Display monthly charged amounts breakdown & total charged amounts for the year
//////////////////////////////////////////////////////// Feature b)

void ReadingOrderPrice(Dictionary<int, double> FinalPrice) // Reading the file which has Order ID : final price.
{
    string line;
    bool skip_first_line = false;
    using StreamReader sr = new StreamReader("orderPrice.csv");
    {
        while ((line = sr.ReadLine()) != null)
        {
            if (!skip_first_line)
            {
                continue;
            }

            string[] data = line.Split(',');
            FinalPrice.Add(Convert.ToInt32(data[0]), Convert.ToDouble(data[1])); // Add the ID , Price to dictionary to use later
        }
    }
}


void DisplayMonthlyCharges(List<Order> orders)
{
    Dictionary<int, double> FinalPrice = new Dictionary<int, double>(); // Dictionary to used to ID and corresponding price inside the ordersPrice.csv
    string[] Months = { "Jan", "Feb", "Mar", "Apr","May", "June", "July","Aug","Sep", "Oct", "Nov", "Dec" }; // Array of months 
    Dictionary<string, double> MonthlyCharges = new Dictionary<string, double>();// Dictionary to used to Store Months and corresponding revenue.
    foreach (string M in Months) // Looping through the months array
    {
        MonthlyCharges.Add(M, 0); // Creating a default dictionary {Jan : 0} ... 
    }
    while (true)
    {
        try
        {
            Console.Write("Enter the year: ");
            int year = Convert.ToInt32(Console.ReadLine()); 
            if (File.Exists("ordersPrice.csv")) // Checking of the ordersPrice csv exists.
            {
                ReadingOrderPrice(FinalPrice); // if it does call it and updated the FinalPrice Dict
            }

            foreach (Order o in orders)
            {
                if (o.TimeFulfilled.Value.Year == year) // Check the year the order was fulfilled
                {
                    bool Exists = false;
                    string Month = Months[o.TimeFulfilled.Value.Month - 1]; // Extracting the Month that it happened. if its jan, [o.TimeFulfilled.Value.Month returns 1. So - 1 will become 0.
                    {
                        if (FinalPrice.Keys.Contains(o.Id)) // Checking if the FinalPrice dictionary key contains the ID. If it use the the price there instead
                        {
                            MonthlyCharges[Month] += FinalPrice[o.Id];
                        }
                        else // If not , calculate the revenue using the method
                        {
                            double revenue = o.CalculateTotal();
                            MonthlyCharges[Month] += revenue;
                        }
                    }

                }
            }
            double Total = 0;
            foreach (KeyValuePair<string, double> kvp in MonthlyCharges)
            {
                foreach (string s in Months)
                {
                    if (s == kvp.Key)
                    {
                        Total += kvp.Value;
                        Console.WriteLine("{0,-5}{1}:  ${2}", kvp.Key, year, kvp.Value); // printing 
                        break;
                    }
                }
            }
            Console.WriteLine("Total: ${0}", Total);
            return;
        }
        catch (FormatException)
        {
            Console.WriteLine("Enter a valid year.");
        }
    }
}





//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
InitialiseCustomers(customerDict, orders);
InitaliseOrder(customerDict);
InitaliseFlavour(FlavoursFile);
InitaliseToppings(ToppingsFile);


int option = -1;
while (true)
{
    Menu();
    while (true)
    {
        try
        {
            Console.Write("Enter your option: ");
            option = Convert.ToInt32(Console.ReadLine());
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Enter a valid number.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Message: {0}", ex);
;           }
    }
    Console.WriteLine();
    if (option == 0)
    {
        UpdateCustomerCSV(customerDict); // When press 0 update the customer.csv
        Console.WriteLine("Exited");
        break;
    }
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
            CreateCustomerOrder(customerDict, orders, FlavoursFile, ToppingsFile);
            break;
        case 5:
            OrderDetailsCustomer(customerDict);
            break;
        case 6:

            Customer wantedCustomer = printCustomers(customerDict);
            int modIceCreamChoice = -1;
            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("[1]Choose an existing ice cream object to modify.");
                    Console.WriteLine("[2]Add an entirely new ice cream object to the order");
                    Console.WriteLine("[3]Choose an existing ice cream object to delete from the order");
                    Console.Write("Choose an option: ");
                    modIceCreamChoice = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch(FormatException)
                {
                    Console.WriteLine("Enter a valid option.\n");
                }
            }

            int index = -1;
            bool Updated = false;
            switch (modIceCreamChoice)
            {
                case 1:
                    index = printSelected(wantedCustomer); // print 
                    if (index == -1)
                    {
                        break;
                    }
                    wantedCustomer.CurrentOrder.ModifyIceCream(index);// CAll the mpdofu ice cream method from class.
                    Updated = true;
                    break;
                case 2:
                    if (wantedCustomer != null && wantedCustomer.CurrentOrder != null && wantedCustomer.CurrentOrder.iceCreamList != null) // Have to check if its not null
                    {

                        IceCream newIce = IceCreamOptionChoice(FlavoursFile, ToppingsFile, wantedCustomer.CurrentOrder); // Call the creating of ice Cream method
                        wantedCustomer.CurrentOrder.AddIceCream(newIce); // use the class method to add to currentOrder IceCream list
                        Updated = true;
                    }
                    else
                    {
                        Console.WriteLine("No Current Order to add ice cream to.\n");
                    }
                    break;
                case 3:
                    index = printSelected(wantedCustomer);
                    if (index == -1)
                    {
                        break;
                    }
                    if (wantedCustomer.CurrentOrder.iceCreamList.Count() == 1) // Check whetehr can delete
                    {
                        Console.WriteLine("Cannot have zero ice creams in an order");
                    }
                    else
                    {
                        wantedCustomer.CurrentOrder.DeleteIceCream(index); // Call the method from class to delete iceCream from current order ice cream list.
                        Updated = true;
                    }
                    break;
                default:
                    Console.WriteLine("Enter a valid modification.");
                    break;
            }
            Console.WriteLine();
            if (Updated)
            {
                Console.WriteLine("=============================================================================================Updated Order=============================================================================================");
                foreach (IceCream ice in wantedCustomer.CurrentOrder.iceCreamList) // print new order details
                {
                    string s = FormattingPrint(ice); // use generalised method to get string
                    if (string.IsNullOrEmpty(Convert.ToString(wantedCustomer.CurrentOrder.TimeFulfilled)))
                    {
                        string TimeFulfilled = "Unfulfilled";
                        Console.WriteLine("ID :{0,-5} Time Received: {1,-22} Time Fulfilled: {2,-22}{3,-22}", wantedCustomer.CurrentOrder.Id, wantedCustomer.CurrentOrder.TimeReceived, TimeFulfilled, s);
                    }
                    else
                    {
                        Console.WriteLine("ID :{0,-5} Time Received: {1,-22} Time Fulfilled: {2,-22}{3,-22}", wantedCustomer.CurrentOrder.Id, wantedCustomer.CurrentOrder.TimeReceived, wantedCustomer.CurrentOrder.TimeFulfilled, s);
                    }
                }
                int OrdersIndex = -1;
                foreach (Order o in orders)
                {
                    if (o.Id == wantedCustomer.CurrentOrder.Id)
                    {
                        OrdersIndex = orders.IndexOf(o); // getting index of the old order that got modified
                        break;
                    }
                }
                if (OrdersIndex != -1)
                {
                    orders[OrdersIndex] = wantedCustomer.CurrentOrder; // Updating main order list with the old order to the new order that got modified
                }
            }
            break; 
        case 7:
            Checkout(customerDict, GoldQueue, RegularQueue);
            break;
        case 8:
            DisplayMonthlyCharges(orders);
            break;
        default:
            Console.WriteLine("Give a valid option.\n");
            break;

       }
}

