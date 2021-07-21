using SinsensApp.Wallets;
using SinsensApp.Wallets.Dtos;
using AutoMapper;
using SinsensApp.Helper;
using Volo.Abp.AutoMapper;

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
        }
    }
}