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



        public int Id { get { return id; } set {  id = value; } }
        public DateTime TimeReceived { get {  return timeReceived; } set {  timeReceived = value; } }
        public DateTime? TimeFulfilled { get; set; }

        public List<IceCream> iceCreamList { get; set; } = new List<IceCream>();

        public Order() { }

        public Order(int i, DateTime TR, DateTime TF)
        {
            Id = i;
            TimeReceived = TR;
            TimeFulfilled = TF;
        }

        public void ModifyIceCream(int i)
        {

        }


    }
}
