using System.ComponentModel.DataAnnotations.Schema;

namespace StationLinerMVC.Models
{
    public class UserLayout
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Mode { get; set; }
        public long? ChannelId { get; set; }
    }
}