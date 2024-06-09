using System.Collections.ObjectModel;

namespace DualLinearProgram.Data;

public class Constraint : Equation
{
    public ObservableCollection<string> InequalitySigns { get; set; }
    public string SelectedInequalitySign { get; set; }
    public int Constant { get; set; }

    public Constraint(int variableCount = 0) : base(variableCount)
    {
        InequalitySigns = new ObservableCollection<string> { "<=", ">=", "=" };
        SelectedInequalitySign = "<=";
        Constant = 0;
    }
}