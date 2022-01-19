using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Networks
{
    public class DeleteNetworkIn
    {
        [Required]
        public string Name { get; set; }
    }
}
