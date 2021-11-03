namespace Rhino.Mocks
{
    using MoqTimes = Moq.Times;

    public enum RepeatType
    {
        Any,
        Once,
        Twice,
        AtLeastOnce,
        Never,
        Exact
    }

    public interface IRepeat<T> where T : class
    {
        IExpect<T> Any();

        IExpect<T> Once();

        IExpect<T> Twice();

        IExpect<T> AtLeastOnce();

        IExpect<T> Never();

        IExpect<T> Times(int count);
    }

    public interface IRepeat<T, TR> where T : class
    {
        IExpect<T, TR> Any();

        IExpect<T, TR> Once();

        IExpect<T, TR> Twice();

        IExpect<T, TR> AtLeastOnce();

        IExpect<T, TR> Never();

        IExpect<T, TR> Times(int count);
    }

    public sealed class Repeat<T, TR> : IRepeat<T, TR> where T : class
    {
        private readonly IExpect<T, TR> expect;

        public int ExactCount { get; private set; }

        public RepeatType Type { get; private set; }

        public Repeat(IExpect<T, TR> expect)
        {
            this.expect = expect;
        }

        public IExpect<T, TR> Any()
        {
            Type = RepeatType.Any;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> Once()
        {
            Type = RepeatType.Once;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> Twice()
        {
            Type = RepeatType.Twice;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> AtLeastOnce()
        {
            Type = RepeatType.AtLeastOnce;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> Never()
        {
            Type = RepeatType.Never;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> Times(int count)
        {
            Type = RepeatType.Exact;
            ExactCount = count;
            return this.expect.SetupWithMoq();
        }
    }

    public sealed class Repeat<T> : IRepeat<T> where T : class
    {
        private readonly IExpect<T> expect;

        public int ExactCount { get; private set; }

        public RepeatType Type { get; private set; }

        public Repeat(IExpect<T> expect)
        {
            this.expect = expect;
        }

        public IExpect<T> Once()
        {
            Type = RepeatType.Once;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> Twice()
        {
            Type = RepeatType.Twice;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> Any()
        {
            Type = RepeatType.Any;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> AtLeastOnce()
        {
            Type = RepeatType.AtLeastOnce;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> Never()
        {
            Type = RepeatType.Never;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> Times(int count)
        {
            Type = RepeatType.Exact;
            ExactCount = count;

            return this.expect.SetupWithMoq();
        }
    }

    public sealed class RepeatAdapter
    {
        public RepeatArg Repeat => new RepeatArg();

        public sealed class RepeatArg
        {
            public MoqTimes Once() => MoqTimes.Once();

            public MoqTimes Twice() => MoqTimes.Exactly(2);

            public MoqTimes AtLeastOnce() => MoqTimes.AtLeastOnce();

            public MoqTimes Never() => MoqTimes.Never();

            public MoqTimes Times(int count) => MoqTimes.Exactly(count);
        }
    }
}
