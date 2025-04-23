using System.Windows.Input;

namespace kursova_biblioteka
{
    public class Commands : ICommand
    {
        Action action;
        Func<bool> canExecute;

        public Commands(Action action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }

        public void Execute(object? parameter)
        {
            action();
        }
    }
}