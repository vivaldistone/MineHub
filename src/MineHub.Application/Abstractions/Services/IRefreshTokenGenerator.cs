namespace MineHub.Application.Abstractions.Services;

public interface ICryptographicStringGenerator
{
    string Generate(int sizeInBytes = 64);
}
