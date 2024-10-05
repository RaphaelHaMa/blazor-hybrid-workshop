using System.Net.Http.Json;

namespace MonkeyFinderHybrid.Services;

public class MonkeyService
{
    private List<Monkey> monkeysList = new();

    private readonly HttpClient _httpClient;

    public MonkeyService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<Monkey>> GetMonkeysAsync()
    {
        if (monkeysList.Count > 0)
        {
            return monkeysList;
        }

        var response = await _httpClient.GetAsync("https://montemagno.com/monkeys.json");
        if (response.IsSuccessStatusCode)
        {
            var monkeysResult = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
            
            if (monkeysResult is not null)
            {
                monkeysList = monkeysResult;
            }
        }

        return monkeysList;
    }

    public List<Monkey> AddMonkey(Monkey monkey)
    {
        monkeysList.Add(monkey);
        return monkeysList;
    }

    public Monkey FindMonkeyByName(string name)
    {
        var monkey = monkeysList.FirstOrDefault(m => m.Name == name);

        if (monkey is null) throw new Exception("Monkey not found");

        return monkey;
    }

}