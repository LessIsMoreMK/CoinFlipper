using CoinFlipper.Shared.DateTimeHelpers;
using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Application.Clients;
using CoinFlipper.Tracer.Application.ExternalResponses;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;

public class FearAndGreedJob(
    IFearAndGreedRepository fearAndGreedRepository,
    IFearAndGreedIndexClient fearAndGreedIndexClient,
    ILogger<FearAndGreedJob> logger
    ) : IFearAndGreedJob
{
    /// <summary>
    /// Updates the FearAndGreed in database up to 100 days
    /// </summary>
    public async Task GetFearAndGreedAsync()
    {
        try
        {
            var lastFearAndGreed = await fearAndGreedRepository.GetLastXFearAndGreedAsync(1);

            var daysMissing = lastFearAndGreed.Count == 0 ? 100 : (DateTime.Today - lastFearAndGreed[0].DateTime).Days;
            
            var fearAndGreedIndexes = await fearAndGreedIndexClient.GetFearAndGreedIndex(daysMissing);

            if (string.IsNullOrEmpty(fearAndGreedIndexes))
            {
                logger.LogError("Unable to obtain FearAndGreedIndex");
                return;
            }
            
            var fearAndGreedResponse = JsonConvert.DeserializeObject<FearAndGreedResponse>(fearAndGreedIndexes);
            
            foreach (var fearAndGreedData in fearAndGreedResponse.Data)
                await fearAndGreedRepository.AddFearAndGreedAsync(
                    new FearAndGreed(
                        DateTimeExtensions.TimestampToDateTime(fearAndGreedData.Timestamp), 
                        fearAndGreedData.Value, 
                        fearAndGreedData.Classification));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while processing {FearAndGreedJob}", JobsIdentifier.FearAndGreedJob);
        }
    }
    
    //TODO: Unit tests
    //TODO: Check external source responsiveness at 00:00
}