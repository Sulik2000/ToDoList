using System;
using System.Windows.Input;

namespace Application.ViewModels;

public class DelegateCommand : ICommand
{
    private Action<object?> _execute;
    private Func<object?, bool> _canExecute;
    public bool CanExecute(object? parameter)
    {
        return _canExecute.Invoke(parameter);
    }

    public DelegateCommand(Func<object?, bool> canExecute, Action<object?> execute)
    {
        this._canExecute = canExecute;
        this._execute = execute;
    }
    public void Execute(object? parameter)
    {
        _execute.Invoke(parameter);
    }

    public event EventHandler? CanExecuteChanged;
}