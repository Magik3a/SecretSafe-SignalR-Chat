using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSafe.Models
{
    public class SecurityLelvelAddons
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SecurityLelvelAddonId { get; set; }

        public int SecurityLevelId { get; set; }

        public string Description { get; set; }

        public virtual SecurityLevel SecurityLevel { get; set; }
    }
}
