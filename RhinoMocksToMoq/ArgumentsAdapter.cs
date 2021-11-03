namespace Rhino.Mocks
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Moq;

    public static class Arg<T>
    {
        public static IsArg<T> Is => new IsArg<T>();

        public static T Matches(Expression<Func<T, bool>> predicate) => It.Is(predicate);

        public static ListArg<T> List => new ListArg<T>();

        public sealed class IsArg<TR>
        {
            public TR Anything => It.IsAny<TR>();

            public TR NotNull => It.IsNotNull<TR>();

            public TR Null => It.Is<TR>(x => x == null);

            public TR Equal(TR input) => It.Is<TR>(x => x.Equals(input));
        }

        public sealed class ListArg<TR> 
        {
            private readonly ArrayList missing = new ArrayList();

            private bool Contains(IEnumerable searchableCollection, IEnumerable searchCollection)
            {
                    foreach (object outer in searchCollection)
                    {
                        bool foundThis = false;
                        foreach (object inner in searchableCollection)
                        {
                            if (inner.Equals(outer))
                            {
                                foundThis = true;
                                break;
                            }
                        }
                        if (!foundThis && !missing.Contains(outer))
                        {
                            missing.Add(outer);
                        }
                    }

                    return missing.Count == 0;
            }

            public TX ContainsAll<TX>(TX collection) where TX : IEnumerable, TR
            {
                return It.Is<TX>(x => Contains(x, collection));
            }
        }
    }

    internal class ArgumentsAdapter
    {
        private static readonly MethodInfo isAnyMethod = typeof(It).GetMethod(nameof(It.IsAny), BindingFlags.Public | BindingFlags.Static);

        internal static MethodCallExpression IsAny(Type genericArgument)
        {
            return Expression.Call(isAnyMethod.MakeGenericMethod(genericArgument));
        }

        internal MethodCallExpression IgnoreArgumentsExpression(MethodCallExpression expressionMethod)
        {
            return expressionMethod.Update(
                expressionMethod.Object,
                expressionMethod.Arguments.Select(x => ArgumentsAdapter.IsAny(x.Type)).ToList<Expression>()
            );
        }
    }

    internal sealed class ArgumentsAdapter<T, TR> : ArgumentsAdapter where T : class
    {
        public Expression<Func<T, TR>> IgnoreArguments(Expression<Func<T, TR>> expression)
        {
            return expression.Update(this.IgnoreArgumentsExpression((MethodCallExpression)expression.Body), expression.Parameters);
        }  
    }

    internal class ArgumentsAdapter<T> : ArgumentsAdapter where T : class
    {
        public Expression<Action<T>> IgnoreArguments(Expression<Action<T>> expression)
        {
            return expression.Update(this.IgnoreArgumentsExpression((MethodCallExpression)expression.Body), expression.Parameters);
        }
    }
}
