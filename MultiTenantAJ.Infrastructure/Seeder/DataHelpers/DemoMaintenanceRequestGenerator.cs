using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Seeder.DataHelpers;

public static class DemoMaintenanceRequestGenerator
{
    public static List<MaintenanceRequest> Create(
        IReadOnlyList<Property> properties)
    {
        var descriptions = new[]
        {
            "Klima uređaj ne hladi dovoljno.",
            "Slavina u kupaonici propušta vodu.",
            "Potrebna je zamjena oštećene rolete.",
            "Potrebno je servisirati bojler.",
            "Ne radi rasvjeta na terasi."
        };

        var priorities = new[]
        {
            MaintenancePriorityEnum.Low,
            MaintenancePriorityEnum.Medium,
            MaintenancePriorityEnum.High,
            MaintenancePriorityEnum.Critical
        };

        var requests = new List<MaintenanceRequest>();

        for (var i = 0; i < properties.Count; i++)
        {
            var description = descriptions[i % descriptions.Length];
            var priority = priorities[i % priorities.Length];

            var request = MaintenanceRequest.Create(
                properties[i].Id,
                description,
                priority);

            SetStatus(request, i);

            requests.Add(request);
        }

        return requests;
    }

    private static void SetStatus(
        MaintenanceRequest request,
        int index)
    {
        switch (index % 5)
        {
            case 0:
                return;

            case 1:
                request.StartProgress();
                return;

            case 2:
                request.StartProgress();
                request.Resolve();
                return;

            case 3:
                request.StartProgress();
                request.Resolve();
                request.Close();
                return;

            case 4:
                request.Cancel();
                return;
        }
    }
}
