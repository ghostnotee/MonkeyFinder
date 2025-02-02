namespace MonkeyFinder.Services.Permission;

public class PermissionService : IPermissionService
{
    public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>() where T : Permissions.BasePermission, new()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<T>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<T>();
            }

            return status;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"İzin kontrolü sırasında hata oluştu: {ex.Message}");
            return PermissionStatus.Denied;
        }
    }
}