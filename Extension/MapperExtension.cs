using System.Reflection;

namespace MapperExtension.Extension;

public static class MapperExtension
{
    public static TTarget Map<TSource, TTarget>(this TSource source) where TTarget : new()
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        var target = new TTarget();
        var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var targetProperties = typeof(TTarget).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var sourceProperty in sourceProperties)
        {
            var targetProperty = targetProperties.FirstOrDefault(p => 
                p.Name == sourceProperty.Name && 
                p.PropertyType == sourceProperty.PropertyType);

            if (targetProperty != null && targetProperty.CanWrite)
                targetProperty.SetValue(target, sourceProperty.GetValue(source));
        }
        return target;
    }
    public static IEnumerable<TTarget> MapCollection<TSource, TTarget>(this IEnumerable<TSource> source) where TTarget : new()
    {
        if (source == null) 
            throw new ArgumentNullException(nameof(source));
        return source.Select(item => item.Map<TSource, TTarget>());
    }
}