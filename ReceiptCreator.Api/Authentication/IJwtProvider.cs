
using ReceiptCreator.Api.Entities;

namespace ReceiptCreator.Api.Authentication;

public interface IJwtProvider
{
     Task <string> Generate(User user);

}