namespace RecipeManagement.Extensions.Services;

using RecipeManagement.Services;
using RecipeManagement.Middleware;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sieve.Services;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public static class WebApiServiceExtension
{
    public static void AddWebApiServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        services.AddMediatR(typeof(Startup));
        services.AddScoped<SieveProcessor>();
        services.AddMvc(options => options.Filters.Add<ErrorHandlerFilterAttribute>())
            .AddFluentValidation(cfg => { cfg.AutomaticValidationEnabled = false; });
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}