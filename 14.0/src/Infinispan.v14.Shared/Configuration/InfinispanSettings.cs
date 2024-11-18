namespace Infinispan.v14.Shared.Configuration;

public sealed class InfinispanSettings
{
    public string BaseAddress { get; set; }
    public string CacheName { get; set; }
    public List<AccessList> AccessList { get; set; } = new();
}

public sealed class AccessList
{
    public AccountType AccountType { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public enum AccountType
{
    None,
    Writer,
    Reader,
    Monitor
}