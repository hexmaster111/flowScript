namespace ReflectorConformer;

public class MyClass
{
    public void MyFunc()
    {
    }
    
    public void MyFunc(int i)
    {
    }
    
    public void MyFunc(int i, string s)
    {
    }
    
    public int MyFunc(int i, string s, double d)
    {
        return 0;
    }
    
    public (int, int) MyFunc(int i, string s, double d, bool b)
    {
        return (0, 0);
    }
    
    public void SomeFunctionWithALongName(int and, int a, int lot, int of , int parameters, int to, int test, int the, int scroll, int bar)
    {
    }
}

public static class DemoDataGen
{
    public static IEnumerable<FunctionInformation> GetConsoleWriteLine() => FunctionInformation.FromType(typeof(MyClass), nameof(MyClass.MyFunc))
        .Concat(FunctionInformation.FromType(typeof(MyClass), nameof(MyClass.SomeFunctionWithALongName)));
}

public class ParameterInformation
{
    public string Name { get; set; } = string.Empty;
    public Type Type { get; set; }
}

public static class ErrorStrings
{
    public static string FormatErrorString(this string errString) => $"<error {errString}>";


    public const string MethodNameNotFound = "Method name not found";
}