# stage 1 (BUILD)

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# SDK -> Software Development Kit

WORKDIR /src

ENV PATH="$PATH:/root/.dotnet/tools"

# like .csproj = package.json pacjage-lock.json ... *.json
COPY *.csproj .

# like npm install
RUN dotnet restore

# like -> npm run build
COPY . .

# stage 2 (RUN)
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT [ "sh", "-c", "dotnet api.dll" ]

