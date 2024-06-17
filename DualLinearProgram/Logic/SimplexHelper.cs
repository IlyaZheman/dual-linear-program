using DualLinearProgram.Data;

namespace DualLinearProgram.SimplexMethod;

public class SimplexHelper
{
    public double Process(MainFunction mainFunction, List<Data.Constraint> mainConstraints)
    {
        var functionVariables = new double[mainFunction.Variables.Count];
        for (var i = 0; i < mainFunction.Variables.Count; i++)
        {
            functionVariables[i] = mainFunction.Variables[i].Coefficient;
        }

        var c = 0;

        var isExtrMax = mainFunction.SelectedOptimizationSign == "max";

        var function = new Function(functionVariables, c, isExtrMax);

        var constraints = new Constraint[mainConstraints.Count];
        for (var i = 0; i < mainConstraints.Count; i++)
        {
            var targetConstraint = mainConstraints[i];

            var variables = new double[targetConstraint.Variables.Count];
            var b = targetConstraint.Constant;
            var sign = targetConstraint.SelectedInequalitySign;
            for (var j = 0; j < targetConstraint.Variables.Count; j++)
            {
                variables[j] = targetConstraint.Variables[j].Coefficient;
            }

            constraints[i] = new Constraint(variables, b, sign);
        }

        var simplex = new Simplex(function, constraints);

        var result = simplex.GetResult();

        return result.Item1.Last().fValue;

        // switch (result.Item2)
        // {
        //     case SimplexResult.Found:
        //         string extrStr = isExtrMax ? "max" : "min";
        //         return "The optimal solution is found: F" + extrStr + $" = {result.Item1.Last().fValue}";
        //     case SimplexResult.Unbounded:
        //         return "The domain of admissible solutions is unbounded";
        //     case SimplexResult.NotYetFound:
        //         return "Algorithm has made 100 cycles and hasn't found any optimal solution.";
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }
    }
}