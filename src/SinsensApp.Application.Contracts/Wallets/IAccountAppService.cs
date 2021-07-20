using System;
using SinsensApp.Wallets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SinsensApp.Wallets
{
    public interface IAccountAppService :
        ICrudAppService<
            AccountDto,
            Guid,
            PagedAndSortedAccountResultRequestDto,
            AccountCreateUpdateDto,
            AccountCreateUpdateDto>
    {
    }
}