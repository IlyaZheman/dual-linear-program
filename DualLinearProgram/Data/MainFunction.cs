using System.Collections.ObjectModel;

namespace DualLinearProgram.Data;

public class MainFunction
{
    public ObservableCollection<Variable> Variables { get; set; }
    public string Constant { get; set; }
    public ObservableCollection<string> OptimizationSigns { get; set; }
    public string SelectedOptimizationSign { get; set; }

    public MainFunction()
    {
        Variables = new ObservableCollection<Variable>();
        Constant = "0";
        OptimizationSigns = new ObservableCollection<string> { "min", "max" };
        SelectedOptimizationSign = "max";
    }
}