using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Papyrus.Business.Documents
{
    public class DisplayableProduct : INotifyPropertyChanged
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool Equals(DisplayableProduct other) {
            return string.Equals(ProductId, other.ProductId) && string.Equals(ProductName, other.ProductName);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DisplayableProduct) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((ProductId != null ? ProductId.GetHashCode() : 0)*397) ^ (ProductName != null ? ProductName.GetHashCode() : 0);
            }
        }

        public DisplayableProduct Clone() {
            return new DisplayableProduct {
                ProductId = ProductId,
                ProductName = ProductName
            };
        }
    }
}