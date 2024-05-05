using NewBannerchi.Entities;

namespace NewBannerchi.Authentication;

public interface IJwtProvider
{
     Task <string> Generate(User user);

}