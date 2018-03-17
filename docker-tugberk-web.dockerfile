FROM microsoft/dotnet:2-sdk

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

WORKDIR /app/Tugberk.Web/
RUN dotnet publish -c $BUILDCONFIG -o out

EXPOSE 5000
ENTRYPOINT ["dotnet", "out/Tugberk.Web.dll"]
