# Use Windows-compatible .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR /app

# Copy solution and projects
COPY FarmTrade.sln .  
COPY FarmApi/FarmApi.csproj FarmApi/  
COPY FarmBusiness/FarmBusiness.csproj FarmBusiness/  
COPY FarmTradeDataLayer/FarmTradeDataLayer.csproj FarmTradeDataLayer/  
COPY FarmTradeEntity/FarmTradeEntity.csproj FarmTradeEntity/  

# Restore dependencies
RUN dotnet restore FarmApi/FarmApi.csproj

# Copy full source code and build
COPY . .
RUN dotnet publish FarmApi/FarmApi.csproj -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022 AS runtime
WORKDIR /app
COPY --from=build /publish .

# Expose port and set entry point
EXPOSE 8080
ENTRYPOINT ["dotnet", "FarmApi.dll"]
