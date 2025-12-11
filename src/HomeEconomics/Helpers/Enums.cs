namespace HomeEconomics.Helpers;

public static class Enums
{
    public static bool IsAValidEnumValue<TEnum>(TEnum value) => !Equals(value, default(TEnum)) && Enum.IsDefined(typeof(TEnum), value!);
}
