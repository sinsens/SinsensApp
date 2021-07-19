using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class CreateUpdateTagDto
    {
        [Required(ErrorMessage = "«Î ‰»Î÷˜Ã‚")]
        public string Title { get; set; }
    }
}