# Use the official .NET 8 SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and restore dependencies
COPY Br1InterviewPreparation.sln ./
COPY Br1InterviewPreparation.API/*.csproj ./Br1InterviewPreparation.API/
COPY Br1InterviewPreparation.Application/*.csproj ./Br1InterviewPreparation.Application/
COPY Br1InterviewPreparation.Domain/*.csproj ./Br1InterviewPreparation.Domain/
COPY Br1InterviewPreparation.Infrastructure/*.csproj ./Br1InterviewPreparation.Infrastructure/
COPY Br1InterviewPreparation.Tests/*.csproj ./Br1InterviewPreparation.Tests/

RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet publish Br1InterviewPreparation.API/Br1InterviewPreparation.API.csproj -c Release -o out

# Use the official .NET 8 runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose port 80
EXPOSE 80

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80

# Start the application
ENTRYPOINT ["dotnet", "Br1InterviewPreparation.API.dll"]