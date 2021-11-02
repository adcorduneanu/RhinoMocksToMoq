namespace RhinoMocksToMoq
{
    using System.Diagnostics;

    internal sealed class CalculatorService : ICalculator
    {
        private readonly ICalculator calculator;

        public CalculatorService(ICalculator calculator)
        {
            this.calculator = calculator;
        }

        public int Add(int? a, int b)
        {
            return this.calculator.Add(a.Value, b);
        }

        public int Random()
        {
            return this.calculator.Random();
        }

        public void Reset()
        {
            this.calculator.Reset();
        }

        public void Output(int a, int b)
        {
            Trace.WriteLine($"a: {a}, b: {b}");
        }
    }
}
