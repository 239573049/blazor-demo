using System.ComponentModel.DataAnnotations;

namespace Entitys.SSH
{
    public class SSHUser
    {
        [Required]
        public string? Host { get; set; } = "127.0.0.1";
        [Required]
        public int Port { get; set; } = 22;
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
