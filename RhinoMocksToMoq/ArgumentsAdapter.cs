namespace RhinoMocksToMoq
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Moq;

    internal sealed class ArgumentsAdapter
    {
        private static readonly MethodInfo isAnyMethod = typeof(It).GetMethod(nameof(It.IsAny), BindingFlags.Public | BindingFlags.Static);

        internal static MethodCallExpression IsAny(Type genericArgument)
        {
            return Expression.Call(isAnyMethod.MakeGenericMethod(genericArgument));
        }
    }

    internal sealed class ArgumentsAdapter<T, TR> where T : class
    {
        public Expression<Func<T, TR>> IgnoreArguments(Expression<Func<T, TR>> expression)
        {
            var expressionMethod = (MethodCallExpression)expression.Body;

            var newExpression = expressionMethod.Update(
                expressionMethod.Object,
                expressionMethod.Arguments.Select(x => ArgumentsAdapter.IsAny(x.Type)).ToList<Expression>()
            );

            return expression.Update(newExpression, expression.Parameters);
        }
    }

    internal class ArgumentsAdapter<T> where T : class
    {
        public Expression<Action<T>> IgnoreArguments(Expression<Action<T>> expression)
        {
            var expressionMethod = (MethodCallExpression)expression.Body;

            var newExpression = expressionMethod.Update(
                expressionMethod.Object,
                expressionMethod.Arguments.Select(x => ArgumentsAdapter.IsAny(x.Type)).ToList<Expression>()
            );

            return expression.Update(newExpression, expression.Parameters);
        }
    }
}
