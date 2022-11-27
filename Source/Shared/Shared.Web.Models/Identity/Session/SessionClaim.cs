namespace BeanLog.Shared.Web.Models.Identity.Session;

public class SessionClaim
{
    public readonly string Key;
    public readonly string Value;

    public SessionClaim(string key, string value)
    {
        Key = key;
        Value = value;
    }
}