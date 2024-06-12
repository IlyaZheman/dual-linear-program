using DualLinearProgram.Data;
using DualLinearProgram.Extensions;

namespace DualLinearProgram.Logic;

public class CalculationHelper
{
    private MainFunction MainFunction { get; set; }
    private IList<Constraint> Constraints { get; set; }
    private IList<Condition> Conditions { get; set; }

    public void SetMainFunction(MainFunction mainFunction)
    {
        MainFunction = mainFunction;
    }

    public void SetConstraints(IList<Constraint> constraints)
    {
        Constraints = constraints;
    }

    public void SetConditions(IList<Condition> conditions)
    {
        Conditions = conditions;
    }

    public bool CalculateDual(
        out MainFunction dualFunction,
        out IList<Constraint> constraints,
        out IList<Condition> conditions)
    {
        if (MainFunction == null || MainFunction.Equals(default(MainFunction)))
            throw new NullReferenceException($"{nameof(MainFunction)} is not set up");
        if (Constraints.IsNullOrEmpty())
            throw new NullReferenceException($"{nameof(Constraints)} is not set up");
        if (Conditions.IsNullOrEmpty())
            throw new NullReferenceException($"{nameof(Conditions)} is not set up");

        dualFunction = CalculateDualFunction();
        constraints = CalculateDualConstraints(dualFunction.SelectedOptimizationSign);
        conditions = new List<Condition>();
        // conditions = CalculateDualConditions();

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

    private IList<Constraint> CalculateDualConstraints(string optimizationSign)
    {
        var constraints = new List<Constraint>();

        for (var i = 0; i < MainFunction.GetVariableCount(); i++)
        {
            var constraint = CalculateDualConstraint(i, Constraints[i].SelectedInequalitySign, optimizationSign);
            constraints.Add(constraint);
        }

        return constraints;
    }

    private Constraint CalculateDualConstraint(int variableIndex, string inequalitySign, string optimizationSign)
    {
        var constraint = new Constraint();
        foreach (var targetConstrain in Constraints)
        {
            var coefficient = targetConstrain.Variables[variableIndex].Coefficient;
            constraint.AddVariable(coefficient);
        }

        return constraint;
    }
}