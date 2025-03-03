using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Anomaly_Detector.ViewModels
{
    /// <summary>
    /// A base class for ViewModels that implements INotifyPropertyChanged to enable property change notifications.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. Automatically provided by the compiler if not specified.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
