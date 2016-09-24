namespace SecretSafe.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SecurityLevel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SecurityLevelId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Level { get; set; }
    }
}