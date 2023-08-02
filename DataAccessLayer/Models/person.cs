namespace DataAccessLayer.Models
{
    public class Person : BaseModel
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public byte Age { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
                
    }
}
