# Copyright 2018 Dirk Lemstra (https://github.com/dlemstra/line-bot-sdk-dotnet)
#
# Dirk Lemstra licenses this file to you under the Apache License,
# version 2.0 (the "License"); you may not use this file except in compliance
# with the License. You may obtain a copy of the License at:
#
#   https://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
# WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
# License for the specific language governing permissions and limitations
# under the License.

FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY LineBotSample.sln ./
COPY src/LineBotSample/LineBotSample.csproj src/LineBotSample/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/src/LineBotSample
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LineBotSample.dll"]
