# ----- BUILD -----
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

# Declare arguments
ARG ENV

# Change working directory
WORKDIR /app

# Copy source code
COPY ./src/ ./

# Install packages
RUN dotnet restore

# Publish to dist directory
RUN dotnet publish --no-restore -c Release -o dist

# ----- RUN -----
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as run

# Change working directory
WORKDIR /app

# Copy compiled code from build stage
COPY --from=build /app/dist/ .

# Change permission
RUN chmod +x /app/

EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

# Run application
CMD ["dotnet", "Api.dll"]