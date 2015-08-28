using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papyrus.Infrastructure.Core.DomainEvents {
    public interface Subscriber { }

    public interface Subscriber<in T> : Subscriber {
        void Handle(T domainEvent);
    }
}
