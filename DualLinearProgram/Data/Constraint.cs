using System.Collections.ObjectModel;

namespace DualLinearProgram.Data;

public class Constraint
{
    public ObservableCollection<Variable> Variables { get; set; }
    public ObservableCollection<string> InequalitySigns { get; set; }
    public string SelectedInequalitySign { get; set; }
    public string Constant { get; set; }

    public Constraint()
    {
        Variables = new ObservableCollection<Variable>();
        InequalitySigns = new ObservableCollection<string> { "<=", ">=", "=" };
        SelectedInequalitySign = "<=";
        Constant = "0";
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
}