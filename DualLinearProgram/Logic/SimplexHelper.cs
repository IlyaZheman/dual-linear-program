using DualLinearProgram.Data;

namespace DualLinearProgram.Logic;

public class SimplexHelper
{
    private List<List<float>> table;
    private string[] rowVariables; // all variables in a row
    private string[] basicVariables; // contain basic variables

    private bool problemType = false; // Problem Type

    public float CalculateResult(MainFunction function, List<Constraint> constraints)
    {
        var n = function.GetVariableCount();
        var m = constraints.Count;

        problemType = function.SelectedOptimizationSign == "max";

        table = new(new List<float>[m + 1]);
        for (var i = 0; i < table.Count; i++)
        {
            table[i] = new List<float>(new float[n + m + 2]);
        }

        var targetFunction = new int[n];
        for (var i = 0; i < n; i++)
        {
            targetFunction[i] = problemType ? function.Variables[i].Coefficient : -function.Variables[i].Coefficient;
        }

        for (var i = 0; i < targetFunction.Length; i++)
        {
            table[0][i + 1] = targetFunction[i];
        }

        for (var j = 0; j < m; j++)
        {
            var constraint = constraints[j];

            table[j + 1][0] = 0;

            for (var i = 0; i < n; i++)
            {
                table[j + 1][i + 1] = constraint.Variables[i].Coefficient;
            }

            for (var i = 0; i < m; i++)
            {
                if (constraints[i].SelectedInequalitySign == ">=")
                {
                    table[0][n + i + 1] = problemType ? -1 : 1;
                }
                else if (constraints[i].SelectedInequalitySign == "=")
                {
                    table[0][n + i + 1] = problemType ? 0 : 1;
                }
            }

            var choice = constraint.SelectedInequalitySign switch
            {
                "<=" => 1,
                ">=" => 2,
                "=" => 3,
                _ => throw new Exception()
            };

            table[j + 1][n + j + 1] = choice switch
            {
                1 => 1,
                2 => -1,
                _ => 0
            };

            table[j + 1][table[0].Count - 1] = constraint.Constant;

            if (constraint.Constant < 0)
            {
                // negative value...
                for (var i = 0; i < table[0].Count; i++)
                {
                    table[j + 1][i] = -table[j + 1][i];
                }
            }
        }

        FillVariables(n, m);
        OptimizeTable();

        if (problemType)
        {
            Console.WriteLine("Значение целевой функции Zmax = " + table[0][table[0].Count - 1]);
            return table[0][table[0].Count - 1];
        }
        else
        {
            Console.WriteLine("Значение целевой функции Zmin = " + table[0][table[0].Count - 1]);
            return table[0][table[0].Count - 1];
        }
    }

    private void OptimizeTable()
    {
        while (IfMinExists())
        {
            var index = MinIndex();

            var minRatio = float.MaxValue;
            var minIndex = 0;

            var state = false;

            for (var j = 1; j < table.Count; j++)
            {
                if (table[j][index] > 0)
                {
                    // must be >= 0
                    state = true;
                    var ratio = table[j][table[0].Count - 1] / table[j][index];

                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        minIndex = j;
                    }
                }
            }

            if (!state)
            {
                Console.WriteLine("******* Система не имеет ограничений на области допустимых решений *******");
                break;
            }
            else
            {
                // Console.WriteLine("************ Итерация № " + iter + " ************");
                // Console.WriteLine("В базисные переменные переходит : " + rowVariables[index]);
                // Console.WriteLine("В свободные переменные переходит : " + basicVariables[minIndex]);
                // Console.WriteLine();

                basicVariables[minIndex] = rowVariables[index]; // swap basic variables...

                for (var i = 1; i < basicVariables.Length; i++)
                {
                    if (Math.Abs(table[i][index] - 1) < .001f)
                    {
                        basicVariables[i] = rowVariables[index];
                    }
                }

                RowOperation(index, minIndex); // row operation in table...
            }
        }
    }


    public void RowOperation(int index, int minIndex)
    {
        var num = table[minIndex][index];

        for (var i = 0; i < table[0].Count; i++)
        {
            table[minIndex][i] /= num;
        }

        for (var i = 0; i < table.Count; i++)
        {
            if (i != minIndex)
            {
                var cal = -table[i][index];

                for (var j = 0; j < table[0].Count; j++)
                {
                    table[i][j] = cal * table[minIndex][j] + table[i][j];
                }
            }
        }
    }

    private void FillVariables(int n, int m)
    {
        basicVariables = new string[m + 1];
        basicVariables[0] = "c";

        for (var i = 0; i < m; i++)
        {
            basicVariables[i + 1] = "s" + (i + 1);
        }

        rowVariables = new string[n + m + 2];
        rowVariables[0] = "z";
        for (var i = 0; i < n; i++)
        {
            rowVariables[i + 1] = "x" + (i + 1);
        }

        for (var i = 0; i < m; i++)
        {
            rowVariables[n + i + 1] = "s" + (i + 1);
        }

        rowVariables[n + m + 1] = "b";
    }

    private bool IfMinExists()
    {
        var state = false;

        for (var i = 1; i < table[0].Count; i++) // начинаем с 1, чтобы пропустить первый столбец
        {
            if (problemType)
            {
                if (table[0][i] > 0) // для максимизации ищем положительные коэффициенты
                {
                    state = true;
                    break;
                }
            }
            else
            {
                if (table[0][i] < 0) // для минимизации ищем отрицательные коэффициенты
                {
                    state = true;
                    break;
                }
            }
        }

        return state;
    }

    private int MinIndex()
    {
        var index = 0;
        var min = float.MaxValue;

        for (var i = 1; i < table[0].Count; i++) // начинаем с 1, чтобы пропустить первый столбец
        {
            if (problemType)
            {
                if (table[0][i] > 0 && table[0][i] < min) // для максимизации ищем наибольший положительный коэффициент
                {
                    index = i;
                    min = table[0][i];
                }
            }
            else
            {
                if (table[0][i] < 0 && table[0][i] < min) // для минимизации ищем наименьший отрицательный коэффициент
                {
                    index = i;
                    min = table[0][i];
                }
            }
        }

        return index;
    }
}