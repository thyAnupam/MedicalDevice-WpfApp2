using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace WpfApp2.ViewModels
{
    internal class RelayCommand : ICommand
    {
        private readonly Action _execute;
        //private readonly Predicate<object> _canExecuteAction;  
        
        public RelayCommand(Action execute)
        {
            _execute = execute;
            //_canExecuteAction = null;

        }

        /*public RelayCommand(Action execute, Predicate<object> canExecuteAction)
        {
            _execute= execute;
            _canExecuteAction = canExecuteAction;
        }*/

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
