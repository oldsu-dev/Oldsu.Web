FROM mcr.microsoft.com/dotnet/sdk:5.0

EXPOSE 8080

WORKDIR /usr/src/app
COPY . .

RUN mkdir Oldsu.Web/avatars

ENV ASPNETCORE_URLS=http://+:8080
RUN dotnet publish -c Release --output ./dist Oldsu.Web.sln

WORKDIR /usr/src/app/dist

CMD ["./Oldsu.Web"]


