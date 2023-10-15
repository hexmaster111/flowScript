using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ReflectorConformer;

[ObservableObject]
public partial class FunctionInformation
{
    [ObservableProperty] private string? _name;
    [ObservableProperty] private Type? _returnType;
    [ObservableProperty] private ObservableCollection<ParameterInformation>? _parameters;
    [ObservableProperty] private string? _signature;


    private FunctionInformation(MethodInfo methodInfo)
    {
        Name = methodInfo.Name;
        ReturnType = methodInfo.ReturnType;
        Parameters = new ObservableCollection<ParameterInformation>();


        foreach (var parameterInfo in methodInfo.GetParameters())
        {
            var parameterInformation = new ParameterInformation
            {
                Name = parameterInfo.Name ?? ErrorStrings.MethodNameNotFound.FormatErrorString(),
                Type = parameterInfo.ParameterType
            };
            Parameters.Add(parameterInformation);
        }

        var sb = new StringBuilder();
        sb.Append(Name);
        sb.Append(" ( ");
        bool first = true;
        foreach (var p in Parameters)
        {
            if (!first) sb.Append(", ");
            else first = false;
            sb.Append(p.Name);
            sb.Append(" : ");
            sb.Append(p.Type.Name);
        }

        sb.Append(" ) -> ");
        sb.Append(ReturnType.Name);
        Signature = sb.ToString();
    }


    public static IEnumerable<FunctionInformation> FromType(Type type, string methodName)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));
        foreach (var info in type.GetMethods().Where(x => x.Name == methodName)) yield return GetFunctionInformation(info);
    }

    private static FunctionInformation GetFunctionInformation(MethodInfo methodInfo) => new(methodInfo ?? throw new ArgumentNullException(nameof(methodInfo)));
}