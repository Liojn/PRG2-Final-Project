using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Project
{
    class Customer
    {
        private string name;
        private int memberId;
        private DateTime dob;

        public List<Order> orderHistory { get; set; } = new List<Order>();

        public string Name { get { return name; } set { name = value; } }
        public int MemberId { get { return memberId; } set { memberId = value; } }
        public DateTime Dob { get { return dob; } set { dob = value; } }

        public PointCard Rewards { get; set; }
        public Order CurrentOrder { get; set; }


        public Customer() { }

        public Customer(string n, int m, DateTime d)
        {
            CurrentOrder = null;
            Rewards = new PointCard(0,0);
            Name = n;
            MemberId = m;
            Dob = d;

        }

        public Order MakeOrder()
        {
            CurrentOrder = new Order(0, DateTime.Now);////Change the ID in the main program, use 0 as default
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            if (DateTime.Now.Month == dob.Month && DateTime.Now.Day == dob.Day)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            string orders = "";
            foreach (Order o in orderHistory)
            {
                orders += o.ToString + "\n";
            }
            return ("Name: " + Name + "\tMember ID: " + MemberId + "\tDate of Birth: " + dob.ToString("MM/dd/yyyy") + "\nRewards: " + Rewards + "\nCurrent Order: " + CurrentOrder + "\nOrder History: " + orders );


        }
    }
}
