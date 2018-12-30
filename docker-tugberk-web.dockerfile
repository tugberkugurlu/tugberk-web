# Stage 1 - the build process
FROM node:8 as build-deps

COPY ./src/Tugberk.Web/wwwroot/package.json /app-temp/wwwroot/
COPY ./src/Tugberk.Web/wwwroot/package-lock.json /app-temp/wwwroot/

WORKDIR /app-temp/wwwroot
RUN npm install

# Stage 2 - the production environment
FROM microsoft/dotnet:2.2.100-sdk

ARG BUILDCONFIG=DEBUG

COPY ./src/Tugberk.Web/Tugberk.Web.csproj /app/Tugberk.Web/
COPY ./src/Tugberk.Persistance.SqlServer/Tugberk.Persistance.SqlServer.csproj /app/Tugberk.Persistance.SqlServer/
COPY ./src/Tugberk.Persistance.InMemory/Tugberk.Persistance.InMemory.csproj /app/Tugberk.Persistance.InMemory/
COPY ./src/Tugberk.Persistance.Abstractions/Tugberk.Persistance.Abstractions.csproj /app/Tugberk.Persistance.Abstractions/
COPY ./src/Tugberk.Domain/Tugberk.Domain.csproj /app/Tugberk.Domain/

WORKDIR /app/
RUN dotnet --info
RUN dotnet restore Tugberk.Web/Tugberk.Web.csproj
ADD ./src/ /app/
COPY --from=build-deps /app-temp/wwwroot/node_modules /app/Tugberk.Web/wwwroot/node_modules

WORKDIR /app/Tugberk.Web/
RUN dotnet publish -c $BUILDCONFIG -o out

EXPOSE 80
ENTRYPOINT ["dotnet", "out/Tugberk.Web.dll"]
