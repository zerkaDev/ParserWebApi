#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Timetable.WepApi/Timetable.WepApi.csproj", "Timetable.WepApi/"]
COPY ["Timetable.Persistance/Timetable.Persistance.csproj", "Timetable.Persistance/"]
COPY ["Timetable.Application/Timetable.Application.csproj", "Timetable.Application/"]
COPY ["Timetable.Domain/Timetable.Domain.csproj", "Timetable.Domain/"]
RUN dotnet restore "Timetable.WepApi/Timetable.WepApi.csproj"
COPY . .
WORKDIR "/src/Timetable.WepApi"
RUN dotnet build "Timetable.WepApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Timetable.WepApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Timetable.WepApi.dll"]