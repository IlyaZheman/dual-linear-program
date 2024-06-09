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
}