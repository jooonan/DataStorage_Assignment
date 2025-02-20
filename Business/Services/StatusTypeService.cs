using Business.Dtos;
using Business.Interfaces;
using Data.Repositories;

namespace Business.Services;

public class StatusTypeService(StatusTypeRepository statusTypeRepository) : IStatusTypeService
{
    private readonly StatusTypeRepository _statusTypeRepository = statusTypeRepository;

    public async Task<IEnumerable<StatusTypeDto>> GetAllStatusTypesAsync()
    {
        try
        {
            var statusTypes = await _statusTypeRepository.GetAllStatusTypesAsync();
            return statusTypes.Select(s => new StatusTypeDto
            {
                Id = s.Id,
                StatusName = s.StatusName
            }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving all status types: {ex.Message}");
            return [];
        }
    }

    public async Task<StatusTypeDto?> GetStatusTypeByIdAsync(int id)
    {
        try
        {
            var statusType = await _statusTypeRepository.GetStatusTypeByIdAsync(id);
            if (statusType == null) return null;

            return new StatusTypeDto
            {
                Id = statusType.Id,
                StatusName = statusType.StatusName
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving status type by ID ({id}): {ex.Message}");
            return null;
        }
    }
}
