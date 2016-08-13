using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using Papyrus.Business.Actions;
using Papyrus.Business.Domain.Products;
using Papyrus.Infrastructure.Core;
using Papyrus.Infrastructure.Repositories;

namespace Papyrus.Api.Controllers
{
    public class ProductsController : ApiController {
        private static readonly string ProductsPath =
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\Debug\MyProducts"));

        [Route("products/"), HttpGet]
        public List<Product> GetAllProducts() {
            var documentsRepository = new FileProductRepository(new JsonFileSystemProvider(ProductsPath));
            return new GetProducts(documentsRepository).Execute().ToList();            
        }
    }
}
