using AutoMapper;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;

namespace PersonalFinanceManagement.API.Profiles;

public class RevenueProfile : Profile
{
    public RevenueProfile()
    {
        CreateMap<CreateRevenueDTO, Revenue>();
        CreateMap<Revenue, CreateRevenueDTO>();
    }
}
