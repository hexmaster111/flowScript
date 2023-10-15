using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using NodeEditorDemo.ViewModels.Nodes;
using ReflectorConformer;
using FuncCallViewModel = NodeEditorDemo.ViewModels.Nodes.FuncCallViewModel;

namespace NodeEditorDemo.Services;

public class NodeFactory : INodeFactory
{
    #region Not my code

    internal static INode CreateRectangle(double x, double y, double width, double height, string? label, double pinSize = 10)
    {
        var node = new NodeViewModel
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new RectangleViewModel { Label = label }
        };

        node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, "L");
        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, "R");
        node.AddPin(width / 2, 0, pinSize, pinSize, PinAlignment.Top, "T");
        node.AddPin(width / 2, height, pinSize, pinSize, PinAlignment.Bottom, "B");

        return node;
    }

    internal static INode CreateEllipse(double x, double y, double width, double height, string? label, double pinSize = 10)
    {
        var node = new NodeViewModel
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new EllipseViewModel { Label = label }
        };

        node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, "L");
        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, "R");
        node.AddPin(width / 2, 0, pinSize, pinSize, PinAlignment.Top, "T");
        node.AddPin(width / 2, height, pinSize, pinSize, PinAlignment.Bottom, "B");

        return node;
    }

    internal static INode CreateSignal(double x, double y, double width = 180, double height = 30, string? label = null, bool? state = false, double pinSize = 10, string name = "SIGNAL")
    {
        var node = new NodeViewModel
        {
            Name = name,
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new SignalViewModel { Label = label, State = state }
        };

        node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, "IN");
        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, "OUT");

        return node;
    }

    internal static INode CreateAndGate(double x, double y, double width = 60, double height = 60, double pinSize = 10, string name = "AND")
    {
        var node = new NodeViewModel
        {
            Name = name,
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new AndGateViewModel { Label = "&" }
        };

        node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, "L");
        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, "R");
        node.AddPin(width / 2, 0, pinSize, pinSize, PinAlignment.Top, "T");
        node.AddPin(width / 2, height, pinSize, pinSize, PinAlignment.Bottom, "B");

        return node;
    }

    internal static INode CreateOrGate(double x, double y, double width = 60, double height = 60, int count = 1, double pinSize = 10, string name = "OR")
    {
        var node = new NodeViewModel
        {
            Name = name,
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new OrGateViewModel { Label = "≥", Count = count }
        };

        node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, "L");
        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, "R");
        node.AddPin(width / 2, 0, pinSize, pinSize, PinAlignment.Top, "T");
        node.AddPin(width / 2, height, pinSize, pinSize, PinAlignment.Bottom, "B");

        return node;
    }

    #endregion
    
    public static INode CreateFunctionCallNode(
        FunctionInformation function,
        double x, double y, string name = "Class.Function")
    {
        Debug.Assert(function.Parameters != null, nameof(function.Parameters) + " != null");
        Debug.Assert(function.Signature != null, nameof(function.Signature) + " != null");
        var pins = new ObservableCollection<IPin>();

        
        var node = new NodeViewModel
        {
            Name = name,
            X = x,
            Y = y,
            // Width = width,
            // Height = height,
            Pins = pins,
            Content = new FuncCallViewModel()
            {
                Function = function
            },
        };
        const double FONT_HEIGHT = 15;
        const double PIN_SIZE = 10;
        double pinPxY = FONT_HEIGHT * 2;
        
        foreach (var p in function.Parameters)
        {
            pins.Add(new PinViewModel
            {
                Name = p.Name,
                Parent = node,
                X = 0,
                Y = pinPxY,
                Width = PIN_SIZE,
                Height = PIN_SIZE,
                Alignment = PinAlignment.Left,
                
            });
            pinPxY += FONT_HEIGHT;
        }
        return node;
    }

    internal static IConnector CreateConnector(IPin? start, IPin? end)
    {
        return new ConnectorViewModel
        {
            Start = start,
            End = end
        };
    }

    public IDrawingNode CreateDrawing(string? name = null)
    {
        var drawing = new DrawingNodeViewModel
        {
            Name = name,
            X = 0,
            Y = 0,
            Width = 900,
            Height = 600,
            Nodes = new ObservableCollection<INode>(),
            Connectors = new ObservableCollection<IConnector>(),
            EnableMultiplePinConnections = false,
            EnableSnap = true,
            SnapX = 15.0,
            SnapY = 15.0,
            EnableGrid = true,
            GridCellWidth = 15.0,
            GridCellHeight = 15.0,
        };

        return drawing;
    }

    public IList<INodeTemplate> CreateTemplates()
    {
        return GetFunctionNodes(DemoDataGen.GetConsoleWriteLine());
    }

    private static IList<INodeTemplate> GetFunctionNodes(IEnumerable<FunctionInformation> avalableFunctions)
    {
        var nodes = new ObservableCollection<INodeTemplate>();
        foreach (var function in avalableFunctions)
        {
            var node = new NodeTemplateViewModel
            {
                Title = function.Signature,
                Template = CreateFunctionCallNode(function, 0, 0),
                Preview = CreateFunctionCallNode(function, 0, 0)
            };
            nodes.Add(node);
        }

        return nodes;
    }
}