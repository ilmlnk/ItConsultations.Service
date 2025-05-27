FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ItConsultations.Service/ItConsultations.sln ./
COPY ItConsultations.Service/ItConsultations/ItConsultations.WebApi.csproj ./ItConsultations/
COPY ItConsultations.Service/ItConsultations.Logger/ItConsultations.Logger.csproj ./ItConsultations.Logger/
COPY ItConsultations.Service/ItConsultations.Utilities/ItConsultations.Utilities.csproj ./ItConsultations.Utilities/
COPY ItConsultations.Service/ItConsultations.Business/ItConsultations.Business.csproj ./ItConsultations.Business/
COPY ItConsultations.Service/ItConsultations.DataAccess/ItConsultations.DataAccess.csproj ./ItConsultations.DataAccess/
COPY ItConsultations.Service/ItConsultations.Tests/ItConsultations.Tests.csproj ./ItConsultations.Tests/
RUN dotnet restore 

COPY ItConsultations.Service/. .

RUN dotnet publish ItConsultations/ItConsultations.WebApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ItConsultations.WebApi.dll"]