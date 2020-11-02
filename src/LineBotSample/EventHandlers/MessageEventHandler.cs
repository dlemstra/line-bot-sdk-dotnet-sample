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
    public class MessageEventHandler : ILineEventHandler
    {
        private readonly LineBotSampleConfiguration configuration;

        public LineEventType EventType
            => LineEventType.Message;

        public MessageEventHandler(LineBotSampleConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task Handle(ILineBot lineBot, ILineEvent evt)
        {
            if (string.IsNullOrEmpty(evt.Message.Text))
                return;

            // The Webhook URL verification uses these invalid token.
            if (evt.ReplyToken == "00000000000000000000000000000000" || evt.ReplyToken == "ffffffffffffffffffffffffffffffff")
                return;

            if (evt.Message.Text.ToLowerInvariant().Contains("ping"))
            {
                var response = new TextMessage($"pong");

                await lineBot.Reply(evt.ReplyToken, response);
            }
            else if (evt.Message.Text.ToLowerInvariant().Contains("user"))
            {
                var userName = evt.Source.User.Id;
                try
                {
                    var user = await lineBot.GetProfile(evt.Source.User);
                    userName = $"{user.DisplayName} ({user.UserId})";
                }
                catch (LineBotException)
                {
                }

                var response = new TextMessage($"You are: {userName}");

                await lineBot.Reply(evt.ReplyToken, response);
            }
            else if (evt.Message.Text.ToLowerInvariant().Contains("logo"))
            {
                var logoUrl = this.configuration.ResourcesUrl + "/Images/Line.Bot.SDK.png";

                Console.WriteLine(logoUrl);
                var response = new ImageMessage(logoUrl, logoUrl);

                await lineBot.Reply(evt.ReplyToken, response);
            }
        }
    }
}
