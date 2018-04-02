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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Line;
using LineBotSample.EventHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LineBotSample
{
    public class LineBotService
    {
        private readonly ILineBot _lineBot;
        private readonly IServiceProvider _serviceProvider;

        public LineBotService(ILineBot lineBot, IServiceProvider serviceProvider)
        {
            _lineBot = lineBot;
            _serviceProvider = serviceProvider;
        }

        public async Task Handle(HttpContext context)
        {
            if (context.Request.Method != HttpMethods.Post)
                return;

            var events = await _lineBot.GetEvents(context.Request);
            foreach (var evt in events)
            {
                foreach (var eventHandler in GetEventHandlers(evt.EventType))
                {
                    await eventHandler.Handle(_lineBot, evt);
                }
            }
        }

        private IEnumerable<ILineEventHandler> GetEventHandlers(LineEventType eventType)
        {
            return _serviceProvider.GetServices<ILineEventHandler>()
                .Where(_ => _.EventType == eventType);
        }
    }
}
