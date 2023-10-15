using System.Collections.ObjectModel;
using System.Reflection;

namespace ReflectorConformer;

public class TypeProvider
{
    public static ObservableCollection<Assembly> DocumentAvailableAssemblies { get; }

    static TypeProvider()
    {
        //TODO: Documents having different namespaces/assembly then what this editor is using
        DocumentAvailableAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToObservableCollection();
    }
}

public static class Ext
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
        var collection = new ObservableCollection<T>();
        foreach (var item in source) collection.Add(item);
        return collection;
    }
}