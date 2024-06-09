using System.Text;
using DualLinearProgram.Data;

namespace DualLinearProgram.Extensions;

public static partial class Extensions
{
    public static string Verbose(this MainFunction function)
    {
        var str = new StringBuilder();
        for (var i = 0; i < function.Variables.Count; i++)
        {
            var variable = function.Variables[i];
            str.Append(variable.Coefficient)
                .Append('x')
                .Append(variable.VariableIndex);
            if (i < function.Variables.Count - 1)
            {
                str.Append('+');
            }
        }

        str.Append($"->{function.SelectedOptimizationSign}");

        return str.ToString();
    }

    public static string Verbose(this IList<Constraint> constraints)
    {
        var str = new StringBuilder();
        for (var i = 0; i < constraints.Count; i++)
        {
            var subStr = new StringBuilder();
            for (var j = 0; j < constraints[i].Variables.Count; j++)
            {
                var variable = constraints[i].Variables[j];
                subStr.Append(variable.Coefficient).Append('x').Append(variable.VariableIndex);
                if (j < constraints[i].Variables.Count - 1)
                {
                    subStr.Append('+');
                }
            }

            subStr.Append(constraints[i].SelectedInequalitySign).Append(constraints[i].Constant);

            if (i < constraints.Count - 1)
                str.AppendLine(subStr.ToString());
            else
                str.Append(subStr.ToString());
        }

        return str.ToString();
    }
}