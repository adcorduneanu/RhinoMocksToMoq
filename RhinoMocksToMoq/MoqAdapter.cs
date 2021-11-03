namespace Rhino.Mocks
{
	using System;
	using System.Linq.Expressions;
	using Moq;

	internal class MoqAdapter<T, TR> where T : class
    {
        private readonly Mock<T> mock;
        private Expression<Func<T, TR>> expression;

        public MoqAdapter(Mock<T> mock, Expression<Func<T, TR>> expression)
        {
            this.mock = mock;
            this.expression = expression;
        }

        public void Setup(TR result, Repeat<T, TR> repeat)
        {
            if (result != null)
            {
                if (repeat == null)
                {
                    this.mock.Setup(this.expression).Returns(result);
                }
                else
                {
                    switch (repeat.Type)
                    {
                        case RepeatType.Once:
                            this.mock.SetupSequence(this.expression).Returns(result).Throws(new Exception("Should call once"));

                            break;
                        case RepeatType.Twice:
                            this.mock.SetupSequence(this.expression).Returns(result).Returns(result).Throws(new Exception("Should call only twice"));

                            break;
                        case RepeatType.Any:
                            this.mock.Setup(this.expression).Returns(result);

                            break;
                        case RepeatType.AtLeastOnce:
                            this.mock.Setup(this.expression).Returns(result).Verifiable();

                            break;
                        case RepeatType.Never:
                            this.mock.Setup(this.expression).Throws(new Exception("Should never call"));

                            break;
                        case RepeatType.Exact:
                            var sequence = this.mock.SetupSequence(this.expression);
                            for (var i = 0; i < repeat.ExactCount; i++)
                            {
                                sequence = sequence.Returns(result);
                            }

                            sequence.Throws(new Exception($"Should call only {repeat.ExactCount} times"));

                            break;
                    }
                }
            }
        }
        
        public void SetupReturnInOrder(params TR[] results)
        {
            this.mock.Setup(this.expression).ReturnsInOrder(results);
        }

        public void Throws(Exception exception)
        {
            this.mock.Setup(this.expression).Throws(exception);
        }

        internal void IgnoreArguments()
        {
            this.expression = new ArgumentsAdapter<T, TR>().IgnoreArguments(this.expression);
        }
    }

    internal class MoqAdapter<T> where T : class
    {
        private readonly Mock<T> mock;
        private Expression<Action<T>> expression;

        public MoqAdapter(Mock<T> mock, Expression<Action<T>> expression)
        {
            this.mock = mock;
            this.expression = expression;
        }

        public void Setup(Repeat<T> repeat)
        {
            if (repeat == null)
            {
                mock.Setup(expression);
            }
            else
            {
                switch (repeat.Type)
                {
                    case RepeatType.Once:
                        mock.SetupSequence(this.expression).Pass().Throws(new Exception("Should call once"));

                        break;
                    case RepeatType.Twice:
                        mock.SetupSequence(this.expression).Pass().Pass().Throws(new Exception("Should call only twice"));

                        break;
                    case RepeatType.Any:
                        mock.Setup(this.expression);

                        break;
                    case RepeatType.AtLeastOnce:
                        mock.Setup(this.expression).Verifiable();

                        break;
                    case RepeatType.Never:
                        mock.Setup(this.expression).Throws(new Exception("Should never call"));

                        break;
                    case RepeatType.Exact:
                        var sequence = this.mock.SetupSequence(this.expression);
                        for (var i = 0; i < repeat.ExactCount; i++)
                        {
                            sequence = sequence.Pass();
                        }

                        sequence.Throws(new Exception($"Should call only {repeat.ExactCount} times"));

                        break;
                }
            }
        }

        public void Throws(Exception exception)
        {
            this.mock.Setup(this.expression).Throws(exception);
        }

        internal void IgnoreArguments()
        {
            this.expression = new ArgumentsAdapter<T>().IgnoreArguments(this.expression);
        }
    }
}
