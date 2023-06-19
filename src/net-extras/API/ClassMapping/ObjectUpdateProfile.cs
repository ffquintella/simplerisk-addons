﻿using AutoMapper;
using DAL.Entities;

namespace API.ClassMapping;

public class ObjectUpdateProfile: Profile
{
    public ObjectUpdateProfile()
    {
        CreateMap<Risk, Risk>();
    }
}