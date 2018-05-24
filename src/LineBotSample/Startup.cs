// Copyright 2018 Dirk Lemstra (https://github.com/dlemstra/line-bot-sdk-dotnet)
//
// Dirk Lemstra licenses this file to you under the Apache License,
// version 2.0 (the "License"); you may not use this file except in compliance
// with the License. You may obtain a copy of the License at:
//
//   https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations
// under the License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Line;
using LineBotSample.EventHandlers;
using LineBotSample.Configuration;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace LineBotSample
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ILineConfiguration, LineConfiguration>()
                .AddSingleton<ILineBot, LineBot>();

            services
                .AddSingleton<ILineBotLogger, LineBotSampleLogger>()
                .AddSingleton<LineBotSampleConfiguration>()
                .AddSingleton<LineBotService>();

            services
                .AddSingleton<ILineEventHandler, MessageEventHandler>()
                .AddSingleton<ILineEventHandler, FollowEventHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var lineConfiguration = (LineConfiguration)app.ApplicationServices.GetRequiredService<ILineConfiguration>();
            _configuration.Bind("LineConfiguration", lineConfiguration);

            var lineBotSampleConfiguration = app.ApplicationServices.GetRequiredService<LineBotSampleConfiguration>();
            _configuration.Bind("LineBotSampleConfiguration", lineBotSampleConfiguration);

            var lineBotService = app.ApplicationServices.GetRequiredService<LineBotService>();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = "/Resources"
            });

            app.Run(async (context) =>
            {
                await lineBotService.Handle(context);
            });
        }
    }
}
