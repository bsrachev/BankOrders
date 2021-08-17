namespace BankOrders.Infrastructure
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ItemDisplayExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
    }
}
