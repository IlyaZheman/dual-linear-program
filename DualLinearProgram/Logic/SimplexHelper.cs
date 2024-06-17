using DualLinearProgram.Data;

namespace DualLinearProgram.SimplexMethod;

public class SimplexHelper
{
    public string Process(MainFunction mainFunction, List<Data.Constraint> mainConstraints)
    {
        var functionVariables = new double[mainFunction.Variables.Count];
        for (var i = 0; i < mainFunction.Variables.Count; i++)
        {
            functionVariables[i] = mainFunction.Variables[i].Coefficient;
        }

        var c = 0;

        var isExtrMax = mainFunction.SelectedOptimizationSign == "max";

        Function function = new Function(functionVariables, c, isExtrMax);
        
        
        Constraint[] constraints = new Constraint[mainConstraints.Count];
        for (int i = 0; i < mainConstraints.Count; i++)
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
        
        // Constraint[] constraints = new Constraint[mainConstraints.GetVariableCount()];
        // for (int i = 0; i < mainConstraints.GetVariableCount(); i++)
        // {
        //     double[] variables = mainConstraints.Variables.Select(x => (double)x.Coefficient).ToArray();
        //     double b = Convert.ToDouble(mainConstraints.Constant);
        //     string sign = Convert.ToString(mainConstraints.SelectedInequalitySign);
        //
        //     constraints[i] = new Constraint(variables, b, sign);
        // }
        

        var simplex = new Simplex(function, constraints);

        var result = simplex.GetResult();
        
        switch (result.Item2)
        {
            case SimplexResult.Found:
                string extrStr = isExtrMax ? "max" : "min";
                return "The optimal solution is found: F" + extrStr + $" = {result.Item1.Last().fValue}";
            case SimplexResult.Unbounded:
                return "The domain of admissible solutions is unbounded";
            case SimplexResult.NotYetFound:
                return "Algorithm has made 100 cycles and hasn't found any optimal solution.";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}