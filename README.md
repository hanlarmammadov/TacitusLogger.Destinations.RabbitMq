# TacitusLogger.Destinations.RabbitMq

> Extension destination for TacitusLogger that sends logs to the RabbitMQ exchanges.
 
Dependencies:  
* NET Standard >= 1.5   
* TacitusLogger >= 0.3.0  
* RabbitMQ.Client >= 4.0.0
  
> Attention: `TacitusLogger.Destinations.RabbitMq` is currently in **Alpha phase**. This means you should not use it in any production code.

## Installation

The NuGet <a href="https://www.nuget.org/packages/TacitusLogger.Destinations.RabbitMq" target="_blank">package</a>:

```powershell
PM> Install-Package TacitusLogger.Destinations.RabbitMq
```

## Examples

### Adding RabbitMq with minimal configuration
Using builders:
```cs
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .RabbitMq()
                              .WithConnection(connection)
                              .WithAddress("exchangeName", "exchangeType", "routingKey")
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, publicationAddressProvider);

Logger logger = new Logger();
logger.AddLogDestinations(rabbitMqDestination);
```
---

### With custom publication address provider
Using builders:
```cs
IPublicationAddressProvider customPublicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;

var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                              .RabbitMq()
                              .WithConnection(connection)
                              .WithAddress(customPublicationAddressProvider)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IPublicationAddressProvider customPublicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, customPublicationAddressProvider);

Logger logger = new Logger();
logger.AddLogDestinations(rabbitMqDestination);
```
---
### With publication address function
Using builders:
```cs
LogModelFunc<PublicationAddress> publicationAddressFunc = (logModel) =>
{
    if (logModel.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical))
        return new PublicationAddress("direct", "ErrorLogs", "");
    else
        return new PublicationAddress("direct", "InfoLogs", "");
};
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                              .RabbitMq()
                              .WithConnection(connection)
                              .WithAddress(publicationAddressFunc)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
FactoryMethodPublicationAddressProvider factoryMethodPublicationAddressProvider = new FactoryMethodPublicationAddressProvider((logModel) =>
{
    if (logModel.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical))
        return new PublicationAddress("direct", "ErrorLogs", "");
    else
        return new PublicationAddress("direct", "InfoLogs", "");
});
RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, factoryMethodPublicationAddressProvider);

Logger logger = new Logger();
logger.AddLogDestinations(rabbitMqDestination);
```
---
### With JSON log serializer
Using builders:
```cs
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                              .RabbitMq()
                              .WithConnection(connection)
                              .WithAddress("exchangeName", "exchangeType")
                              .WithJsonLogText()
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
ILogSerializer logSerializer = new JsonLogSerializer();
RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, 
                                                                  publicationAddressProvider, 
                                                                  logSerializer);

Logger logger = new Logger();
logger.AddLogDestinations(rabbitMqDestination);
```
---
### With custom log serializer
Using builders:
```cs
ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                              .RabbitMq()
                              .WithConnection(connection)
                              .WithAddress("exchangeName", "exchangeType")
                              .WithLogSerializer(logSerializer)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
ILogSerializer customLogSerializer = new Mock<ILogSerializer>().Object;
RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, 
                                                                  publicationAddressProvider, 
                                                                  customLogSerializer);

Logger logger = new Logger();
logger.AddLogDestinations(rabbitMqDestination);
```
---
### With basic message publisher
Using builders:
```cs
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                              .RabbitMq()
                              .WithConnection(connection)
                              .WithAddress("exchangeName", "exchangeType", "routingKey")
                              .WithPublisher(basicProperties)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
ILogSerializer logSerializer = new JsonLogSerializer();
IMessagePublisher basicMessagePublisher = new BasicMessagePublisher(basicProperties);
RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, 
                                                                  publicationAddressProvider, 
                                                                  logSerializer, 
                                                                  basicMessagePublisher);
Logger logger = new Logger();
logger.AddLogDestinations(rabbitMqDestination);
```
---
### With custom message publisher
Using builders:
```cs
IMessagePublisher customMessagePublisher = new Mock<IMessagePublisher>().Object;

var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                              .RabbitMq()
                              .WithConnection(connection)
                              .WithAddress("exchangeName", "exchangeType", "routingKey")
                              .WithPublisher(customMessagePublisher)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
ILogSerializer logSerializer = new JsonLogSerializer();
IMessagePublisher customMessagePublisher = new Mock<IMessagePublisher>().Object;
RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, 
                                                                  publicationAddressProvider, 
                                                                  logSerializer, 
                                                                  customMessagePublisher); 
Logger logger = new Logger();
logger.AddLogDestinations(rabbitMqDestination);
```
---

## License

[APACHE LICENSE 2.0](https://www.apache.org/licenses/LICENSE-2.0)

## See also

TacitusLogger:  

- [TacitusLogger](https://github.com/khanlarmammadov/TacitusLogger) - A simple yet powerful .NET logging library.

Destinations:

- [TacitusLogger.Destinations.MongoDb](https://github.com/khanlarmammadov/TacitusLogger.Destinations.MongoDb) - Extension destination for TacitusLogger that sends logs to MongoDb database.
- [TacitusLogger.Destinations.Email](https://github.com/khanlarmammadov/TacitusLogger.Destinations.Email) - Extension destination for TacitusLogger that sends logs as emails using SMTP protocol.
- [TacitusLogger.Destinations.EntityFramework](https://github.com/khanlarmammadov/TacitusLogger.Destinations.EntityFramework) - Extension destination for TacitusLogger that sends logs to database using Entity Framework ORM.
- [TacitusLogger.Destinations.Trace](https://github.com/khanlarmammadov/TacitusLogger.Destinations.Trace) - Extension destination for TacitusLogger that sends logs to System.Diagnostics.Trace listeners.  
  
Dependency injection:
- [TacitusLogger.DI.Ninject](https://github.com/khanlarmammadov/TacitusLogger.DI.Ninject) - Extension for Ninject dependency injection container that helps to configure and add TacitusLogger as a singleton.
- [TacitusLogger.DI.Autofac](https://github.com/khanlarmammadov/TacitusLogger.DI.Autofac) - Extension for Autofac dependency injection container that helps to configure and add TacitusLogger as a singleton.
- [TacitusLogger.DI.MicrosoftDI](https://github.com/khanlarmammadov/TacitusLogger.DI.MicrosoftDI) - Extension for Microsoft dependency injection container that helps to configure and add TacitusLogger as a singleton.  

Log contributors:

- [TacitusLogger.Contributors.ThreadInfo](https://github.com/khanlarmammadov/TacitusLogger.Contributors.ThreadInfo) - Extension contributor for TacitusLogger that generates additional info related to the thread on which the logger method was called.
- [TacitusLogger.Contributors.MachineInfo](https://github.com/khanlarmammadov/TacitusLogger.Contributors.MachineInfo) - Extension contributor for TacitusLogger that generates additional info related to the machine on which the log was produced.