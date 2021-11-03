using System.Collections.Generic;

namespace RhinoMocksToMoq
{
    public interface ICalculator
    {
        void Reset();

        int Add(int? a, int b);

        int Random();

        void Output(int a, int b);

        int SumAll(List<int> args);
    }
}
