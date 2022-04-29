namespace RecipeManagement.Extensions.Services;

using RecipeManagement.Resources;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public static class CorsServiceExtension
{
    public static void AddCorsService(this IServiceCollection services, string policyName, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsEnvironment(LocalConfig.IntegrationTestingEnvName) ||
            env.IsEnvironment(LocalConfig.FunctionalTestingEnvName))
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder => 
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
            });
        }
        else
        {
            //TODO update origins here with env vars or secret
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(policyName, builder =>
            //        builder.WithOrigins(origins)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .WithExposedHeaders("X-Pagination"));
            //});
        }
    }
}