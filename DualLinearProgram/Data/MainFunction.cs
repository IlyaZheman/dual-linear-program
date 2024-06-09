using System.Collections.ObjectModel;

namespace DualLinearProgram.Data;

public class MainFunction
{
    public ObservableCollection<Variable> Variables { get; set; }
    public ObservableCollection<string> OptimizationSigns { get; set; }
    public string SelectedOptimizationSign { get; set; }

    public MainFunction()
    {
        Variables = new ObservableCollection<Variable>();
        OptimizationSigns = new ObservableCollection<string> { "min", "max" };
        SelectedOptimizationSign = "max";
    }

    public void AddVariable()
    {
        Variables.Add(new Variable
        {
            Coefficient = "0",
            VariableIndex = (Variables.Count + 1).ToString()
        });
    }

    public void RemoveVariable()
    {
        if (Variables.Count > 0)
        {
            Variables.RemoveAt(Variables.Count - 1);
        }
    }

    public int GetVariableCount()
    {
        return Variables.Count;
    }
}