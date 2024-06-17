namespace DualLinearProgram.SimplexMethod
{
    public class Constraint
    {
        public double[] variables;
        public double b;
        public string sign;

        public Constraint(double[] variables, double b, string sign)
        {
            if (sign == "=" || sign == "<=" || sign == ">=")
            {
                this.variables = variables;
                this.b = b;
                this.sign = sign;
            }
            else
            {
                throw new ArgumentException("Wrong sign");
            }
        }
    }
}