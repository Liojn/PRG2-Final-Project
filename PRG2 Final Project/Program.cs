using PRG2_Final_Project;
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
                Console.WriteLine(dob.ToString("dd/MM/yyyy"));
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

void InitaliseOrder(Queue<Order> RegularQueue, Queue<Order> GoldQueue, Dictionary<int, Customer> customerDict)
{
    List<Flavour> flavourList = new List<Flavour>();
    List<Topping> toppingsList = new List<Topping>();

    string line;
    int count = 0;
    using StreamReader sr = new StreamReader("orders.csv");
    {
        while ((line = sr.ReadLine()) != null)
        {
            if (count == 0)
            {
                count++;
            }
            else
            {
                //27/10/2023 13:28
                string[] data = line.Split(",");

                //Extracting Data
                int id = Convert.ToInt32(data[0]);
                int MemberID = Convert.ToInt32(data[1]);
                string Option = data[4];
                int Scoops = Convert.ToInt32(data[5]);
                DateTime TimeRecevied = DateTime.ParseExact(data[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime TimeFulfilled  = DateTime.ParseExact(data[3], "dd/MM/yyyy HH:mm",CultureInfo.InvariantCulture);
                bool premium = false;
                string waffleFlavour = data[7];
                bool dippedCone = Convert.ToBoolean(data[6].ToLower());


                Order order = new Order(id, TimeRecevied);
                order.TimeFulfilled = TimeFulfilled;
                for (int i = 8; i < 10; i++)
                {
                    string Flavour = data[i];
                    if (Flavour != null)
                    {
                        if (Flavour == "Durian" || Flavour == "Ube" || Flavour == "Sea Salt")
                        {
                            premium = true;
                        }

                        foreach (Flavour f in flavourList)
                        {
                            if (f.Type == Flavour)
                            {
                                f.Quantity += 1;
                            }
                            else
                            {
                                Flavour newFlavour = new Flavour(Flavour, premium, 1);
                                flavourList.Add(newFlavour);
                            }
                        }
;                    }
                }
                for (int j = 11; j < 14; j++)
                {
                    string Topping = data[j];
                    if (Topping != null)
                    {
                        Topping top = new Topping(Topping);
                        toppingsList.Add(top);
                    }
                }


                if (Option == "Waffle")
                {
                    IceCream ice = new Waffle(Option, Scoops, flavourList, toppingsList, waffleFlavour);
                    order.iceCreamList.Add(ice);
                }
                else if (Option == "Cone")
                {
                    IceCream ice = new Cone(Option, Scoops, flavourList, toppingsList, dippedCone);
                    order.iceCreamList.Add(ice);
                }
                else
                {
                    IceCream ice = new Cup(Option, Scoops, flavourList, toppingsList);
                    order.iceCreamList.Add(ice);
                }

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
                }

            }
        }
    }
}


void Menu()
{
    Console.WriteLine("[1]List all customers.");
    Console.WriteLine("[2]List all current orders.");
    Console.WriteLine("[3]Register a new customer.");
    Console.WriteLine("[4]Create a new customer's order.");
    Console.WriteLine("[5]Display order details of a customer".);
    Console.WriteLine("[6]Modify Order Detail.");
}

//List Curren orders

void ListCurrentOrders(Queue<Order> GoldQueue, Queue<Order> RegularQueue)
{
    Console.WriteLine("Gold Queue Orders: ");
    Console.WriteLine("{:<20}{:<20}{:<20}", "Time Received", "Time Fulfilled", "Ice Cream Details");
    foreach (Order o in GoldQueue)
    {
    }

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
            break;
    }
}


