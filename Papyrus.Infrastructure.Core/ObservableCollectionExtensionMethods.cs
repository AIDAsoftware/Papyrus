using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Papyrus.Infrastructure.Core {
    public static class ObservableCollectionExtensionMethods {
        public static void AddRange<T>(this ObservableCollection<T> sourceCollection, IEnumerable<T> collectionToAdd) {
            collectionToAdd.ToList().ForEach(sourceCollection.Add);
        }
    }
}