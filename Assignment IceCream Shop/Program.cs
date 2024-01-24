// See https://aka.ms/new-console-template for more information

using Assignment_IceCream_Shop;

List<Customer> customerList = new List<Customer>();
List<Flavour> flavourList = new List<Flavour>();
List<Topping> toppingList = new List<Topping>();

InitialiseCustomers(customerList);
ListCustomers(customerList);
RegisterNewCustomers(customerList);
void InitialiseCustomers(List<Customer> customerList)
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
                DateTime dob = Convert.ToDateTime(data[2]);
                
                Customer customer = new Customer(customerName, customerID, dob);
                customerList.Add(customer);
            }
        }
    };
} 

void ListCustomers(List<Customer> customerList)
{
    Console.WriteLine("CUSTOMER INFORMATION");
    Console.WriteLine("{0, -10} {1, 15} {2, 15}", "Name", "Membership ID", "Date of Birth");
    for (int i = 0; i < customerList.Count; i++)
    {
        Console.WriteLine("{0, -10} {1, 15} {2, 15}", customerList[i].Name, customerList[i].MemberId, customerList[i].Dob.ToString("dd/MM/yyyy"));
    }
}

void RegisterNewCustomers(List<Customer> customerList)
{
    Console.Write("Enter customer's name: ");
    string name = Console.ReadLine();
    Console.Write("Enter ID Number: ");
    int memberId = Convert.ToInt32(Console.ReadLine());
    Console.Write("Enter Date-of-Birth: ");
    DateTime dob = Convert.ToDateTime(Console.ReadLine());

    Customer newCustomer = new Customer(name, memberId, dob);
    customerList.Add(newCustomer);

    PointCard newPointCard = new PointCard(0, 0);
    newCustomer.Rewards = newPointCard;

    Console.WriteLine("Registration successful!");
}

