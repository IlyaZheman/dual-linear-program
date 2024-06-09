using System.Collections.ObjectModel;

namespace DualLinearProgram.Data;

public abstract class Equation
{
    public ObservableCollection<Variable> Variables { get; protected set; }

    protected Equation(int variableCount = 0)
    {
        Variables = new ObservableCollection<Variable>();
        UpdateVariablesCount(variableCount);
    }

    public virtual void AddVariable(int coefficient = 0)
    {
        Variables.Add(new Variable
        {
            Coefficient = coefficient,
            VariableIndex = Variables.Count + 1
        });
    }

    public virtual void RemoveVariable()
    {
        if (Variables.Count > 0)
        {
            Variables.RemoveAt(Variables.Count - 1);
        }
    }

    public virtual int GetVariableCount()
    {
        return Variables.Count;
    }

    private void UpdateVariablesCount(int count)
    {
        var difference = count - Variables.Count;
        if (difference > 0)
        {
            for (var i = 0; i < difference; i++)
                AddVariable();
        }
        else if (difference < 0)
        {
            for (var i = 0; i < -difference; i++)
                RemoveVariable();
        }
    }
}