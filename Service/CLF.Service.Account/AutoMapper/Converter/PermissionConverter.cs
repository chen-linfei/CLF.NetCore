using AutoMapper;
using CLF.Model.Account;
using CLF.Service.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Service.Account.AutoMapper.Converter
{
    public class PermissionConverter : ITypeConverter<Permission, PermissionDTO>
    {
        public PermissionDTO Convert(Permission source, PermissionDTO destination, ResolutionContext context)
        {
            return new PermissionDTO();
        }
    }
}
