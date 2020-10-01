using System;
using System.Collections.Generic;
using System.Text;

namespace TjuvOchPolis
{
    class Cop : Person
    {
        public Inventory ConfiscatedItems = new Inventory();
        public Cop()
        {
            ConfiscatedItems = new Inventory();
        }
    }
}
