using System.Net.Http.Json;
using MonkeyFinder.Model;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    private readonly HttpClient _httpClient;

    public MonkeyService()
    {
        _httpClient = new HttpClient();
    }
    
    private List<Monkey> _monkeyList = [];
    public async Task<List<Monkey>> GetMonkeysAsync()
    {
        if (_monkeyList.Count > 0) return _monkeyList;
        var response = await _httpClient.GetAsync("https://montemagno.com/monkeys.json");
        if (!response.IsSuccessStatusCode) return _monkeyList;
        var listOfMonkey = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
        if (listOfMonkey is not null)
        {
            _monkeyList = listOfMonkey;
        }
        return _monkeyList;
    }
}