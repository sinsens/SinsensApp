using SinsensApp.Wallets;
using SinsensApp.Wallets.Dtos;
using AutoMapper;
using SinsensApp.Helper;
using Volo.Abp.AutoMapper;
using SinsensApp.Wallets.Dtos.backup;
using System.Linq;
using System;
using System.Security.Cryptography.X509Certificates;

namespace SinsensApp
{
    public class SinsensAppApplicationAutoMapperProfile : Profile
    {
        public SinsensAppApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<CreateUpdateAccountDto, Account>(MemberList.Source);
            CreateMap<Tag, TagDto>();
            CreateMap<CreateUpdateTagDto, Tag>(MemberList.Source);
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateUpdateCategoryDto, Category>(MemberList.Source);
            CreateMap<CreateUpdateTransactionDto, Transaction>(MemberList.Source)
                .ForMember(x => x.Category, e => e.Ignore())
                .ForMember(x => x.CategoryId, e => e.MapFrom(dto => dto.Category.Id))
                .ForMember(x => x.AccountFrom, e => e.Ignore())
                .ForMember(x => x.AccountFromId, e => e.MapFrom(dto => dto.AccountFrom.Id))
                .ForMember(x => x.AccountTo, e => e.Ignore())
                .ForMember(x => x.AccountToId, e => e.MapFrom(dto => dto.AccountTo.Id));

            CreateMap<Transaction, TransactionDto>()
                .ForMember(dto => dto.TransactionTypeDescription, e => e.MapFrom(entity => entity.TransactionType.GetEnumDescription()));

            CreateMap<Account, AccountDto>();
            CreateMap<AccountCreateUpdateDto, Account>(MemberList.Source);
            CreateMap<CategoryDto, Category>();
            CreateMap<CategoryCreateUpdateDto, Category>(MemberList.Source);
            CreateMap<TagDto, Tag>();
            CreateMap<TagCreateUpdateDto, Tag>(MemberList.Source);
            CreateMap<TransactionAttachment, TransactionAttachmentDto>();
            CreateMap<TransactionAttachmentCreateUpdateDto, TransactionAttachment>(MemberList.Source);
            CreateMap<Currency, CurrencyDto>();

            #region backup and restore

            CreateMap<Currency, CurrenciesItemDto>().ReverseMap();

            CreateMap<Account, AccountsItemDto>()
                .ForMember(x => x.model_state, e => e.MapFrom(entity => entity.IsDeleted ? 0 : 1))
                .ForMember(x => x.balance, e => e.MapFrom(entity => entity.Balance * 100))
                .ForMember(x => x.currency_code, e => e.MapFrom(entity => entity.CurrencyCode))
                .ReverseMap()
                .ForMember(entity => entity.IsDeleted, e => e.MapFrom(dto => dto.model_state == 0))
                .ForMember(entity => entity.Balance, e => e.MapFrom(dto => dto.balance / 100))
                .ForMember(entity => entity.Currency, e => e.Ignore())
                .ForMember(entity => entity.CurrencyCode, e => e.MapFrom(dto => dto.currency_code));

            CreateMap<Category, CategoriesItemDto>()
                .ForMember(x => x.model_state, e => e.MapFrom(entity => entity.IsDeleted ? 0 : 1))
                .ForMember(x => x.transaction_type, e => e.MapFrom(entity => entity.TransactionType))
                .ReverseMap()
                .ForMember(entity => entity.IsDeleted, e => e.MapFrom(dto => dto.model_state == 0))
                .ForMember(entity => entity.TransactionType, e => e.MapFrom(dto => dto.transaction_type));

            CreateMap<Tag, TagsItemDto>()
                .ForMember(x => x.model_state, e => e.MapFrom(entity => entity.IsDeleted ? 0 : 1))
                .ReverseMap()
                .ForMember(entity => entity.IsDeleted, e => e.MapFrom(dto => dto.model_state == 0));

            CreateMap<Transaction, TransactionsItemDto>()
                .ForMember(x => x.date, e => e.MapFrom(entity => entity.Date.HasValue ? UnixTimeHelper.ToUnixTimeMilliseconds(entity.Date.Value) : 0))
                .ForMember(x => x.tag_ids, e => e.MapFrom(entity => entity.Tags.Select(t => t.Id)))
                .ForMember(x => x.account_from_id, e => e.MapFrom(entity => entity.AccountFromId))
                .ForMember(x => x.account_to_id, e => e.MapFrom(entity => entity.AccountToId))
                .ForMember(x => x.model_state, e => e.MapFrom(entity => entity.IsDeleted ? 0 : 1))
                .ForMember(x => x.amount, e => e.MapFrom(entity => entity.Amount * 100))
                .ForMember(x => x.include_in_reports, e => e.MapFrom(entity => entity.IncludeInReports))
                .ForMember(x => x.transaction_type, e => e.MapFrom(entity => entity.TransactionType))
                .ForMember(x => x.transaction_state, e => e.MapFrom(entity => entity.TransactionState))
                .ReverseMap()
                .ForMember(entity => entity.Date, e => e.MapFrom(dto => UnixTimeHelper.FromUnixTimeMilliseconds(dto.date)))
                .ForMember(entity => entity.IsDeleted, e => e.MapFrom(dto => dto.model_state == 0))
                .ForMember(entity => entity.Amount, e => e.MapFrom(dto => dto.amount / 100))
                .ForMember(entity => entity.AccountFrom, e => e.Ignore())
                .ForMember(entity => entity.AccountFromId, e => e.MapFrom(dto => dto.account_from_id))
                .ForMember(entity => entity.AccountTo, e => e.Ignore())
                .ForMember(entity => entity.AccountToId, e => e.MapFrom(dto => dto.account_to_id))
                .ForMember(entity => entity.Category, e => e.Ignore())
                .ForMember(entity => entity.CategoryId, e => e.MapFrom(dto => dto.category_id))
                .ForMember(entity => entity.IncludeInReports, e => e.MapFrom(dto => dto.include_in_reports))
                .ForMember(entity => entity.TransactionType, e => e.MapFrom(dto => dto.transaction_type))
                .ForMember(entity => entity.TransactionState, e => e.MapFrom(dto => dto.transaction_state));

            #endregion backup and restore
        }
    }
}