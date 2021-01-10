
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

ARG VERSION=0.1.0

EXPOSE 80

COPY AuthenticationClientService.csproj /build/
RUN dotnet restore ./build/AuthenticationClientService.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./AuthenticationClientService.csproj -c Release -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "AuthenticationClientService.dll"]