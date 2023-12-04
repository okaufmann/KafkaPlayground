# Kafka Playground

Kafka Playground is a .NET-based interactive application for experimenting with Apache Kafka. It allows users to either produce messages to or consume messages from a Kafka topic. The solution is divided into separate libraries for Kafka message production and consumption, encapsulating Kafka functionalities in a modular and reusable manner.

## Prerequisites

- .NET 8.0 SDK or later
- Apache Kafka Server running locally or accessible remotely

## Project Structure

- `KafkaClient`: The main application for user interaction in the Kafka Playground.
- `Producer`: Library handling Kafka message production.
- `Consumer`: Library handling Kafka message consumption.

## Setup

1. **Clone the Repository**

   ```sh
   git clone [Your-Repository-URL]
   cd KafkaPlayground
   ```

2. **Build the Solution**

   Navigate to the root directory of the solution and run:

   ```sh
   dotnet build
   ```

## Running the Application

1. **Start the Kafka Server**

   Ensure your Kafka server is up and running. If you're running a local Kafka server, follow the instructions provided with your Kafka distribution to start the server.

2. **Run KafkaClient**

   Navigate to the `KafkaClient` project directory and execute:

   ```sh
   dotnet run
   ```

   Follow the on-screen instructions to choose between producer and consumer modes.

## Configurations

- Kafka configurations can be adjusted in `appsettings.json` within the `KafkaClient` project.
- Ensure that the Kafka broker addresses and topic names are correctly specified.

## Testing

- To run unit tests, navigate to the `tests` directory and run:

  ```sh
  dotnet test
  ```


