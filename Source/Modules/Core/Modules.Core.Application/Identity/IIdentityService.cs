namespace BeanLog.Modules.Core.Application.Identity;

public interface IIdentityService
{
    bool IsActiveSession { get; }

    Task Register(string username, string emailAddress, string password);
    void BeginSession(string emailAddress, string password);
    void EndActiveSession();
}