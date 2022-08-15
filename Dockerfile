FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /build

COPY ["src/Aopa.Suporte.gRPC/Aopa.Suporte.gRPC.csproj", "src/Aopa.Suporte.gRPC/"]

RUN dotnet restore "src/Aopa.Suporte.gRPC/Aopa.Suporte.gRPC.csproj"
COPY . .
WORKDIR /build/src/Aopa.Suporte.gRPC

FROM build AS publish
RUN dotnet publish "Aopa.Suporte.gRPC.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS app
# RUN apk --no-cache add tzdata
# ENV TZ=America/Sao_Paulo

WORKDIR /app
COPY --from=publish /publish .

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "Aopa.Suporte.gRPC.dll", "--urls", "http://+:80"]