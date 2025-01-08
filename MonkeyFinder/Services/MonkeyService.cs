using System.Net.Http.Json;
using MonkeyFinder.Model;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    private readonly HttpClient httpClient;

    public MonkeyService()
    {
        httpClient = new HttpClient();
    }
    
    private List<Monkey> _monkeyList = [];
    public async Task<List<Monkey>> GetMonkeysAsync()
    {
        if (_monkeyList.Count > 0) return _monkeyList;
        var response = await httpClient.GetAsync("https://montemagno.com/monkeys.json");
        if (response.IsSuccessStatusCode)
        {
            var monkeysResult = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
            if (monkeysResult is not null)
            {
                _monkeyList = monkeysResult;
            }
        }
        return _monkeyList;
    }
}