using System.Collections.ObjectModel;
using System.Windows.Input;
using DualLinearProgram.Command;
using DualLinearProgram.Data;

namespace DualLinearProgram.ViewModel;

public class MainViewModel
{
    public MainFunction MainFunction { get; set; }
    public ObservableCollection<Constraint> Equations { get; set; }

    public ICommand AddEquationCommand { get; }
    public ICommand RemoveEquationCommand { get; }
    public ICommand AddVariableCommand { get; }
    public ICommand RemoveVariableCommand { get; }
    public ICommand AddMainFunctionVariableCommand { get; }
    public ICommand RemoveMainFunctionVariableCommand { get; }
    public ICommand SolveCommand { get; }

    public MainViewModel()
    {
        MainFunction = new MainFunction();
        Equations = new ObservableCollection<Constraint>();

        AddEquationCommand = new RelayCommand(AddConstraint);
        RemoveEquationCommand = new RelayCommand(RemoveConstraint);

        AddVariableCommand = new RelayCommand(AddVariable);
        RemoveVariableCommand = new RelayCommand(RemoveVariable);

        AddMainFunctionVariableCommand = new RelayCommand(AddMainFunctionVariable);
        RemoveMainFunctionVariableCommand = new RelayCommand(RemoveMainFunctionVariable);

        SolveCommand = new RelayCommand(Solve);
    }

    private void AddMainFunctionVariable(object parameter)
    {
        MainFunction.Variables.Add(new Variable
        {
            Coefficient = "0",
            VariableIndex = (MainFunction.Variables.Count + 1).ToString()
        });
    }

    private void RemoveMainFunctionVariable(object parameter)
    {
        if (MainFunction.Variables.Count > 0)
        {
            MainFunction.Variables.RemoveAt(MainFunction.Variables.Count - 1);
        }
    }

    private void AddConstraint(object parameter)
    {
        Equations.Add(new Constraint());
    }

    private void RemoveConstraint(object parameter)
    {
        if (Equations.Count > 0)
        {
            Equations.RemoveAt(Equations.Count - 1);
        }
    }

    private void AddVariable(object parameter)
    {
        var equation = parameter as Constraint;
        equation?.AddVariable();
    }

    private void RemoveVariable(object parameter)
    {
        var equation = parameter as Constraint;
        equation?.RemoveVariable();
    }

    private void Solve(object parameter)
    {
        var data = new double[Equations.Count, Equations.Max(eq => eq.Variables.Count) + 2];
        for (var i = 0; i < Equations.Count; i++)
        {
            for (var j = 0; j < Equations[i].Variables.Count; j++)
            {
                data[i, j] = double.Parse(Equations[i].Variables[j].Coefficient);
            }

            data[i, Equations[i].Variables.Count] = Equations[i].InequalitySigns.IndexOf(Equations[i].SelectedInequalitySign);
            data[i, Equations[i].Variables.Count + 1] = double.Parse(Equations[i].Constant);
        }
    }
}