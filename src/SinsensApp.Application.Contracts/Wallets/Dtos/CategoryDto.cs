using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class CategoryDto : EntityDto<Guid>
    {
        [Required(ErrorMessage = "����������")]
        public string Title { get; set; }

        public int? Color { get; set; }

        [Required(ErrorMessage = "��ѡ�������")]
        public TransactionType TransactionType { get; set; }

        public int? SortOrder { get; set; }
    }
}