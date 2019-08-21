using System;
namespace wxapi.Services
{
	public class ConfigService: IConfigService
	{
		public string GetToken()
		{
			return "0b22c95a-7ca5-4161-8740-a5fb74197321";
		}

		public string GetName()
		{
			return "Jun Shao";
		}

		public string GetProductApiUrl()
		{
			return "http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/products";
		}

		public string GetShopperHistoryApiUrl()
		{
			return "http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/shopperHistory";
		}
	}
}
