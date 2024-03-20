FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /App
COPY --from=b /App/bin/Release/net7.0/linux-x64/publish /App

ENTRYPOINT ["dotnet", "ADOPSE.dll"]