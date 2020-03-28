using System.ComponentModel.DataAnnotations;

namespace HiringDev.Client.Models
{
    public class SearchModel
    {
        [Required]
        public string SearchTerm { get; set; }

    }
}