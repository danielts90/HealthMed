#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HealthMed.Patients/HealthMed.Patients.csproj", "src/HealthMed.Patients/"]
COPY ["src/HealthMed.Shared/HealthMed.Shared.csproj", "src/HealthMed.Shared/"]
RUN dotnet restore "./src/HealthMed.Patients/HealthMed.Patients.csproj"
COPY . .
WORKDIR "/src/src/HealthMed.Patients"
RUN dotnet build "./HealthMed.Patients.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthMed.Patients.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthMed.Patients.dll"]