namespace wxapi.Services
{
    public interface IConfigService
    {
        string GetToken();
        string GetName();
        string GetProductApiUrl();
		string GetShopperHistoryApiUrl();
    }
}
