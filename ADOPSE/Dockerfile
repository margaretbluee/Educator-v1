#FROM mcr.microsoft.com/dotnet/core/sdk:7.0 AS build
#WORKDIR /src

#COPY ["ADOPSE/ADOPSE.csproj", "ADOPSE/"]
#RUN dotnet restore "adopseWEB/ADOPSE.csproj"

#COPY . .
#WORKDIR "/src/ADOPSE"
#RUN dotnet build "ADOPSE.csproj" -c Release -o /app/build

#FROM build AS publish
#RUN dotnet publish "ADOPSE.csproj" -c Release -o /app/publish --no-restore

#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "ADOPSE.dll"]



FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /App

ENTRYPOINT ["dotnet", "ADOPSE.dll"]