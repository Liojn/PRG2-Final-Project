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

        public Customer(string n, int m, DateTime d, PointCard r, Order c)
        {
            Name = n;
            MemberId = m;
            Dob = d;
            Rewards = r;
            CurrentOrder = c;
        }

        public Order MakeOrder()
        {
            CurrentOrder = new Order(MemberId, dob);
            return CurrentOrder;
        }

        public bool IsBirthday(DateTime date)
        {
            if (date == dob)
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
            foreach (Order o in orderHistory)
            {

            }
            return ("Name: " + Name + "\tMember ID: " + MemberId + "\tDate of Birth: " + dob.ToString("MM/dd/yyyy") + "\nRewards: " + Rewards + "\nCurrent Order: " + CurrentOrder + );


        }
    }
}
