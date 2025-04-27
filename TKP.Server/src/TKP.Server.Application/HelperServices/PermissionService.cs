using System.Reflection;
using TKP.Server.Application.HelperServices.Interface;
using TKP.Server.Domain.Permissions;

namespace TKP.Server.Application.HelperServices
{
    public class PermissionService : IPermissionService
    {
        private readonly Lazy<Dictionary<string, Permission>> _permissionsMap = new(() =>
        {
            var permissions = new Dictionary<string, Permission>();

            var types = typeof(FeaturePermissions).GetNestedTypes(BindingFlags.Public | BindingFlags.Static);
            foreach (var type in types)
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(Permission))
                    {
                        var permission = field.GetValue(null) as Permission;
                        if (permission != null && !string.IsNullOrEmpty(permission.Key))
                        {
                            permissions[permission.Key] = permission;
                        }
                    }
                }
            }

            return permissions;
        });

        public List<string> GetAllPermissionKeys()
        {
            return _permissionsMap.Value.Keys.ToList();
        }

        public List<Permission> GetAllPermissions()
        {
            return _permissionsMap.Value.Values.ToList();
        }

        public bool IsPermissionExists(string key)
        {
            return _permissionsMap.Value.ContainsKey(key);
        }

        public bool IsListPermissionExists(List<string> keys)
        {
            foreach (var item in keys)
            {
                if (!_permissionsMap.Value.ContainsKey(item))
                {
                    return false;
                }
            }
            return true;
        }

        public List<Permission> GetPermissionByListKey(List<string> keys)
        {
            List<Permission> permissions = new List<Permission>();
            foreach (var key in keys)
            {
                _permissionsMap.Value.TryGetValue(key, out var permission);
                if (permission is not null)
                    permissions.Add(permission);
            }

            return permissions;
        }

        public Permission? GetPermissionByKey(string key)
        {
            _permissionsMap.Value.TryGetValue(key, out var permission);
            return permission;
        }
        public List<string> GetPermissionKeysByFeature(string feature)
        {
            return _permissionsMap.Value
                .Where(p => p.Key.StartsWith(feature, StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Key)
                .ToList();
        }
    }

}
