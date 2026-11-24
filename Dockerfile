FROM mcr.microsoft.com/dotnet/sdk:11.0-alpine AS build
WORKDIR /src
COPY . /src/
RUN dotnet publish "BookwormAPI.csproj" -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:11.0-alpine
WORKDIR /app
COPY --from=build /publish /app/

EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Development

# CMD [ "dotnet", "BookwormAPI.dll" ]

ENTRYPOINT [ "dotnet", "BookwormAPI.dll" ]
