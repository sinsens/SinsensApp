using System;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class CategoryCreateUpdateDto
    {
        [Required(ErrorMessage = "请输入主题")]
        public string Title { get; set; }

        public int? Color { get; set; }

        [Required(ErrorMessage = "请选择交易类别")]
        public TransactionType TransactionType { get; set; }

        public int? SortOrder { get; set; }
    }
}