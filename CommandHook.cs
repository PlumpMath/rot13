using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Rot13
{
    /// <summary>
    /// Implements ICommand. Based tosome extent on the Prism DelegateCommand class but without
    /// quite so many bells & whistles
    /// </summary>
    public class CommandHook : ICommand
    {
        Func<object, bool> canExecute;
        Action<object> action;
        public event EventHandler CanExecuteChanged;

        // Constructors
        public CommandHook(Action<object> action)
        : this(action, (_) => true, null)
        { }

        public CommandHook(Action<object> action, Func<object, bool> canExecute)
            : this(action, canExecute, null)
        { }

        // Action is required. CanExecute defaults to true. Parent is optional
        public CommandHook(Action<object> action, Func<object, bool> canExecute, INotifyPropertyChanged parent)
        {
            this.canExecute = canExecute;
            this.action = action;
            if (parent != null)
                parent.PropertyChanged += Parent_PropertyChanged;
        }

        /// <summary>
        /// If CanExecute(p) returns true the UI element attached to it will be enabled
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
            => canExecute(parameter);

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            action(parameter);
        }

        /// <summary>
        /// Call this when the state of a commandable object has changed but has not
        /// necessarily updated a notifiable property (or if the object does not
        /// implement INotifyPropertyChanged)
        /// </summary>
        public void SuggestCanExecuteChanged()
           => OnCanExecuteChanged();

        // Ping the WPF runtime to check if the changed parameter should also result in a 
        // change to the executable status of this command.
        // Might not want to do this *every time* a property on the parent changes, but
        // for small use cases (or small containers!) it's probably ok 
        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => OnCanExecuteChanged();

        private void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}