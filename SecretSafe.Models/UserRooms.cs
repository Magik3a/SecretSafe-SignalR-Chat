namespace Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserRooms
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual SecretSafeUser User { get; set; }


        public string RoomName { get; set; }
    }
}
