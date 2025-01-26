using System.Net.Http.Json;
using MonkeyFinder.Model;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    private readonly HttpClient _httpClient;

    private List<Monkey> _monkeyList = [];

    public MonkeyService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<Monkey>> GetMonkeysAsync()
    {
        if (_monkeyList.Count > 0) return _monkeyList;
        var response = await _httpClient.GetAsync("https://montemagno.com/monkeys.json");
        if (!response.IsSuccessStatusCode) return _monkeyList;
        var listOfMonkey = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
        if (listOfMonkey is not null) _monkeyList = listOfMonkey;
        return _monkeyList;
    }

    public List<Monkey> AddMonkey(Monkey monkey)
    {
        _monkeyList.Add(monkey);
        return _monkeyList;
    }

    public Monkey FindMonkeyByName(string? name)
    {
        var monkey = _monkeyList.FirstOrDefault(m => m.Name == name);
        if (monkey is null) throw new Exception($"Monkey with name {name} not found");

        return monkey;
    }
}