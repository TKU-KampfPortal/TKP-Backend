using TKP.Server.Domain.Permissions;

namespace TKP.Server.Application.HelperServices.Interface
{
    public interface IPermissionService
    {
        List<string> GetAllPermissionKeys();
        List<Permission> GetAllPermissions();
        bool IsPermissionExists(string key);
        bool IsListPermissionExists(List<string> keys);
        List<Permission> GetPermissionByListKey(List<string> keys);
        Permission? GetPermissionByKey(string key);
        List<string> GetPermissionKeysByFeature(string feature);

    }
}
