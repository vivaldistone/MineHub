using MineHub.Application.Abstractions.Persistence;

namespace MineHub.Application.Users.Queries.GetUsers;

public class GetUsersQueryHandler
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyCollection<GetUserResponse>> HandleAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => new GetUserResponse(
            u.Id,
            u.Email,
            u.CreatedAtUtc,
            u.MinecraftProfile?.MinecraftUuid,
            u.MinecraftProfile?.NickName))
            .ToList();
    }
}
