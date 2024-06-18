using System.Reflection;

namespace BytePlatform.Server.Extensions;

public static class ObjectExtensions
{
    public static bool ExistsMember<T>(string name)
    {
        return typeof(T).GetProperties().Any(p => p.Name == name);
    }

    public static string[] GetMemberName<T>()
        where T : class
    {
        var members = typeof(T).GetMembers();
        return members.Select(m => m.Name).ToArray();
    }
}
