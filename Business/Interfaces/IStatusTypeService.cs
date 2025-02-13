using Business.Dtos;

namespace Business.Interfaces;

public interface IStatusTypeService
{
    Task<IEnumerable<StatusTypeDto>> GetAllStatusTypesAsync();
    Task<StatusTypeDto?> GetStatusTypeByIdAsync(int id);
}