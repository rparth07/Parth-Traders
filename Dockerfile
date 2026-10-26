FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
COPY Backend/ Backend/
RUN dotnet publish "Backend/Parth Traders/Parth Traders.API.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000
USER $APP_UID
ENTRYPOINT ["dotnet", "Parth Traders.API.dll"]
