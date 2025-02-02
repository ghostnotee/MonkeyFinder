namespace MonkeyFinder.Services.Permission;

public interface IPermissionService
{
    Task<PermissionStatus> CheckAndRequestPermissionAsync<T>() where T : Permissions.BasePermission, new();
}