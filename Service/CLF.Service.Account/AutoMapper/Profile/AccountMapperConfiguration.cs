using CLF.Common.Infrastructure.Mapper;
using CLF.Model.Account;
using CLF.Model.Core.Data;
using CLF.Service.Account.AutoMapper.Converter;
using CLF.Service.DTO.Account;
using CLF.Service.DTO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLF.Service.Account.AutoMapper.Profile
{
    public class AccountMapperProfile : global::AutoMapper.Profile, IOrderedMapperProfile
    {
        public AccountMapperProfile()
        {
            CreateAccountMaps();
        }

        protected virtual void CreateAccountMaps()
        {
            CreateMap<EntityDTO, Entity>();
            CreateMap<Entity, EntityDTO>();

            CreateMap<Permission, PermissionDTO>()
                .IncludeBase<Entity, EntityDTO>()
                .ForMember(target => target.children, config => config.MapFrom(src => src.ChildNodes.Where(m => !m.IsDeleted)));
            //.ConvertUsing<PermissionConverter>();

            CreateMap<PermissionDTO, Permission>()
                .IncludeBase<EntityDTO, Entity>();
        }
        public int Order => 0;
    }
}
