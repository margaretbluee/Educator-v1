FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN dotnet tool install --version 7.0.0 --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
ENV ASPNETCORE_ENVIRONMENT=Deployment

RUN mkdir /App

COPY ADOPSE.csproj /App

WORKDIR /App
RUN dotnet restore

COPY . /App

RUN dotnet publish -c Release -r linux-x64

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /App
COPY --from=build /App/bin/Release/net7.0/linux-x64/publish /App

ENTRYPOINT ["dotnet", "ADOPSE.dll"]