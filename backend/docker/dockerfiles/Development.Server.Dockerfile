FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY . .

RUN dotnet restore src/Application/Application.csproj
RUN dotnet publish src/Application/Application.csproj -c Release -o out
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /app/out

COPY config ../config

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:7777
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "Application.dll"]