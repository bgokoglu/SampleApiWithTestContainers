FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
# Copy the project file
COPY ["SampleApiWithTestContainers/SampleApiWithTestContainers.csproj", "SampleApiWithTestContainers/"]
# Restore NuGet packages
RUN dotnet restore "SampleApiWithTestContainers/SampleApiWithTestContainers.csproj"
# Copy the entire solution
COPY . .
WORKDIR "/src/SampleApiWithTestContainers"
# Build the application
RUN dotnet build "SampleApiWithTestContainers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleApiWithTestContainers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleApiWithTestContainers.dll"]