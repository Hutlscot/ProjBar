namespace ProjBar
{
    using System;
    using System.Windows.Input;

    public class Command : ICommand
    {
        private readonly Func<object, bool> canExecute;

        private readonly Action<object> execute;

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) != false;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}