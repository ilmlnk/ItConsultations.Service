using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.UserDtos;
using ItConsultations.Business.Entities.User;
using ItConsultations.Utilities.Guards;

namespace ItConsultations.Business.Services.UserService;

public class UserService : IUserService
{
    private readonly IRepository<UserEntity, long> _repository;

    public UserService(IRepository<UserEntity, long> repository)
    {
        _repository = repository;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = MapperManager.Map<UserEntity>(dto);
        await _repository.CreateAsync(user);
        return MapperManager.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto, string consId)
    {
        var user = MapperManager.Map<UserEntity>(dto);
        await _repository.CreateAsync(user);
        return MapperManager.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateAsync(UpdateUserDto dto)
    {
        var user = MapperManager.Map<UserEntity>(dto);
        await _repository.UpdateAsync(user);
        return MapperManager.Map<UserDto>(user);
    }

    public async Task<UserDto> DeleteAsync(long id)
    {
        var user = await _repository.GetAsync(id);
        Guard.NotNull(user);
        await _repository.DeleteAsync(user);
        return MapperManager.Map<UserDto>(user);
    }

    public async Task<UserDto> DeleteAsync(string consId)
    {
        throw new NotImplementedException();
        //var user = _repository.Get(u => );
        //Guard.NotNull(user);
        //await _repository.DeleteAsync(user);
        //return MapperManager.Map<UserDto>(user);
    }

    public async Task<UserDto> GetAsync(long id)
    {
        var user = await _repository.GetAsync(id);
        Guard.NotNull(user);
        return MapperManager.Map<UserDto>(user);
    }

    public UserDto GetById(string consId)
    {
        throw new NotImplementedException();
        /*var user = _repository.Get(x => x.ConsId == consId).FirstOrDefault();
        Guard.NotNull(user);
        return MapperManager.Map<UserDto>(user);*/
    }

    public Task<List<UserDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    /*public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _repository.GetAsync();
        return MapperManager.Map<List<UserDto>>(users);
    }*/
}