using System.Reflection;

namespace BytePlatform.Server.Extensions;

public static class AppDomainExtensions
{
    #region Utilities

    /// <summary>
    /// Does type implement generic?
    /// </summary>
    /// <param name="type"></param>
    /// <param name="openGeneric"></param>
    /// <returns></returns>
    private static bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
    {
        try
        {
            var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
            foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
            {
                if (!implementedInterface.IsGenericType)
                    continue;

                if (genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition()))
                    return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    #endregion

    public static IEnumerable<Type> FindClassesOfType<FType>(this IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
    {
        var result = new List<Type>();
        try
        {
            foreach (var a in assemblies)
            {
                Type[]? types = null;
                try
                {
                    types = a.GetTypes();
                }
                catch
                {
                }

                if (types is null)
                    continue;

                foreach (var t in types)
                {
                    var assignTypeFrom = typeof(FType);
                    if (!assignTypeFrom.IsAssignableFrom(t) && (!assignTypeFrom.IsGenericTypeDefinition || !DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                        continue;

                    if (t.IsInterface)
                        continue;

                    if (onlyConcreteClasses)
                    {
                        if (t.IsClass && !t.IsAbstract)
                        {
                            result.Add(t);
                        }
                    }
                    else
                    {
                        result.Add(t);
                    }
                }
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            var msg = string.Empty;
            foreach (var e in ex.LoaderExceptions)
                msg += e.Message + Environment.NewLine;

            var fail = new Exception(msg, ex);
            //Debug.WriteLine(fail.Message, fail);

            throw fail;
        }

        return result;
    }
}
