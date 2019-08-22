using System;

namespace HomeEconomics.Helpers
{
    public static class Enums
    {
        public static bool IsAValidEnumValue<TEnum>(TEnum value)
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }
    }
}