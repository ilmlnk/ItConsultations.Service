using ItConsultations.Business.Dtos.UserDtos;

namespace ItConsultations.Business.Services.UserService;

public interface IUserService
{
    Task<UserDto> CreateAsync(CreateUserDto dto);

    Task<UserDto> CreateAsync(CreateUserDto dto, string consId);

    Task<UserDto> UpdateAsync(UpdateUserDto dto);

    Task<UserDto> DeleteAsync(long id);

    Task<UserDto> DeleteAsync(string consId);

    Task<UserDto> GetAsync(long id);

    UserDto GetById(string consId);

    Task<List<UserDto>> GetAllAsync();
}
