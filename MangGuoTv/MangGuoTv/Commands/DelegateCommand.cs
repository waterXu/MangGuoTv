using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MangGuoTv.Commands
{
    public class DelegateCommand:ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        public DelegateCommand(Action execute)
            : this(execute, null)
        {

        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentException("execute");
            }
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object prarameter)
        {
            if (this._canExecute != null)
            {
                return this._canExecute();
            }
            //默认返回true
            return true;
        }
        public void Execute(object parameter = null)
        {
            this._execute();
        }

        public event EventHandler CanExecuteChanged;

        public void TriggerCanExecuteChanged()
        {
            EventHandler canExecuteEvent = this.CanExecuteChanged;
            if (canExecuteEvent != null)
            {
                canExecuteEvent(this, EventArgs.Empty);
            }
        }
    }
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        {

        }

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            if (this._canExecute != null)
            {
                return this._canExecute((T)parameter);
            }
            return true;
        }

        public void Execute(object parameter = null)
        {
            this._execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void TriggerCanExecuteChanged()
        {
            EventHandler canExecuteChanged = this.CanExecuteChanged;
            if (canExecuteChanged != null)
            {
                canExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
