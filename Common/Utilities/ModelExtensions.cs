using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Common.Utilities;

public static class ModelExtensions
{
    public static string ToDisplay(this object value, EnumExtensions.DisplayProperty property = EnumExtensions.DisplayProperty.Name)
    {
        Assert.NotNull(value, nameof(value));

        var attribute = value.GetType().GetField(value.ToString())
            .GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();
        if (attribute == null)
            return value.ToString();

        var propValue = attribute.GetType().GetProperty(property.ToString()).GetValue(attribute, null);
        return propValue.ToString();
    }
}