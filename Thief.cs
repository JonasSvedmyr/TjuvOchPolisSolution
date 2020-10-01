using System;
using System.Collections.Generic;
using System.Text;

namespace TjuvOchPolis
{
    class Thief: Person
    {
        public Inventory StolenGoods = new Inventory();
        public int LokeckedUp { get; set; }
        public Thief()
        {
            StolenGoods = new Inventory();
        }
    }
}
