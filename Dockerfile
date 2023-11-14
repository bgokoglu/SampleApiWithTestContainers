FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
# Copy the project file
COPY ["SampleApi/SampleApi.csproj", "SampleApi/"]
# Restore NuGet packages
RUN dotnet restore "SampleApi/SampleApi.csproj"
# Copy the entire solution
COPY . .
WORKDIR "/src/SampleApi"
# Build the application
RUN dotnet build "SampleApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleApi.dll"]