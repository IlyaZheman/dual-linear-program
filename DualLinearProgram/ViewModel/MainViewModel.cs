using System.Collections.ObjectModel;
using System.Windows.Input;
using DualLinearProgram.Command;
using DualLinearProgram.Data;

namespace DualLinearProgram.ViewModel;

public class MainViewModel
{
    public MainFunction MainFunction { get; set; }
    public ObservableCollection<Constraint> Constraints { get; set; }

    public ICommand AddConstraintCommand { get; }
    public ICommand RemoveConstraintCommand { get; }
    public ICommand AddVariableCommand { get; }
    public ICommand RemoveVariableCommand { get; }
    public ICommand SolveCommand { get; }

    public MainViewModel()
    {
        MainFunction = new MainFunction();
        Constraints = new ObservableCollection<Constraint>();

        AddConstraintCommand = new RelayCommand(AddConstraint);
        RemoveConstraintCommand = new RelayCommand(RemoveConstraint);

        AddVariableCommand = new RelayCommand(AddVariable);
        RemoveVariableCommand = new RelayCommand(RemoveVariable);

        SolveCommand = new RelayCommand(Solve);
    }

    private void AddVariable(object parameter)
    {
        MainFunction.AddVariable();

        foreach (var constraint in Constraints)
        {
            constraint.AddVariable();
        }
    }

    private void RemoveVariable(object parameter)
    {
        MainFunction.RemoveVariable();

        foreach (var constraint in Constraints)
        {
            constraint.RemoveVariable();
        }
    }

    private void AddConstraint(object parameter)
    {
        Constraints.Add(new Constraint(MainFunction.GetVariableCount()));
    }

    private void RemoveConstraint(object parameter)
    {
        if (Constraints.Count > 0)
        {
            Constraints.RemoveAt(Constraints.Count - 1);
        }
    }

    private void Solve(object parameter)
    {
        var data = new double[Constraints.Count, Constraints.Max(eq => eq.Variables.Count) + 2];
        for (var i = 0; i < Constraints.Count; i++)
        {
            for (var j = 0; j < Constraints[i].Variables.Count; j++)
            {
                data[i, j] = double.Parse(Constraints[i].Variables[j].Coefficient);
            }

            data[i, Constraints[i].Variables.Count] = Constraints[i].InequalitySigns.IndexOf(Constraints[i].SelectedInequalitySign);
            data[i, Constraints[i].Variables.Count + 1] = double.Parse(Constraints[i].Constant);
        }
    }
}