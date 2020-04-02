﻿using Microsoft.Extensions.Configuration;
using PhotoStorage.WindowsService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PhotoStorage.WindowsService.Configuration
{
    public class ConfigurationService
    {
        public AppSettings Load(string path)
        {
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json");

            var appSettings = new AppSettings();

            var configuration = builder.Build();
            appSettings.ConnectionString = configuration["ConnectionStrings:netcodephotostorage"];
            appSettings.MonitorPath = configuration["MonitorPath"];

            return appSettings;
        }

    }
}
