using MatchApi.Domain.DTOs.Cricket;
using MatchApi.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace MatchApi.Infrastructure.Services;

public class CricApiService : ICricApiService, ICricbuzzService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;

    public CricApiService(HttpClient httpClient, IConfiguration configuration, IDistributedCache cache)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _cache = cache;
    }

    public async Task<CricbuzzMatchListDto?> GetMatchesAsync()
    {
        var cacheKey = "cricbuzz:matches";

        try
        {
            var response = await _httpClient.GetAsync(
                "https://www.cricbuzz.com/api/home");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new JsonException(
                    "Cricbuzz returned an empty response.");
            }

            var result = JsonSerializer.Deserialize<CricbuzzMatchListDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result is null)
            {
                throw new JsonException(
                    "Cricbuzz returned invalid match list.");
            }

            
            var cacheJson = JsonSerializer.Serialize(result);

            await _cache.SetStringAsync(
                cacheKey,
                cacheJson);

            
            result.Source = "api";

            return result;
        }
        catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is JsonException)
        {
            // Cricbuzz fail -> Redis fallback

            var cachedJson = await _cache.GetStringAsync(cacheKey);

            if (string.IsNullOrWhiteSpace(cachedJson))
            {
                return null;
            }

            var cachedResult =
                JsonSerializer.Deserialize<CricbuzzMatchListDto>(
                    cachedJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (cachedResult is null)
            {
                return null;
            }

            cachedResult.Source = "cache";

            return cachedResult;
        }
    }


    public async Task<TestMatchInfoDto?> GetMatchInfoAsync(
     long matchId,
     CancellationToken cancellationToken)
    {
        var cacheKey =
            $"cricbuzz:match-info:{matchId}";

        try
        {
            var response = await _httpClient.GetAsync(
                $"api/mcenter/comm/{matchId}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(
                cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new JsonException(
                    "Cricbuzz returned an empty response.");
            }

            var result =
                JsonSerializer.Deserialize<TestMatchInfoDto>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (result is null)
            {
                throw new JsonException(
                    "Cricbuzz returned invalid match info.");
            }

           
            var cacheJson = JsonSerializer.Serialize(result);

            await _cache.SetStringAsync(
                cacheKey,
                cacheJson,
                cancellationToken);

            result.Source = "api";

            return result;
        }
        catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is JsonException)
        {
            // Cricbuzz fail -> Redis fallback

            var cachedJson = await _cache.GetStringAsync(
                cacheKey,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(cachedJson))
            {
                return null;
            }

            var cachedResult =
                JsonSerializer.Deserialize<TestMatchInfoDto>(
                    cachedJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (cachedResult is null)
            {
                return null;
            }

            cachedResult.Source = "cache";

            return cachedResult;
        }
    }

    public async Task<CricbuzzScorecardResponseDto?> GetScorecardAsync(
       long matchId,
       CancellationToken cancellationToken)
    {
        var cacheKey =
            $"cricbuzz:scorecard:{matchId}";

        try
        {
            var response = await _httpClient.GetAsync(
                $"api/mcenter/scorecard/{matchId}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(
                cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new JsonException(
                    "Cricbuzz returned an empty scorecard response.");
            }

            var result =
                JsonSerializer.Deserialize<CricbuzzScorecardResponseDto>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (result is null)
            {
                throw new JsonException(
                    "Cricbuzz returned invalid scorecard.");
            }

            
            var cacheJson = JsonSerializer.Serialize(result);

            await _cache.SetStringAsync(
                cacheKey,
                cacheJson,
                cancellationToken);

            result.Source = "api";

            return result;
        }
        catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is JsonException)
        {
            // Cricbuzz fail -> Redis fallback

            var cachedJson = await _cache.GetStringAsync(
                cacheKey,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(cachedJson))
            {
                return null;
            }

            var cachedResult =
                JsonSerializer.Deserialize<CricbuzzScorecardResponseDto>(
                    cachedJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (cachedResult is null)
            {
                return null;
            }

            cachedResult.Source = "cache";

            return cachedResult;
        }
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