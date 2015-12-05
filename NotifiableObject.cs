using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Rot13
{
    /// <summary>
    /// Base class for ViewModel classes. Implements INotifyPropertyChanged to allow data binding.
    /// </summary>
    public abstract class NotifiableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Trigger a change notification for data binding. By default the name of the calling member
        /// is used for the propertyName parameter.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed. May be left blank
        /// to use the name of the calling member. Caller may also use the nameof keyword to 
        /// provide this data, or an arbitrary string if the member to be updated is not directly the
        /// caller of this method.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName="")
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
