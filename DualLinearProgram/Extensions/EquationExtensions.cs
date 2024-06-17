using DualLinearProgram.Data;

namespace DualLinearProgram.Extensions;

public static partial class Extensions
{
    public static T Init<T>(this T lst,
        int initialConstraintsCount,
        int initialVariableCount)
        where T : IList<Constraint>
    {
        for (var i = 0; i < initialConstraintsCount; i++)
        {
            lst.Add(new Constraint(initialVariableCount));
        }

        return lst;
    }

    public static T Init<T>(this T lst,
        int initialVariableCount,
        string inequalitySign)
        where T : IList<Condition>
    {
        for (var i = 0; i < initialVariableCount; i++)
        {
            lst.Add(new Condition(i + 1, inequalitySign, 0));
        }

        return lst;
    }
    
    public static IList<Constraint> ReformConstraints(MainFunction function, IList<Constraint> constraints)
    {
        foreach (var constraint in constraints)
        {
            if (function.SelectedOptimizationSign == "min")
            {
                if (constraint.SelectedInequalitySign == "<=")
                {
                    constraint.RevertConstraintSigns();
                    constraint.SelectedInequalitySign = ">=";
                }
                // else if (constraint.SelectedInequalitySign == "=")
                // {
                //     constraint.SelectedInequalitySign = ">=";
                // }
            }
            else if (function.SelectedOptimizationSign == "max")
            {
                if (constraint.SelectedInequalitySign == ">=")
                {
                    constraint.RevertConstraintSigns();
                    constraint.SelectedInequalitySign = "<=";
                }
                // else if (constraint.SelectedInequalitySign == "=")
                // {
                //     constraint.SelectedInequalitySign = "<=";
                // }
            }
        }

        return constraints;
    }

    public static Constraint RevertConstraintSigns(this Constraint constraint)
    {
        foreach (var variable in constraint.Variables)
        {
            variable.Coefficient = -variable.Coefficient;
        }

        constraint.Constant = -constraint.Constant;

        return constraint;
    }
}