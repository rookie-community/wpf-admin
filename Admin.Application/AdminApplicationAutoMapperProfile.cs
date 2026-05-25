using AutoMapper;

namespace Admin
{
    public class AdminApplicationAutoMapperProfile : Profile
    {
        public AdminApplicationAutoMapperProfile()
        {
            // 创建从 Product 到 ProductDto 的映射
            //CreateMap<Product, ProductDto>()
            //    .ForMember(dest => dest.DisplayPrice, opt => opt.MapFrom(src => $"${src.Price:F2}")); // 自定义映射规则
        }
    }
}
