using MultiTenantAJ.Domain.Models.PropertyManagement;


namespace MultiTenantAJ.Infrastructure.Seeder.DataHelpers;

public static class DemoPropertyGenerator
{
    public static List<Property> Create(string tenantId)
    {
        var propertyNames = GetPropertyNames(tenantId);
        var city = GetCity(tenantId);

        var configurations = new[]
        {
            (Rooms: 2, Beds: 4, Capacity: 4),
            (Rooms: 3, Beds: 6, Capacity: 6),
            (Rooms: 1, Beds: 2, Capacity: 2),
            (Rooms: 4, Beds: 8, Capacity: 8),
            (Rooms: 2, Beds: 5, Capacity: 5)
        };

        var properties = new List<Property>();

        for (var i = 0; i < propertyNames.Length; i++)
        {
            var configuration = configurations[i];

            var property = Property.Create(
                propertyNames[i],
                $"Ulica smještaja {i + 1}, {city}",
                $"Smještajni objekt {propertyNames[i]} u gradu {city}.",
                configuration.Rooms,
                configuration.Beds,
                configuration.Capacity);

            properties.Add(property);
        }

        return properties;
    }

    private static string[] GetPropertyNames(string tenantId)
    {
        return tenantId switch
        {
            "adria_stay" =>
            [
                "Apartman Mare",
                "Vila Jadran",
                "Apartman Lavanda",
                "Kuća Maslina",
                "Apartman Bonaca"
            ],

            "dalmatia_rentals" =>
            [
                "Apartman Palma",
                "Vila Maris",
                "Apartman Bura",
                "Kuća Oleandar",
                "Apartman Val"
            ],

            "sibenik_travel" =>
            [
                "Apartman Krešimir",
                "Vila Solaris",
                "Apartman Kanal",
                "Kuća Mandalina",
                "Apartman Tvrđava"
            ],

            "jadran_apartments" =>
            [
                "Apartman Kornati",
                "Vila Murter",
                "Apartman Pinea",
                "Kuća Laguna",
                "Apartman Marina"
            ],

            _ => throw new InvalidOperationException(
                $"Demo properties are not configured for tenant {tenantId}.")
        };
    }

    private static string GetCity(string tenantId)
    {
        return tenantId switch
        {
            "adria_stay" => "Split",
            "dalmatia_rentals" => "Zadar",
            "sibenik_travel" => "Šibenik",
            "jadran_apartments" => "Vodice",
            _ => throw new InvalidOperationException(
                $"Demo properties are not configured for tenant {tenantId}.")
        };
    }
}
