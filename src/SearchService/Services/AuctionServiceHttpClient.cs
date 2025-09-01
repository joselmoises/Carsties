using System;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Services;

public class AuctionServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    public AuctionServiceHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _config = config;
        _httpClient = httpClient;

    }

    public async Task<List<Item>> GetItemsForSearchDB()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        return await _httpClient.GetFromJsonAsync<List<Item>>($"{_config["AuctionServiceUrl"]}/api/auctions?date={lastUpdated}");
    }
}
