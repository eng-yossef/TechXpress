using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TechXpress.Web.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            if (member != null)
            {
                var display = member.GetCustomAttribute<DisplayAttribute>();
                return display?.Name ?? enumValue.ToString();
            }
            return enumValue.ToString();
        }

        // For the whole enum type: get list of (Value, Display Name)
        public static IEnumerable<KeyValuePair<string, string>> GetEnumSelectList(this Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("Type must be an enum");

            return Enum.GetValues(enumType)
                .Cast<Enum>()
                .Select(e => new KeyValuePair<string, string>(e.ToString(), e.GetDisplayName()));
        }
    }
}
