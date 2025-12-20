
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY ["ZooApi.Source/WebApiAnimal/WebApiAnimal.csproj", "ZooApi.Source/WebApiAnimal/"]
COPY ["ZooApi.Source/DomainAnimal/DomainAnimal.csproj", "ZooApi.Source/DomainAnimal/"]
COPY ["ZooApi.Source/ApplicationAnimal/ApplicationAnimal.csproj", "ZooApi.Source/ApplicationAnimal/"] 
COPY ["ZooApi.Source/Shared/Shared.csproj", "ZooApi.Source/Shared/"] 


RUN dotnet restore "ZooApi.Source/WebApiAnimal/WebApiAnimal.csproj"


COPY . .


WORKDIR "/src/ZooApi.Source/WebApiAnimal"
RUN dotnet publish "WebApiAnimal.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApiAnimal.dll"]