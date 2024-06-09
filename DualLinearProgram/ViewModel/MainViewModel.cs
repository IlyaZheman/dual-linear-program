using System.Collections.ObjectModel;
using System.Windows.Input;
using DualLinearProgram.Command;
using DualLinearProgram.Data;
using DualLinearProgram.Extensions;
using DualLinearProgram.Logic;

namespace DualLinearProgram.ViewModel;

public class MainViewModel : ViewModel
{
    public ICommand AddConstraintCommand { get; }
    public ICommand RemoveConstraintCommand { get; }
    public ICommand AddVariableCommand { get; }
    public ICommand RemoveVariableCommand { get; }
    public ICommand SolveCommand { get; }

    public const int InitialVariableCount = 2;
    public const int InitialConstraintsCount = 2;

    private MainFunction _mainFunction;
    private MainFunction _dualFunction;

    private ObservableCollection<Constraint> _mainConstraints;
    private ObservableCollection<Constraint> _dualConstraints;

    public MainFunction MainFunction
    {
        get => _mainFunction;
        set
        {
            _mainFunction = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Constraint> MainConstraints
    {
        get => _mainConstraints;
        set
        {
            _mainConstraints = value;
            OnPropertyChanged();
        }
    }

    public MainFunction DualFunction
    {
        get => _dualFunction;
        set
        {
            _dualFunction = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Constraint> DualConstraints
    {
        get => _dualConstraints;
        set
        {
            _dualConstraints = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        MainFunction = new MainFunction(InitialVariableCount);
        MainConstraints = new ObservableCollection<Constraint>().Init(InitialConstraintsCount, InitialVariableCount);

        DualFunction = new MainFunction();
        DualConstraints = new ObservableCollection<Constraint>();

        AddConstraintCommand = new RelayCommand(AddConstraint);
        RemoveConstraintCommand = new RelayCommand(RemoveConstraint);

        AddVariableCommand = new RelayCommand(AddVariable);
        RemoveVariableCommand = new RelayCommand(RemoveVariable);

        SolveCommand = new RelayCommand(Solve);
    }

    private void AddVariable(object parameter)
    {
        MainFunction.AddVariable();

        foreach (var constraint in MainConstraints)
        {
            constraint.AddVariable();
        }
    }

    private void RemoveVariable(object parameter)
    {
        MainFunction.RemoveVariable();

        foreach (var constraint in MainConstraints)
        {
            constraint.RemoveVariable();
        }
    }

    private void AddConstraint(object parameter)
    {
        MainConstraints.Add(new Constraint(MainFunction.GetVariableCount()));
    }

    private void RemoveConstraint(object parameter)
    {
        if (MainConstraints.Count > 0)
        {
            MainConstraints.RemoveAt(MainConstraints.Count - 1);
        }
    }

    private void Solve(object parameter)
    {
        var helper = new CalculationHelper();
        helper.SetMainFunction(MainFunction);
        helper.SetConstraints(MainConstraints);

        var success = helper.CalculateDual(out var dualFunction, out var dualConstraints);
        if (!success)
        {
            throw new Exception("Failed to calculate");
        }

        DualFunction = dualFunction;
        Console.WriteLine(DualFunction.Verbose());

        DualConstraints = new ObservableCollection<Constraint>(dualConstraints);
        Console.WriteLine(DualConstraints.Verbose());
    }
}