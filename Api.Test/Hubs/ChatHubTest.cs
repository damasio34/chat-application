using ChatApplication.API;
using ChatApplication.Core.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.ComponentModel;
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
			// This token will exipire in 30 days, since 2022-06-26
			var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImJhdG1hbiIsIm5iZiI6MTY1NjI4ODQxOSwiZXhwIjoxOTE5MDg4NDE5LCJpYXQiOjE2NTYyODg0MTl9.J9YcRTD5r-iAk-v2XNQZsnRRENTBgG2xoO9fKXHI_MQ";
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
		[Category("Allow_registered_users_to_log_in_and_talk_with_other_users_in_a_chatroom.")]

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
			var message = default(ChatMessage);

			// Act
			await connection.InvokeAsync("AddToRoom", "testRoom");
			connection.On<ChatMessage>("receiveMessage", (chatMessage) =>
			{
				message = chatMessage;
				semaphore.Release();
			});
			await connection.InvokeAsync("SendMessageToRoom", new ChatMessage 
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

		[Fact(Skip = "Not finished, need open or mock bot")]
		[Category("Allow_users_to_post_messages_as_commands_into_the_chatroom_with_the_following_format_/stock_=_stock_code")]	
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
			var message = default(ChatMessage);

			// Act
			await connection.InvokeAsync("AddToRoom", "testCommandRoom");
			connection.On<ChatMessage>("receiveMessage", (chatMessage) =>
			{
				message = chatMessage;
				semaphore.Release();
			});
			await connection.InvokeAsync("SendMessageToRoom", new ChatMessage
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
