using System.Collections.Generic;
using System.Threading.Tasks;
using wxapi.Data;

namespace wxapi.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfigService configService;
        private readonly IRestClient restClient;

        public ProductService(IConfigService configService, IRestClient restClient)
        {
            this.configService = configService;
            this.restClient = restClient;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var apiUrl = configService.GetProductApiUrl();
            var list = await restClient.GetAsync<List<Product>>(apiUrl);
            return list;
        }
    }
}
