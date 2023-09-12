#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 6379

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["project/project.csproj", "project/"]
COPY ["Common.Util/Common.Util.csproj", "Common.Util/"]
COPY ["DapperDal/DapperDal.csproj", "DapperDal/"]
COPY ["MessageMiddleware/MessageMiddleware.csproj", "MessageMiddleware/"]
COPY ["RepositoryComponent/RepositoryComponent.csproj", "RepositoryComponent/"]
RUN dotnet restore "project/project.csproj"
COPY . .
WORKDIR "/src/project"
RUN dotnet build "project.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "project.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "project.dll"]