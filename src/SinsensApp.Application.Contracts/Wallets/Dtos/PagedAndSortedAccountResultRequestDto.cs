using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SinsensApp.Wallets.Dtos
{
    public class PagedAndSortedAccountResultRequestDto : PagedResultRequestDto
    {
        public string Title { get; set; }

        public bool? IncludeInTotals { get; set; }
    }
}