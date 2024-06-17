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
        out List<Constraint> constraints,
        out List<Condition> conditions)
    {
        if (MainFunction == null || MainFunction.Equals(default(MainFunction)))
            throw new NullReferenceException($"{nameof(MainFunction)} is not set up");
        if (Constraints.IsNullOrEmpty())
            throw new NullReferenceException($"{nameof(Constraints)} is not set up");
        if (Conditions.IsNullOrEmpty())
            throw new NullReferenceException($"{nameof(Conditions)} is not set up");

        Constraints = Extensions.Extensions.ReformConstraints(MainFunction, Constraints);

        dualFunction = CalculateDualFunction();
        conditions = CalculateDualConditions(dualFunction.SelectedOptimizationSign);
        constraints = CalculateDualConstraints(dualFunction.SelectedOptimizationSign);

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

    private List<Condition> CalculateDualConditions(string optimizationSign)
    {
        var conditions = new List<Condition>();

        for (var i = 0; i < Constraints.Count; i++)
        {
            var constraint = Constraints[i];
            var sign = constraint.SelectedInequalitySign;
            if (optimizationSign == "min")
            {
                if (sign == "<=")
                {
                    conditions.Add(new Condition(i + 1, ">=", 0));
                }
            }
            else if (optimizationSign == "max")
            {
                if (sign == ">=")
                {
                    conditions.Add(new Condition(i + 1, ">=", 0));
                }
            }
        }

        return conditions;
    }

    private List<Constraint> CalculateDualConstraints(string optimizationSign)
    {
        var constraints = new List<Constraint>();

        for (var i = 0; i < MainFunction.GetVariableCount(); i++)
        {
            var constraint = CalculateDualConstraint(i, optimizationSign);
            constraints.Add(constraint);
        }

        return constraints;
    }

    private Constraint CalculateDualConstraint(int variableIndex, string optimizationSign)
    {
        var constraint = new Constraint();

        foreach (var targetConstrain in Constraints)
        {
            var coefficient = targetConstrain.Variables[variableIndex].Coefficient;
            constraint.AddVariable(coefficient);
        }

        if (optimizationSign == "min")
        {
            constraint.SelectedInequalitySign = ">=";
        }
        else if (optimizationSign == "max")
        {
            constraint.SelectedInequalitySign = "<=";
        }

        constraint.Constant = MainFunction.Variables[variableIndex].Coefficient;

        return constraint;
    }
}