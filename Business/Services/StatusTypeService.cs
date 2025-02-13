using Business.Dtos;
using Business.Interfaces;
using Data.Repositories;

namespace Business.Services;

public class StatusTypeService(StatusTypeRepository statusTypeRepository) : IStatusTypeService
{
    private readonly StatusTypeRepository _statusTypeRepository = statusTypeRepository;

    public async Task<IEnumerable<StatusTypeDto>> GetAllStatusTypesAsync()
    {
        var statusTypes = await _statusTypeRepository.GetAllStatusTypesAsync();
        return statusTypes.Select(s => new StatusTypeDto
        {
            Id = s.Id,
            StatusName = s.StatusName
        }).ToList();
    }

    public async Task<StatusTypeDto?> GetStatusTypeByIdAsync(int id)
    {
        var statusType = await _statusTypeRepository.GetStatusTypeByIdAsync(id);
        if (statusType == null) return null;

        return new StatusTypeDto
        {
            Id = statusType.Id,
            StatusName = statusType.StatusName
        };
    }
}