using System.Collections.ObjectModel;

namespace DualLinearProgram.Data;

public class MainFunction : Equation
{
    public ObservableCollection<string> OptimizationSigns { get; set; }
    public string SelectedOptimizationSign { get; set; }

    public MainFunction(int variableCount = 0) : base(variableCount)
    {
        OptimizationSigns = new ObservableCollection<string> { "min", "max" };
        SelectedOptimizationSign = "max";
    }
    
    public MainFunction(string optimizationSign) : base(0)
    {
        OptimizationSigns = new ObservableCollection<string> { "min", "max" };
        SelectedOptimizationSign = optimizationSign;
    }
}