using AutoMapper;
using DealWatcher.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealWatcher
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            ConfigureProduct();
        }

        private static void ConfigureProduct()
        {
            Mapper.CreateMap<Product, ProductViewModel>();
            Mapper.CreateMap<ProductPrice, ProductPriceViewModel>();
            Mapper.CreateMap<ProductImage, ProductImageViewModel>();
            Mapper.CreateMap<ProductCode, ProductCodeViewModel>()
                .ForMember(vm => vm.Type, m => m.MapFrom(model => model.ProductCodeType.Type));

            Mapper.CreateMap<IList<Product>, IList<ProductViewModel>>();
            Mapper.CreateMap<IList<ProductPrice>, IList<ProductPriceViewModel>>();
            Mapper.CreateMap<IList<ProductImage>, IList<ProductImageViewModel>>();
            Mapper.CreateMap<IList<ProductCode>, IList<ProductCodeViewModel>>();

            Mapper.CreateMap<ProductSearchBindingModel, ProductSearchViewModel>();

            Mapper.CreateMap<Configuration, ConfigurationViewModel>();
        }
    }
}