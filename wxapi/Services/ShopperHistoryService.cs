using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using wxapi.Data;

namespace wxapi.Services
{

	public class ShopperHistoryService: IShopperHistoryService
	{
		private readonly IConfigService configService;
		private readonly IRestClient restClient;

		public ShopperHistoryService(IConfigService configService, IRestClient restClient)
		{
			this.configService = configService;
			this.restClient = restClient;
		}

		public async Task<IEnumerable<ShopperHistory>> GetShopperHistory()
		{
			var url = configService.GetShopperHistoryApiUrl();
			var list = await restClient.GetAsync<List<ShopperHistory>>(url);
			return list;
		}
	}
}
