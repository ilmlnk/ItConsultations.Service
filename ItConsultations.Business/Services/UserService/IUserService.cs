using ItConsultations.Business.Dtos.AuthDtos;
using ItConsultations.Business.Dtos.UserDtos;

namespace ItConsultations.Business.Services.UserService;

public interface IUserService
{
    Task<UserDto> CreateOrUpdateAsync(string firebaseUid, RegisterDto dto);

    Task<UserDto> GetByFirebaseUidAsync(string firebaseUid);
    
    Task<UserDto> CreateAsync(CreateUserDto dto);

    Task<UserDto> CreateAsync(CreateUserDto dto, string consId);

    Task<UserDto> UpdateAsync(UpdateUserDto dto);

    Task<UserDto> DeleteAsync(long id);

    Task<UserDto> DeleteAsync(string consId);

    Task<UserDto> GetAsync(long id);

    UserDto GetById(string consId);

    Task<List<UserDto>> GetAllAsync();
}
