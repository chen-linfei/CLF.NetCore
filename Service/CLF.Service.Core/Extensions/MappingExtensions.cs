using CLF.Common.Infrastructure.Mapper;
using CLF.Model.Core.Data;
using CLF.Service.DTO.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Service.Core.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination Map<TDestination>(this object source) where TDestination :class
        {
            return AutoMapperConfiguration.Mapper.Map<TDestination>(source);
        }
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination) where TSource : class where TDestination : class
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        public static TModel ToModel<TModel>(this BaseEntity entity) where TModel : BaseEntityDTO
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return entity.Map<TModel>();
        }

        public static TModel ToModel<TEntity, TModel>(this TEntity entity, TModel model)
            where TEntity : BaseEntity where TModel : BaseEntityDTO
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return entity.MapTo(model);
        }
        public static TEntity ToEntity<TEntity>(this BaseEntityDTO model) where TEntity : BaseEntity
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return model.Map<TEntity>();
        }

        public static TEntity ToEntity<TEntity, TModel>(this TModel model, TEntity entity)
            where TEntity : BaseEntity where TModel : BaseEntityDTO
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return model.MapTo(entity);
        }
    }
}
