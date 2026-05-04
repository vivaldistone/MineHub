using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Users.Queries.GetUser;

public class GetUserQueryHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserResponse> HandleAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User Id is required", nameof(userId));

        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
            throw new NotFoundException("User not found", "user_not_found");

        return new GetUserResponse(
            user.Id,
            user.Email,
            user.CreatedAtUtc,
            user.MinecraftProfile?.MinecraftUuid,
            user.MinecraftProfile?.NickName);
    }
}
