using AutoMapper;
using BookingSite.Domains.Models;
using BookingSite.ViewModels;
using Route = BookingSite.Domains.Models.Route;

namespace BookingSite.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
            // Airport -> AirportViewModel
            CreateMap<Airport, AirportViewModel>()
                .ForMember(dest => dest.CityName, 
                           opt => opt.MapFrom(src => src.City != null ? src.City.Name : null));
        
            // Booking -> BookingViewModel

            // City -> CityViewModel
            CreateMap<City, CityViewModel>();

            // Flight -> FlightViewModel
            CreateMap<Flight, FlightViewModel>()
                .ForMember(dest => dest.FromAirport, opt => opt.MapFrom(
                    src => src.Route.FromAirport.Name))
                .ForMember(dest => dest.ToAirport, opt => opt.MapFrom(
                    src => src.Route.ToAirport.Name))
                .ForMember(dest => dest.ToAirportId, opt => opt.MapFrom(
                    src => src.Route.ToAirport.Id))
                .ForMember(dest => dest.RouteSegments, opt => opt.MapFrom(
                    src => string.Join(" -> ", src.Route.RouteSegments.Select(r => r.Airport.Name))))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(
                    src => src.Route.Price));
            


            // MealChoice -> MealChoiceViewModelJ
            CreateMap<MealChoice, MealChoiceViewModel>()
                .ForMember(dest => dest.AirportName, 
                           opt => opt.MapFrom(src => src.Airport ));

            // Route -> RouteViewModel
            CreateMap<Route, RouteViewModel>()
                .ForMember(dest => dest.FromAirportName,
                    opt => opt.MapFrom(src => src.FromAirport.Name))
                .ForMember(dest => dest.ToAirportName,
                    opt => opt.MapFrom(src => src.ToAirport.Name));

            // RouteSegment -> RouteSegmentViewModel
            CreateMap<RouteSegment, RouteSegmentViewModel>()
                .ForMember(dest => dest.AirportName,
                           opt => opt.MapFrom(src => src.Airport != null ? src.Airport.Name : null));

            // Season -> SeasonViewModel
            CreateMap<Season, SeasonViewModel>()
                .ForMember(dest => dest.AirportName,
                           opt => opt.MapFrom(src => src.Airport != null ? src.Airport.Name : null));

            CreateMap<Ticket, TicketViewModel>()
                .ForMember(dest => dest.FlightInfo,
                           opt => opt.MapFrom(src => src.Flight != null 
                                    ? $"{src.Flight.Id} ({src.Flight.Date.ToString()})" 
                                    : string.Empty))
                .ForMember(dest => dest.MealType,
                           opt => opt.MapFrom(src => src.Meal != null ? src.Meal.Type : null))
                .ForMember(dest => dest.SeatNumber,
                           opt => opt.MapFrom(src => src.SeatNumber.HasValue ? src.SeatNumber.ToString() : "Not assigned"));

            // AspNetUser -> AspNetUserViewModel
            CreateMap<AspNetUser, AspNetUserViewModel>()
                // Map the domain's "LastnNme" property to a ViewModel "LastName" property
                .ForMember(dest => dest.LastName,
                    opt => opt.MapFrom(src => src.LastnNme));
            // Map the Roles collection to a list of role names
    }

}