
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY *.sln .
COPY KoffieOfNie/*.csproj ./KoffieOfNie/
COPY BLL/*.csproj ./BLL/
RUN dotnet restore

COPY . ./
WORKDIR /app/KoffieOfNie
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build-env /app/KoffieOfNie/out ./
EXPOSE 80
ENTRYPOINT ["dotnet","KoffieOfNie.dll"]
