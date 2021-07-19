using System;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class CategoryCreateUpdateDto
    {
        [Required(ErrorMessage = "����������")]
        public string Title { get; set; }

        public int? Color { get; set; }

        [Required(ErrorMessage = "��ѡ�������")]
        public TransactionType TransactionType { get; set; }

        public int? SortOrder { get; set; }
    }
}