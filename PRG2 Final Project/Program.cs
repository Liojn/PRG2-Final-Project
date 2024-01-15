using PRG2_Final_Project;

List<Customer> customerList = new List<Customer>();

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

///Feature 2: List all Current Orders



