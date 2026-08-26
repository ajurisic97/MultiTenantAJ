using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Authorization;

public static class ActionCatalog
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
}
