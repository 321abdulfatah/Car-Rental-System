namespace DataAccessLayer.Models
{
    public class Tokens : BaseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
