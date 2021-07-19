using System;
using SinsensApp.Wallets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SinsensApp.Wallets
{
    public class CurrencyAppService : CrudAppService<Currency, CurrencyDto, string, PagedAndSortedResultRequestDto>,
        ICurrencyAppService
    {
        private readonly ICurrencyRepository _repository;

        public CurrencyAppService(ICurrencyRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}