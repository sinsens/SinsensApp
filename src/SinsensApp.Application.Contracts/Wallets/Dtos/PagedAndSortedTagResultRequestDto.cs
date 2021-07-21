using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SinsensApp.Wallets.Dtos
{
    public class PagedAndSortedTagResultRequestDto : PagedResultRequestDto
    {
        public PagedAndSortedTagResultRequestDto()
        {
            Ids = new HashSet<Guid>();
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}