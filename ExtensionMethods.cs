using System;
using System.Collections.Generic;
using System.Text;

namespace TjuvOchPolis
{
    public static class ExtensionMethods
    {
        public static char[,] Clear(this char[,] Array2D, int YLength, int XLength)
        {
            for (int y = 0; y < YLength; y++)
            {
                for (int x = 0; x < XLength; x++)
                {
                    Array2D[y, x] = ' ';
                }
            }
            return Array2D;
        }
    }
}
