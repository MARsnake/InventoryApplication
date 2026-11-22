namespace InventoryApplication.Domain.Enum;
public enum UnitType { Meter = 1, Kilogram = 2, Roll = 3, Piece = 4 }

public static class UnitInfo
{
    public static string GetName(this UnitType unit) => unit switch
    {
        UnitType.Meter => "متر",
        UnitType.Kilogram => "کیلوگرم",
        UnitType.Roll => "رول",
        UnitType.Piece => "عدد",
        _ => unit.ToString()
    };
}