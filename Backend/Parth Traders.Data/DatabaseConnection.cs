using Npgsql;
namespace Parth_Traders.Data;
public static class DatabaseConnection
{
    public static string Fingerprint(string value)
    {
        var host = new NpgsqlConnectionStringBuilder(Parse(value)).Host!.Replace("-pooler", "");
        return Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(host))).ToLowerInvariant();
    }
    public static string Parse(string value)
    {
        if (!value.StartsWith("postgres://") && !value.StartsWith("postgresql://")) return value;
        var uri = new Uri(value);
        var credentials = uri.UserInfo.Split(':', 2);
        return new NpgsqlConnectionStringBuilder {
            Host = uri.Host, Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Username = Uri.UnescapeDataString(credentials[0]), Password = Uri.UnescapeDataString(credentials[1]),
            Database = uri.AbsolutePath.Trim('/'), SslMode = SslMode.VerifyFull,
            Timeout = 15, CommandTimeout = 30, MaxPoolSize = 10
        }.ConnectionString;
    }
}
