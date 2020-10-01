using System;
using System.Collections.Generic;
using System.Text;

namespace TjuvOchPolis
{
    class Citizen : Person
    {
        public bool HasClock { get; set; }
        public bool HasMoney { get; set; }
        public bool HasMoblephone { get; set; }
        public bool HasKeys { get; set; }
        public Citizen()
        {
            HasClock = true;
            HasMoblephone = true;
            HasMoney = true;
            HasKeys = true;
        }
    }
}
