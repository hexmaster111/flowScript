using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using NodeEditor.Model;
using ReflectorConformer;

namespace NodeEditorDemo.ViewModels.Nodes;

public partial class FuncCallViewModel : ViewModelBase
{
    [ObservableProperty] private FunctionInformation? _function;
    [ObservableProperty] private double _labelWidth;

    public FuncCallViewModel()
    {
        Function = DemoDataGen.GetConsoleWriteLine().Skip(2).First();
    }
}