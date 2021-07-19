using System;
using Volo.Abp.Application.Dtos;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class TagDto : EntityDto<Guid>
    {
        public string Title { get; set; }
    }
}