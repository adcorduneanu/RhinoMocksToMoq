namespace RhinoMocksToMoq
{
    using System;
    using System.Reflection;

    internal static class ReflectionExtensions
    {
        public static T GetPrivatePropertyValue<T>(this object input, string propertyName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var propertyInfo = input.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), $"Property {propertyName} was not found in Type {input.GetType().FullName}");
            }

            return (T)propertyInfo.GetValue(input, null);
        }

        public static T GetPrivateFieldValue<T>(this object input, string propertyName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var type = input.GetType();
            FieldInfo fieldInfo = null;
            while (fieldInfo == null && type != null)
            {
                fieldInfo = type.GetField(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = type.BaseType;
            }

            if (fieldInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), $"Field {propertyName} was not found in Type {input.GetType().FullName}");
            }

            return (T)fieldInfo.GetValue(input);
        }

        public static void SetPrivatePropertyValue<T>(this object input, string propertyName, T value)
        {
            var type = input.GetType();
            if (type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), $"Property {propertyName} was not found in Type {input.GetType().FullName}");
            }

            type.InvokeMember(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, input, new object[] { value });
        }

        public static void SetPrivateFieldValue<T>(this object input, string propertyName, T value)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var type = input.GetType();
            FieldInfo fieldInfo = null;
            while (fieldInfo == null && type != null)
            {
                fieldInfo = type.GetField(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = type.BaseType;
            }

            if (fieldInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), $"Field {propertyName} was not found in Type {input.GetType().FullName}");
            }

            fieldInfo.SetValue(input, value);
        }
    }
}
