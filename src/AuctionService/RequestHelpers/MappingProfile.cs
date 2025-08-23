using System;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.RequestHelpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src));
        CreateMap<CreateAuctionDto, Item>();
        CreateMap<UpdateAuctionDto, Auction>();

    }
}
