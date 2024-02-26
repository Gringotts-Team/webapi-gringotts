using AutoMapper;
using gringotts_application.DTOs;
using gringotts_application.Models;


namespace gringotts_application
{
    /// <summary>
    /// Configures object-to-object mappings using AutoMapper.
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the AutoMapper class and defines object mappings.
        /// </summary>
        public MapperProfile()
        {
            /// <summary>
            /// Configures mapping from MageModel to MageDTO.
            /// </summary>
            CreateMap<MageModel, MageDTO>();
            
            /// <summary>
            /// Configures mapping from MageDTO to MageModel.
            /// </summary>
            CreateMap<MageDTO, MageModel>();

            /// <summary>
            /// Configures mapping from MageDTO to MageListModel, specifying how the mag_house property is mapped.
            /// </summary>
            CreateMap<MageDTO, MageListModel>().ForMember(dest => dest.mag_house, opt => opt.MapFrom(src => src.mag_hou_id)); ;

        }
    }
}
