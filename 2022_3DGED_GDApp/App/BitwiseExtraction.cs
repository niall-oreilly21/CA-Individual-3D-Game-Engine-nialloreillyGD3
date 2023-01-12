using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
 public class BitwiseExtraction
{
    //Extracts k bits from a given number, beginning at position p
    //See: https://www.geeksforgeeks.org/extract-k-bits-given-position-number/
    public static int extractKBitsFromNumberAtPositionP(int number, int k, int p)
    {
        return (((1 << k) - 1) & (number >> (p)));
    }
}
}
