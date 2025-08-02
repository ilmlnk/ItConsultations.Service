FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ItConsultations.sln ./
COPY ItConsultations/ItConsultations.WebApi.csproj ./ItConsultations/
COPY ItConsultations.Logger/ItConsultations.Logger.csproj ./ItConsultations.Logger/
COPY ItConsultations.Utilities/ItConsultations.Utilities.csproj ./ItConsultations.Utilities/
COPY ItConsultations.Business/ItConsultations.Business.csproj ./ItConsultations.Business/
COPY ItConsultations.DataAccess/ItConsultations.DataAccess.csproj ./ItConsultations.DataAccess/
COPY ItConsultations.Tests/ItConsultations.Tests.csproj ./ItConsultations.Tests/

COPY . .
RUN dotnet restore 
RUN dotnet publish ItConsultations/ItConsultations.WebApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ItConsultations.WebApi.dll"]