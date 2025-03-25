# 1. Base stage: Use the aspnet image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# 2. Build stage: Use the sdk image as the base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/UserManagementService/UserStore/UserStore.csproj", "UserStore/"]
COPY ["/UserManagementService/UserData/UserData.csproj", "UserData/"]
RUN dotnet restore "UserStore/UserStore.csproj"
COPY . .
WORKDIR "/src/UserStore"
RUN dotnet build "UserStore.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publish stage: Use the sdk image as the base image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserStore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Final stage: Use the aspnet image as the base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserStore.dll"]
