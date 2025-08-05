FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app

COPY . ./
RUN dotnet restore

RUN dotnet publish WebApp/WebApp.csproj -c Release -o out

EXPOSE 8080

WORKDIR /app/out

ENTRYPOINT ["dotnet", "WebApp.dll"]