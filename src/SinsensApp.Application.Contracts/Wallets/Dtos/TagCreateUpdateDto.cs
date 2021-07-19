using System;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class TagCreateUpdateDto
    {
        [Required(ErrorMessage = "«Î ‰»Î÷˜Ã‚")]
        public string Title { get; set; }
    }
}