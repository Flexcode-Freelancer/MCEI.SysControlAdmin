#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MCEI.SysControlAdmin.WebApp/MCEI.SysControlAdmin.WebApp.csproj", "MCEI.SysControlAdmin.WebApp/"]
COPY ["MCEI.SysControlAdmin.BL/MCEI.SysControlAdmin.BL.csproj", "MCEI.SysControlAdmin.BL/"]
COPY ["MCEI.SysControlAdmin.DAL/MCEI.SysControlAdmin.DAL.csproj", "MCEI.SysControlAdmin.DAL/"]
COPY ["MCEI.SysControlAdmin.EN/MCEI.SysControlAdmin.EN.csproj", "MCEI.SysControlAdmin.EN/"]
RUN dotnet restore "./MCEI.SysControlAdmin.WebApp/./MCEI.SysControlAdmin.WebApp.csproj"
COPY . .
WORKDIR "/src/MCEI.SysControlAdmin.WebApp"
RUN dotnet build "./MCEI.SysControlAdmin.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MCEI.SysControlAdmin.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MCEI.SysControlAdmin.WebApp.dll"]