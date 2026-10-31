# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Clinic.Domain/Clinic.Domain.csproj", "src/Clinic.Domain/"]
COPY ["src/Clinic.Application/Clinic.Application.csproj", "src/Clinic.Application/"]
COPY ["src/Clinic.Infrastructure/Clinic.Infrastructure.csproj", "src/Clinic.Infrastructure/"]
COPY ["src/Clinic.API/Clinic.API.csproj", "src/Clinic.API/"]
RUN dotnet restore "src/Clinic.API/Clinic.API.csproj"

COPY . .
WORKDIR "/src/src/Clinic.API"
RUN dotnet publish "Clinic.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Clinic.API.dll"]
