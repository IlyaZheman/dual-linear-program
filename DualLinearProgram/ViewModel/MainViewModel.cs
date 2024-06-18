using System.Collections.ObjectModel;
using System.Windows.Input;
using DualLinearProgram.Command;
using DualLinearProgram.Data;
using DualLinearProgram.Extensions;
using DualLinearProgram.Logic;
using Constraint = DualLinearProgram.Data.Constraint;

namespace DualLinearProgram.ViewModel;

public class MainViewModel : ViewModel
{
    public ICommand AddConstraintCommand { get; }
    public ICommand RemoveConstraintCommand { get; }

    public ICommand AddVariableCommand { get; }
    public ICommand RemoveVariableCommand { get; }

    public ICommand SolveCommand { get; }

    public ICommand SetDefaultMaxValueCommand { get; }
    public ICommand SetDefaultMinValueCommand { get; }

    public const int InitialVariableCount = 2;
    public const int InitialConstraintsCount = 2;

    private MainFunction _mainFunction;
    private ObservableCollection<Constraint> _mainConstraints;
    private ObservableCollection<Condition> _mainConditions;
    private double _mainDualResult;

    private MainFunction _dualFunction;
    private ObservableCollection<Constraint> _dualConstraints;
    private ObservableCollection<Condition> _dualConditions;
    private double _dualDualResult;

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

    public ObservableCollection<Condition> MainConditions
    {
        get => _mainConditions;
        set
        {
            _mainConditions = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Condition> DualConditions
    {
        get => _dualConditions;
        set
        {
            _dualConditions = value;
            OnPropertyChanged();
        }
    }

    public double MainResult
    {
        get => _mainDualResult;
        set
        {
            _mainDualResult = value;
            OnPropertyChanged();
        }
    }

    public double DualResult
    {
        get => _dualDualResult;
        set
        {
            _dualDualResult = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        MainFunction = new MainFunction(InitialVariableCount);
        MainConstraints = new ObservableCollection<Constraint>().Init(InitialConstraintsCount, InitialVariableCount);
        MainConditions = new ObservableCollection<Condition>().Init(InitialVariableCount, ">=");

        DualFunction = new MainFunction();
        DualConstraints = new ObservableCollection<Constraint>();
        DualConditions = new ObservableCollection<Condition>();

        AddConstraintCommand = new RelayCommand(AddConstraint);
        RemoveConstraintCommand = new RelayCommand(RemoveConstraint);

        AddVariableCommand = new RelayCommand(AddVariable);
        RemoveVariableCommand = new RelayCommand(RemoveVariable);

        SolveCommand = new RelayCommand(Solve);

        SetDefaultMaxValueCommand = new RelayCommand(SetDefaultMaxValue);
        SetDefaultMinValueCommand = new RelayCommand(SetDefaultMinValue);
    }

    private void AddVariable(object parameter)
    {
        MainFunction.AddVariable();

        foreach (var constraint in MainConstraints)
        {
            constraint.AddVariable();
        }

        MainConditions.Add(new Condition(MainFunction.GetVariableCount(), ">=", 0));
    }

    private void RemoveVariable(object parameter)
    {
        MainFunction.RemoveVariable();

        foreach (var constraint in MainConstraints)
        {
            constraint.RemoveVariable();
        }

        if (MainConditions.Count > 0)
        {
            MainConditions.RemoveAt(MainFunction.GetVariableCount());
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
        helper.SetConditions(MainConditions);

        var success = helper.CalculateDual(out var dualFunction, out var dualConstraints, out var dualConditions);
        if (!success)
        {
            throw new Exception("Failed to calculate");
        }

        DualFunction = dualFunction;
        Console.WriteLine(DualFunction.Verbose());

        DualConstraints = new ObservableCollection<Constraint>(dualConstraints);
        Console.WriteLine(DualConstraints.Verbose());

        DualConditions = new ObservableCollection<Condition>(dualConditions);
        Console.WriteLine(DualConditions.Verbose());

        MainResult = new SimplexHelper().CalculateResult(MainFunction, MainConstraints.ToList());
        Console.WriteLine(MainResult);
        DualResult = new SimplexHelper().CalculateResult(DualFunction, DualConstraints.ToList());
        Console.WriteLine(DualResult);
    }

    private void SetDefaultMaxValue(object parameter)
    {
        var value = DefaultValues.GetDefaultMaxValue();

        MainFunction = value.Item1;
        MainConstraints = value.Item2;
    }

    private void SetDefaultMinValue(object parameter)
    {
        var value = DefaultValues.GetDefaultMinValue();

        MainFunction = value.Item1;
        MainConstraints = value.Item2;
    }
}