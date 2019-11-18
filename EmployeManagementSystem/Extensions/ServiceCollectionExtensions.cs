﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmployeManagementSystem.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddByConvention(this IServiceCollection services, Assembly assembly, Func<Type, bool> interfacePredicate, Func<Type, bool> implementationPredicate)
        {
            var interfaces = assembly.ExportedTypes
                .Where(x => x.IsInterface && interfacePredicate(x))
                .ToList();
            var implementations = assembly.ExportedTypes
                .Where(x => !x.IsInterface && !x.IsAbstract && implementationPredicate(x))
                .ToList();
            foreach (var @interface in interfaces)
            {
                var implementation = implementations.FirstOrDefault(x => @interface.IsAssignableFrom(x));
                if (implementation == null) continue;
                services.AddTransient(@interface, implementation); 
            }
            return services;
        }

        public static IServiceCollection AddByConvention(this IServiceCollection services, Assembly assembly, Func<Type, bool> predicate)
            => services.AddByConvention(assembly, predicate, predicate);

    }
}
