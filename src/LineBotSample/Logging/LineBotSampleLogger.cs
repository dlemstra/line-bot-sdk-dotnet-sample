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

using Line;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LineBotSample
{
    public class LineBotSampleLogger : ILineBotLogger
    {
        public async Task LogApiCall(Uri uri, HttpContent httpContent)
        {
            var postedData = string.Empty;
            if (httpContent != null)
            {
                var bytes = await httpContent.ReadAsByteArrayAsync();
                postedData = $"PostedData: {Encoding.UTF8.GetString(bytes)}{Environment.NewLine}";
            }

            Console.WriteLine($"Request to: {uri}{Environment.NewLine}{postedData}");
        }

        public Task LogReceivedEvents(byte[] eventsData)
        {
            Console.WriteLine($"Events received: {Encoding.UTF8.GetString(eventsData)}");

            return Task.CompletedTask;
        }
    }
}
