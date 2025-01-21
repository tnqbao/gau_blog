FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /gau_blog
EXPOSE 8085
EXPOSE 8086

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["gau_blog/gau_blog.csproj", "gau_blog/"]
RUN dotnet restore "gau_blog/gau_blog.csproj"
COPY . .
WORKDIR "/src/gau_blog"
RUN dotnet build "gau_blog.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "gau_blog.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gau_blog.dll"]
