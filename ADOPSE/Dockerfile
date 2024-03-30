# Use the official Microsoft .NET Core SDK image for the build stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# Copy csproj and restore any dependencies (via NuGet)
COPY ["MyApp/MyApp.csproj", "MyApp/"]
RUN dotnet restore "MyApp/MyApp.csproj"

# Copy the project files and build the release
COPY . .
WORKDIR "/src/MyApp"
RUN dotnet build "MyApp.csproj" -c Release -o /app/build

# Publish the application to the /app/publish directory
FROM build AS publish
RUN dotnet publish "MyApp.csproj" -c Release -o /app/publish --no-restore

# Use the official Microsoft ASP.NET Core runtime image for the runtime stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyApp.dll"]





# FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# WORKDIR /App

# ENTRYPOINT ["dotnet", "ADOPSE.dll"]
