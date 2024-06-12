namespace DualLinearProgram.Data;

public class Condition
{
    public int VariableIndex { get; set; }
    public string InequalitySign { get; set; }
    public int Constant { get; set; }

    public Condition()
    {
        VariableIndex = 0;
        InequalitySign = ">=";
        Constant = 0;
    }

    public Condition(int variableIndex, string inequalitySign, int constant)
    {
        VariableIndex = variableIndex;
        InequalitySign = inequalitySign;
        Constant = constant;
    }
}