namespace HomeEconomics.Helpers;

public static class Enums
{
    public static bool IsAValidEnumValue<TEnum>(TEnum value) => value != null && Enum.IsDefined(typeof(TEnum), value);
}