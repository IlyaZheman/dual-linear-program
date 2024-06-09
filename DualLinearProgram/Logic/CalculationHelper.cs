using DualLinearProgram.Data;

namespace DualLinearProgram.Logic;

public class CalculationHelper
{
    private MainFunction MainFunction { get; set; }
    private IList<Constraint> Constraints { get; set; }

    public void SetMainFunction(MainFunction mainFunction)
    {
        MainFunction = mainFunction;
    }

    public void SetConstraints(IList<Constraint> constraints)
    {
        Constraints = constraints;
    }

    public bool CalculateDual(out MainFunction dualFunction, out IList<Constraint> constraints)
    {
        dualFunction = CalculateDualFunction();
        constraints = CalculateDualConstraints();

        return true;
    }

    private MainFunction CalculateDualFunction()
    {
        var dualFunction = new MainFunction();

        dualFunction.SelectedOptimizationSign = MainFunction.SelectedOptimizationSign switch
        {
            "min" => "max",
            "max" => "min",
            _ => throw new ArgumentException()
        };

        foreach (var constraint in Constraints)
        {
            dualFunction.AddVariable(constraint.Constant);
        }

        return dualFunction;
    }

    private IList<Constraint> CalculateDualConstraints()
    {
        var constraints = new List<Constraint>();

        for (var i = 0; i < MainFunction.GetVariableCount(); i++)
        {
            var constraint = new Constraint();
            for (var j = 0; j < Constraints.Count; j++)
            {
                var coefficient = Constraints[j].Variables[i].Coefficient;
                constraint.AddVariable(coefficient);
            }

            constraints.Add(constraint);
        }

        return constraints;
    }
}