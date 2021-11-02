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

        IExpect<T, TR> Return(TR result);

        void ReturnInOrder(params TR[] results);

        void Throw(Exception exception);
        
        IExpect<T, TR> SetupWithMoq();

        IExpect<T, TR> IgnoreArguments();
    }

    public class Expect<T, TR> : IExpect<T, TR> where T : class
    {
        private readonly MoqAdapter<T, TR> _moqAdapter;
        
        private Repeat<T, TR> _repeat;

        private TR _result;

        private bool _isResultAssigned;

        public Expect(Mock<T> mock, Expression<Func<T, TR>> expression)
        {
            this._moqAdapter = new MoqAdapter<T, TR>(mock, expression);
        }

        public IExpect<T, TR> Return(TR result)
        {
            if (this._isResultAssigned)
            {
                throw new InvalidOperationException("Return should be setup only once");
            }

            this._isResultAssigned = true;
            this._result = result;

            return this.SetupWithMoq();
        }

        public void Throw(Exception exception)
        {
            this._moqAdapter.Throws(exception);
        }

        public IExpect<T, TR> SetupWithMoq()
        {
            this._moqAdapter.Setup(this._result, this._repeat);

            return this;
        }

        public void ReturnInOrder(params TR[] results)
        {
            this._moqAdapter.SetupReturnInOrder(results);
        }

        public IExpect<T, TR> IgnoreArguments()
        {
            this._moqAdapter.IgnoreArguments();

            return this;
        }

        public IRepeat<T, TR> Repeat
        {
            get
            {
                if (this._repeat != null)
                {
                    throw new InvalidOperationException("Repeat should be setup only once");
                }

                return this._repeat = new Repeat<T, TR>(this);
            }
        }
    }

    public class Expect<T> : IExpect<T> where T : class
    {
        private readonly MoqAdapter<T> _moqAdapter;

        private Repeat<T> _repeat;

        public Expect(Mock<T> mock, Expression<Action<T>> expression)
        {
            this._moqAdapter = new MoqAdapter<T>(mock, expression);
        }

        public void Throw(Exception exception)
        {
            this._moqAdapter.Throws(exception);
        }

        public IExpect<T> SetupWithMoq()
        {
            this._moqAdapter.Setup(this._repeat);
            return this;
        }

        public IExpect<T> IgnoreArguments()
        {
            this._moqAdapter.IgnoreArguments();

            return this;
        }

        public IRepeat<T> Repeat
        {
            get
            {
                if (this._repeat != null)
                {
                    throw new InvalidOperationException("Repeat should setup only once");
                }
                return this._repeat = new Repeat<T>(this);
            }
        }
    }
}
