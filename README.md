# Chat Application
A chat application using .NET and Angular.
This application allow users to talk in a chatroom and also to get stock quotes
from an API using a specific command.

## Author
### Darlan Damasio
- [github](https://github.com/damasio34)
- [likedin](https://www.linkedin.com/in/damasio34/)

### Requiriments
- .NET CLI;
- .NET SDK 5.0+;
- Node and NPM;
- Angular 13+;
- Docker.

### Mandatory Features
- [x] Allow registered users to log in and talk with other users in a chatroom;
- [x] Allow users to post messages as commands into the chatroom with the following format /stock=stock_code;
- [x] Create a decoupled bot that will call an API using the stock_code as a parameter (https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the stock_code);
- [x] The bot should parse the received CSV file and then it should send a message back into the chatroom using a message broker like RabbitMQ. The message will be a stock quote using the following format: “APPL.US quote is $93.42 per share”. The post owner will be the bot.
- [x] Have the chat messages ordered by their timestamps and show only the last 50
messages.
- [x] Unit test the functionality you prefer.

### Bonus (Optional)
- [x] Have more than one chatroom;
- [ ] Use .NET identity for users authentication;
- [ ] Handle messages that are not understood or any exceptions raised within the bot;
- [ ] Build an installer.

### Install

```cmd
cd .\chat-application\
npm install
cd ..
dotnet restore
cd ..
cd .\Resources\
.\install-image-docker.cmd
```

### Test

```bash
dotnet test
```
### Run

#### Backend StockBot
```cmd
dotnet build .\StockBot\StockBot.csproj -c release
.\StockBot\bin\release\net5.0\ChatApplication.StockBot.exe
```

#### Backend API
```cmd
dotnet run -c release --project .\API\API.csproj --urls=https://localhost:44328/
```

#### Frontend
```cmd
cd .\chat-application\
npm run start
```

### References

 - [Background tasks with hosted services in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio)
 - [Rabbitmq Tutorials](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html)
 - [Get started with ASP.NET Core SignalR using TypeScript and Webpack](https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr-typescript-webpack?view=aspnetcore-6.0&tabs=visual-studio)
