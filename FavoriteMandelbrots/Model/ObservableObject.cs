using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FavoriteMandelbrots.Model
{
    public class ObservableObject : INotifyPropertyChanged
    {
        // EVIP: INPC base class and CallerMemberName attribute
        protected void Notify([CallerMemberName] string propertyName = "")
        {
            // EVIP: calling event with elvis operator
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
