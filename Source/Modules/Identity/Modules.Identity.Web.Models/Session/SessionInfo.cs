namespace BeanLog.Modules.Identity.Web.Models.Session;

public class SessionInfo
{
    public SessionState State { get; init; } = SessionState.Inactive;
    public IEnumerable<KeyValuePair<string, string>> Claims { get; init; } = new List<KeyValuePair<string, string>>();
}