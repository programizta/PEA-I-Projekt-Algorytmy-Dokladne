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
        public int[] numbersOnStack;

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
                auxNumbers[i] = numbersOnStack[i - 1];
            }

            StackSize = auxStackSize;
            numbersOnStack = auxNumbers;
        }

        public void Pop()
        {
            int auxStackSize = StackSize - 1;
            int[] auxNumbers = new int[auxStackSize];

            for (int i = 0; i < auxStackSize; i++)
            {
                auxNumbers[i] = numbersOnStack[i + 1];
            }

            StackSize = auxStackSize;
            numbersOnStack = auxNumbers;
        }

        public int Top()
        {
            return numbersOnStack[0];
        }

        public void Display()
        {
            for (int i = StackSize - 1; i >= 0; i--)
            {
                if (i > 0) Console.Write(numbersOnStack[i] + " -> ");
                else Console.Write(numbersOnStack[i]);
            }
        }
    }
}