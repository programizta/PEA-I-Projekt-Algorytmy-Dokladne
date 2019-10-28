using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Projekt
{
    /// <summary>
    /// klasa, która jest "optymalizacją pamięciową"
    /// dla programowania dynamicznego
    /// </summary>
    class PartialTravel
    {
        public int CurrentVertexIndex { get; set; }

        public int CurrentVerticesMask { get; set; }

        public int Value { get; set; }

        public PartialTravel(int currentVertexIndex, int currentVerticesMask, int value)
        {
            CurrentVertexIndex = currentVertexIndex;
            CurrentVerticesMask = currentVerticesMask;
            Value = value;
        }
    }
}
