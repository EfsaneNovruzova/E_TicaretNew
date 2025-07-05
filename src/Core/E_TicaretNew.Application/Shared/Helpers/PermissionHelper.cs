namespace E_TicaretNew.Application.Shared.Helpers;
using System.Reflection;

public static class PermissionHelper
{
    public static Dictionary<string, List<string>> GetAllPermissions()
    {
        var result = new Dictionary<string, List<string>>();
        var permissionType = typeof(Permissions);

        var nestedTypes = permissionType.GetNestedTypes(BindingFlags.Public | BindingFlags.Static);

        foreach (var modelType in nestedTypes)
        {
            var allField = modelType.GetField("All", BindingFlags.Public |
                BindingFlags.Static);

            if (allField != null)
            {
                var permissions = allField.GetValue(null) as List<string>;

                if (permissions != null)
                {
                    result.Add(modelType.Name, permissions);
                }
            }

        }
        return result;
    }
    public static List<string> GetAllPermissionList()
    {
        return GetAllPermissions().SelectMany(x => x.Value).ToList();
    }
}

