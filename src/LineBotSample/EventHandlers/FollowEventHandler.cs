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
using System.Threading.Tasks;
using Line;
using LineBotSample.Configuration;

namespace LineBotSample.EventHandlers
{
    public class FollowEventHandler : ILineEventHandler
    {
        private readonly LineBotSampleConfiguration configuration;

        public LineEventType EventType => LineEventType.Follow;

        public FollowEventHandler(LineBotSampleConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task Handle(ILineBot lineBot, ILineEvent evt)
        {
            string userName = "[UNKNOW USER]";

            try
            {
                var user = await lineBot.GetProfile(evt.Source.User);
                userName = $"{user.DisplayName} ({user.UserId})";
            }
            catch (LineBotException)
            {
            }
            catch (Exception)
            {
            }

            var response = new TextMessage($"Welcome, {userName} !");

            await lineBot.Reply(evt.ReplyToken, response);
        }
    }
}
