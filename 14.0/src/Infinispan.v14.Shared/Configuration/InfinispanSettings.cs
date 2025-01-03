namespace Infinispan.v14.Shared.Configuration;

public sealed class InfinispanSettings
{
    public required string BaseAddress { get; set; }
    public required string CacheName { get; set; }
    public required List<AccessList> AccessList { get; set; } = [];
}

public sealed class AccessList
{
    public AccountType AccountType { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public enum AccountType
{
    None,
    Writer,
    Reader,
    Monitor
}