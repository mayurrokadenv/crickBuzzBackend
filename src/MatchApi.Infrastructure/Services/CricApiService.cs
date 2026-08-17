using MatchApi.Domain.DTOs.Cricket;
using MatchApi.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace MatchApi.Infrastructure.Services;

public class CricApiService : ICricApiService, ICricbuzzService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CricApiService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    public async Task<TestMatchInfoDto?> GetMatchInfoAsync(
 long matchId,
 CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(
            $"api/mcenter/comm/{matchId}",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        var result = JsonSerializer.Deserialize<TestMatchInfoDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result;
    }
    public async Task<CricbuzzMatchListDto?> GetMatchesAsync()
    {
        var response = await _httpClient.GetAsync("https://www.cricbuzz.com/api/home");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<CricbuzzMatchListDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }


    public async Task<CricbuzzScorecardResponseDto?> GetScorecardAsync(
    long matchId,
    CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(
            $"api/mcenter/scorecard/{matchId}",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(
            cancellationToken);

        return JsonSerializer.Deserialize<CricbuzzScorecardResponseDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }



    //Cricdatorg

    public async Task<CurrentMatchesResponse> GetCurrentMatchesAsync(
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["CricApi:ApiKey"];

        var url = $"currentMatches?apikey={apiKey}&offset={offset}";

        var response = await _httpClient.GetFromJsonAsync<CurrentMatchesResponse>(
            url,
            cancellationToken);

        return response ?? new CurrentMatchesResponse();
    }

    public async Task<MatchDetailsResponse> GetMatchDetailsAsync(
      string matchId,
      CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["CricApi:ApiKey"];

        var url = $"match_scorecard?apikey={apiKey}&id={matchId}";

        var response = await _httpClient.GetFromJsonAsync<MatchDetailsResponse>(
            url,
            cancellationToken);

        return response ?? new MatchDetailsResponse();
    }
}