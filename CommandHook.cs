using System;
using System.ComponentModel;
using System.Windows.Input;

namespace rot13
{
    public class CommandHook : ICommand
    {
        Func<object, bool> canExecute;
        Action<object> action;

        public event EventHandler CanExecuteChanged;
        
        public CommandHook(Action<object> Action) 
        : this(Action, (_) => true, null) 
        { }

        public CommandHook(Action<object> Action, Func<object, bool> CanExecute)
            : this(Action, CanExecute, null)
        { }

        public CommandHook(Action<object> Action, Func<object, bool> CanExecute, INotifyPropertyChanged Parent)
        {
            this.canExecute = CanExecute;
            this.action = Action;
            if(Parent!= null)
                Parent.PropertyChanged += Parent_PropertyChanged;
        }

        // Might not want to do this in general *every time* a property on the parent changes
        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
            => canExecute(parameter);

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}