using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nicologies.WpfCommon.Utils
{
    public static class ObservableCollectionEx
    {
        public static void AddRange<T>(this ObservableCollection<T> This, IEnumerable<T> collection)
        {
            foreach (var current in collection)
            {
                This.Add(current);
            }
        }
    }
}
