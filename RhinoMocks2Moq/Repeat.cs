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
			this.Type = RepeatType.Any;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> Once()
        {
			this.Type = RepeatType.Once;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> Twice()
        {
			this.Type = RepeatType.Twice;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> AtLeastOnce()
        {
			this.Type = RepeatType.AtLeastOnce;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> Never()
        {
			this.Type = RepeatType.Never;
            return this.expect.SetupWithMoq();
        }

        public IExpect<T, TR> Times(int count)
        {
			this.Type = RepeatType.Exact;
			this.ExactCount = count;
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
			this.Type = RepeatType.Once;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> Twice()
        {
			this.Type = RepeatType.Twice;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> Any()
        {
			this.Type = RepeatType.Any;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> AtLeastOnce()
        {
			this.Type = RepeatType.AtLeastOnce;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> Never()
        {
			this.Type = RepeatType.Never;

            return this.expect.SetupWithMoq();
        }

        public IExpect<T> Times(int count)
        {
			this.Type = RepeatType.Exact;
			this.ExactCount = count;

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
