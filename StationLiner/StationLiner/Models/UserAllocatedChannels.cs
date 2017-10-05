using System.ComponentModel.DataAnnotations.Schema;

namespace StationLinerMVC.Models
{
    public class UserAllocatedChannels
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        [ForeignKey("Station")]
        public long StationId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Station Station { get; set; }
//        public virtual
    }
}