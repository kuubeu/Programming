using System;
using System.Windows.Input;

namespace PilkarzeMVVM.ViewModel.KlasaBazowa
{
    class Przekaz : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public Przekaz(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            else
            {
                _execute = execute;
                _canExecute = canExecute;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { if (_canExecute != null) CommandManager.RequerySuggested += value; }
            remove { if (_canExecute != null) CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
