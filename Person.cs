using System;
using System.Collections.Generic;
using System.Text;

namespace TjuvOchPolis
{
    class Person
    {
        private static int seed = 2;
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int DirectionX { get; set; }
        public int DirectionY { get; set; }

        public Person() // Sets Startion Position and direction to move in (There is a chance that multiple Persons will share the same cordinates and directions) 
        {
            Random random = new Random();
            while (DirectionX == 0 && DirectionY == 0)
            {
                DirectionX = random.Next(-1, 2);
                DirectionY = random.Next(-1, 2);
            }

            PosX = random.Next(0, 100);
            PosY = random.Next(0, 25);
            seed++;
        }
        public void Move()
        {
            PosX += DirectionX;
            if (PosX >= 100)
            {
                PosX = 0;
            }
            else if (PosX < 0)
            {
                PosX = 99;
            }
            PosY += DirectionY;
            if (PosY >= 25)
            {
                PosY = 0;
            }
            else if (PosY < 0)
            {
                PosY = 24;
            }
        }
    }
}
