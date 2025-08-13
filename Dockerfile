
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["ZooApi/ZooApi.csproj", "ZooApi/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["LibraryAnimals/LibraryAnimals.csproj", "LibraryAnimals/"] 


RUN dotnet restore "ZooApi/ZooApi.csproj"


COPY . .


WORKDIR "/src/ZooApi"
RUN dotnet publish "ZooApi.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ZooApi.dll"]