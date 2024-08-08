namespace WebSocketAPI.TokenService
{
    public interface ITokenService
    {
            string GenerateAcsessToken(string roles);  
    }
}
