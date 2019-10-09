using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Projekt
{
    class Stack
    {
        public int Value { get; private set; }
        public int StackSize { get; private set; }
        int[] Numbers;

        public Stack()
        {
            Value = 0;
            StackSize = 0;
        }

        public void Push(int value)
        {
            int auxStackSize = StackSize + 1;
            int[] auxNumbers = new int[auxStackSize];

            auxNumbers[0] = value;

            for (int i = 1; i < auxStackSize; i++)
            {
                auxNumbers[i] = Numbers[i - 1];
            }

            StackSize = auxStackSize;
            Numbers = auxNumbers;
        }

        public void Pop()
        {
            int auxStackSize = StackSize - 1;
            int[] auxNumbers = new int[auxStackSize];

            for (int i = 0; i < auxStackSize; i++)
            {
                auxNumbers[i] = Numbers[i + 1];
            }

            StackSize = auxStackSize;
            Numbers = auxNumbers;
        }

        public bool Empty()
        {
            if (StackSize == 0) return true;
            return false;
        }

        public int Top()
        {
            return Numbers[0];
        }

        public int Bottom()
        {
            if (StackSize > 0) return Numbers[StackSize - 1];
            else return -1;
        }

        public int Position(int index)
        {
            return Numbers[index];
        }

        public void Display()
        {
            for (int i = StackSize - 1; i >= 0; i--)
            {
                Console.Write(Numbers[i] + " -> ");
            }
        }
    }
}
