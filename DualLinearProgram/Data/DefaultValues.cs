using System.Collections.ObjectModel;

namespace DualLinearProgram.Data;

public static class DefaultValues
{
    public static (MainFunction, ObservableCollection<Constraint>) GetDefaultMaxValue()
    {
        var mainFunction = new MainFunction();
        var mainConstraints = new ObservableCollection<Constraint>();

        mainFunction.AddVariable(2);
        mainFunction.AddVariable(1);
        mainFunction.AddVariable(-3);
        mainFunction.AddVariable(1);
        mainFunction.SelectedOptimizationSign = "max";

        var constraint1 = new Constraint();
        constraint1.AddVariable(1);
        constraint1.AddVariable(2);
        constraint1.AddVariable(0);
        constraint1.AddVariable(-1);
        constraint1.SelectedInequalitySign = "<=";
        constraint1.Constant = 4;
        mainConstraints.Add(constraint1);

        var constraint2 = new Constraint();
        constraint2.AddVariable(1);
        constraint2.AddVariable(-1);
        constraint2.AddVariable(1);
        constraint2.AddVariable(3);
        constraint2.SelectedInequalitySign = "<=";
        constraint2.Constant = 1;
        mainConstraints.Add(constraint2);

        return (mainFunction, mainConstraints);
    }

    public static (MainFunction, ObservableCollection<Constraint>) GetDefaultMinValue()
    {
        var mainFunction = new MainFunction("min");
        var mainConstraints = new ObservableCollection<Constraint>();

        mainFunction.AddVariable(2);
        mainFunction.AddVariable(1);
        mainFunction.AddVariable(-3);
        mainFunction.AddVariable(1);
        mainFunction.SelectedOptimizationSign = "min";

        var constraint1 = new Constraint();
        constraint1.AddVariable(-1);
        constraint1.AddVariable(-2);
        constraint1.AddVariable(0);
        constraint1.AddVariable(1);
        constraint1.SelectedInequalitySign = "<=";
        constraint1.Constant = -4;
        mainConstraints.Add(constraint1);

        var constraint2 = new Constraint();
        constraint2.AddVariable(1);
        constraint2.AddVariable(-1);
        constraint2.AddVariable(1);
        constraint2.AddVariable(3);
        constraint2.SelectedInequalitySign = ">=";
        constraint2.Constant = 1;
        mainConstraints.Add(constraint2);

        return (mainFunction, mainConstraints);
    }
}