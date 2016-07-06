using System.Collections.ObjectModel;
using Papyrus.Business.Documents;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics {
    public class DesignModeTopicsGridVm : TopicsGridVm
    {
        public DesignModeTopicsGridVm()
        {
            TopicsToList = new ObservableCollection<TopicSummary>
            {
                new TopicSummary
                {
                    LastDocumentTitle = "Login",
                    LastDocumentDescription = "Explicaci�n",
                    VersionName = "2.0",
                    Product = new DisplayableProduct {ProductId = "ProductId", ProductName = "Opportunity"}
                },
                new TopicSummary
                {
                    LastDocumentTitle = "Llamadas",
                    LastDocumentDescription = "Explicaci�n",
                    VersionName = "3.0",
                    Product = new DisplayableProduct {ProductId = "ProductId", ProductName = "Opportunity"}
                }
            };
        }
    }
}