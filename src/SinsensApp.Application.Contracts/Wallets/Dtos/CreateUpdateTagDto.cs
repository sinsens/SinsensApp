using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class CreateUpdateTagDto
    {
        [Required(ErrorMessage = "����������")]
        public string Title { get; set; }
    }
}