using System;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class TagCreateUpdateDto
    {
        [Required(ErrorMessage = "����������")]
        public string Title { get; set; }
    }
}