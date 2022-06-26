using ChatApplication.API;
using ChatApplication.Core.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ChatApplication.Api.Test.Hubs
{
    public class ChatHubTest
    {
		private static async Task<HubConnection> StartConnectionAsync(HttpMessageHandler handler, string hubName)
		{
			var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImJhdG1hbiIsIm5iZiI6MTY1NjI3NjIyNywiZXhwIjoxNjU2MzYyNjI3LCJpYXQiOjE2NTYyNzYyMjd9.BGudmO9v8PCMFmrJCadSghloCdt-03nYdcCZjmjFz4M";
			var webProxy = new WebProxy(new Uri("http://localhot:4201"));
			var hubConnection = new HubConnectionBuilder()
				.WithUrl($"ws://localhost/ws/{hubName}", options => {
					options.AccessTokenProvider = () => Task.FromResult(token);
					options.HttpMessageHandlerFactory = _ => handler;
					options.Proxy = webProxy;
				})
				.Build();

			await hubConnection.StartAsync();

			return hubConnection;
		}

		[Fact]
		public async Task SendMessage_Should_Send_Message_To_All_Clients()
		{
			// Arrange
			using var application = new WebApplicationFactory<Startup>();			
			using var client = application.CreateClient();
			var server = application.Server;			
			var payload = new
			{
                Username = "batman",
				Password = "batman"
			};
			var semaphore = new Semaphore(initialCount: 0, maximumCount: 1, name: "WaitReceiveMessage");
			var connection = await StartConnectionAsync(server.CreateHandler(), "chat");
			var message = default(Message);

			// Act
			await connection.InvokeAsync("AddToRoom", "testRoom");
			connection.On<Message>("receiveMessage", (chatMessage) =>
			{
				message = chatMessage;
				semaphore.Release();
			});
			await connection.InvokeAsync("SendMessageToRoom", new Message 
			{
				Username = "batman",
				Text = "Hello World!!",
				Room = "testRoom"
			});

			semaphore.WaitOne();

			//Assert
			message.Text.Should().Be("Hello World!!");
			message.Username.Should().Be("batman");
		}

		[Fact]
		public async Task SendMessage_Should_Send_Message_As_Command()
		{
			// Arrange
			using var application = new WebApplicationFactory<Startup>();
			using var client = application.CreateClient();
			var server = application.Server;
			var payload = new
			{
				Username = "robin",
				Password = "robin"
			};
			var semaphore = new Semaphore(initialCount: 0, maximumCount: 1, name: "WaitReceiveMessage");
			var connection = await StartConnectionAsync(server.CreateHandler(), "chat");
			var message = default(Message);

			// Act
			await connection.InvokeAsync("AddToRoom", "testCommandRoom");
			connection.On<Message>("receiveMessage", (chatMessage) =>
			{
				message = chatMessage;
				semaphore.Release();
			});
			await connection.InvokeAsync("SendMessageToRoom", new Message
			{
				Username = "robin",
				Text = "/stock=AAPL.US",
				Room = "testCommandRoom"
			});

			semaphore.WaitOne();

			//Assert
			message.Text.Should().Contain("APPL.US quote is");
			message.Username.Should().Be("robin");
		}
	}
}
