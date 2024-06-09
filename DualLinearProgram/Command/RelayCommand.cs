using System.Windows.Input;

namespace DualLinearProgram.Command;

public class RelayCommand(Action<object> execute, Predicate<object> canExecute) : ICommand
{
    private readonly Action<object> _execute = execute ?? throw new ArgumentNullException("execute");

    public RelayCommand(Action<object> execute) : this(execute, null)
    {
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return canExecute == null || canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}