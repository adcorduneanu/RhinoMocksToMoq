namespace Rhino.Mocks
{
    using System;
    using System.Linq.Expressions;
    using Moq;

    public interface IExpect<T> where T : class
    {
        IRepeat<T> Repeat { get; }

        void Throw(Exception exception);

        IExpect<T> SetupWithMoq();
              
        IExpect<T> IgnoreArguments();
    }

    public interface IExpect<T, TR> where T : class
    {
        IRepeat<T, TR> Repeat { get; }

        IExpect<T, TR> Do(TR result);

        IExpect<T, TR> Return(TR result);

        void ReturnInOrder(params TR[] results);

        void Throw(Exception exception);

        IExpect<T, TR> SetupWithMoq();

        IExpect<T, TR> IgnoreArguments();
    }

    public sealed  class Expect<T, TR> : IExpect<T, TR> where T : class
    {
        private readonly MoqAdapter<T, TR> moqAdapter;

        private Repeat<T, TR> repeat;

        private TR result;

        private bool isResultAssigned;

        public Expect(Mock<T> mock, Expression<Func<T, TR>> expression)
        {
            this.moqAdapter = new MoqAdapter<T, TR>(mock, expression);
        }

        public IExpect<T, TR> Return(TR result)
        {
            if (this.isResultAssigned)
            {
                throw new InvalidOperationException("Return should be setup only once");
            }

            this.isResultAssigned = true;
            this.result = result;

            return this.SetupWithMoq();
        }

        public void Throw(Exception exception)
        {
            this.moqAdapter.Throws(exception);
        }

        public IExpect<T, TR> SetupWithMoq()
        {
            this.moqAdapter.Setup(this.result, this.repeat);

            return this;
        }

        public void ReturnInOrder(params TR[] results)
        {
            this.moqAdapter.SetupReturnInOrder(results);
        }

        public IExpect<T, TR> IgnoreArguments()
        {
            this.moqAdapter.IgnoreArguments();

            return this;
        }

        public IExpect<T, TR> Do(TR result)
        {
            this.Return(result);

            return this;
        }

        public IRepeat<T, TR> Repeat
        {
            get
            {
                if (this.repeat != null)
                {
                    throw new InvalidOperationException("Repeat should be setup only once");
                }

                return this.repeat = new Repeat<T, TR>(this);
            }
        }
    }

    public sealed class Expect<T> : IExpect<T> where T : class
    {
        private readonly MoqAdapter<T> moqAdapter;

        private Repeat<T> repeat;

        public Expect(Mock<T> mock, Expression<Action<T>> expression)
        {
            this.moqAdapter = new MoqAdapter<T>(mock, expression);
        }

        public void Throw(Exception exception)
        {
            this.moqAdapter.Throws(exception);
        }

        public IExpect<T> SetupWithMoq()
        {
            this.moqAdapter.Setup(this.repeat);

            return this;
        }

        public IExpect<T> IgnoreArguments()
        {
            this.moqAdapter.IgnoreArguments();

            return this;
        }

        public IRepeat<T> Repeat
        {
            get
            {
                if (this.repeat != null)
                {
                    throw new InvalidOperationException("Repeat should setup only once");
                }

                return this.repeat = new Repeat<T>(this);
            }
        }
    }
}
