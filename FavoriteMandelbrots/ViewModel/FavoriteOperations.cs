using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using FavoriteMandelbrots.Model;

namespace FavoriteMandelbrots.ViewModel
{
    internal class FavoriteOperations
    {
        private MainViewerViewModel vm;
        public FavoriteOperations(MainViewerViewModel vm)
        {
            this.vm = vm;
        }

        public void AddCurrentArea()
        {
            Add(vm.CurrentArea.Clone());
        }

        private void Add(Area area)
        {
            var newAreaVM = new AreaViewModel(area, vm);
            vm.Favorites.Add(newAreaVM);
            // Now we need to notify the UI about the new values.
            newAreaVM.NotifyAllPropertiesChanged();
        }
    }
}
