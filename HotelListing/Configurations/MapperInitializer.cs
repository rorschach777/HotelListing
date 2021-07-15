using System;
using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
namespace HotelListing.Configurations
{
    public class MapperInitializer : Profile
    {

        public MapperInitializer()
        {
            // All the mappings occur here.

            // Country Data have direct correlation to CountryDTO
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, CreateCountryDTO>().ReverseMap();
            CreateMap<Country, UpdateCountryDTO>().ReverseMap();
            // Hotel Data have direct correlation to HotelDTO
            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDTO>().ReverseMap();
            // Users: 
            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}
