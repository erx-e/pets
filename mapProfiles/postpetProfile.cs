using System;
using AutoMapper;
using mascotas.Models;

namespace mascotas.Profiles
{
    public class postpetProfile : Profile
    {
        public postpetProfile(){
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<UpdatePostPetDTO, PostPet>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));;
        }
    }
}