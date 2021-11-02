namespace RhinoMocksToMoq
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Moq;

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

    public static class Arg<T>
    {
        public static IsArg Is => new IsArg();

        public static T Matches(Expression<Func<T, bool>> predicate) => It.Is(predicate);

        public class IsArg
        {
            public T Anything => It.IsAny<T>();

            public T Null => It.Is<T>(x => x == null);

            public T NotNull => It.IsNotNull<T>();

            public T Equal(T input) => It.Is<T>(x => x.Equals(input));
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
