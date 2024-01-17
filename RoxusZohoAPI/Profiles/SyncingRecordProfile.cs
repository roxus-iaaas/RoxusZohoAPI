using AutoMapper;
using RoxusZohoAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Profiles
{
    public class SyncingRecordProfile : Profile
    {
        public SyncingRecordProfile()
        {
            CreateMap<Models.TrenchesReporting.SyncingForCreation, Entities.TrenchesReportDB.SyncingRecord>()
                .ForMember(dest => dest.ZohoDate,
                opt => opt.MapFrom(src => DateTimeHelpers.UnixTimestampToDateTime(src.ZohoDate)));
        }
    }
}
