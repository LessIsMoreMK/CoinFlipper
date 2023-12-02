using CoinFlipper.Shared.DateTimeHelpers;
using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Application.Clients;
using CoinFlipper.Tracer.Application.Responses;
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
    public async Task GetFearAndGreedAsync()
    {
        try
        {
            var lastFearAndGreed = await fearAndGreedRepository.GetLastXFearAndGreedAsync(1);
            if (lastFearAndGreed.Count != 0 && lastFearAndGreed[0].DateTime >= DateTime.Today) 
                return;
            
            var fearAndGreedIndexes = await fearAndGreedIndexClient.GetFearAndGreedIndex(100);

            if (string.IsNullOrEmpty(fearAndGreedIndexes))
                return; //TODO: Log error?
            
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
            throw;
        }
    }
    
    //TODO: Unit tests
    //TODO: Base next iteration on received timeToUpdate from first record?
    //TODO: Check external source responsiveness at 00:00
}