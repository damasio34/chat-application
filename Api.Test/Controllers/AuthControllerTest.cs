using ChatApplication.API;
using ChatApplication.Core.Domain.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChatApplication.Api.Test
{
    public class AuthControllerTest
    {
        [Fact]
        [Category("Allow_registered_users_to_log_in_and_talk_with_other_users_in_a_chatroom.")]        
        public async Task POST_Authenticate_Success()
        {
            // Arrange
            using var application = new WebApplicationFactory<Startup>();
            using var client = application.CreateClient();
            var payload = new
            {
                Username = "batman",
                Password = "batman"
            };
            var stringPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            // Act            
            var response = await client.PostAsync("/api/auth/login", httpContent);
            var loginResult = JsonConvert.DeserializeObject<LoginResultDTO>(
              await response.Content.ReadAsStringAsync()
            );

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            loginResult.Should().BeOfType<LoginResultDTO>();
            loginResult.IsAuthenticated.Should().Be(true);
            loginResult.Token.Should().NotBeNullOrEmpty();
            loginResult.Username.Should().Be("batman");
        }

        [Fact]
        [Category("Allow_registered_users_to_log_in_and_talk_with_other_users_in_a_chatroom.")]        
        public async Task POST_Authenticate_Not_Found()
        {
            // Arrange
            using var application = new WebApplicationFactory<Startup>();
            using var client = application.CreateClient();            
            var payload = new
            {
                Username = "not-batman",
                Password = "not-batman"
            };
            var stringPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            // Act            
            var response = await client.PostAsync("/api/auth/login", httpContent);
            var loginResult = JsonConvert.DeserializeObject<LoginResultDTO>(
              await response.Content.ReadAsStringAsync()
            );

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            loginResult.Should().BeOfType<LoginResultDTO>();
            loginResult.IsAuthenticated.Should().Be(false);
            loginResult.Token.Should().BeNullOrEmpty();
            loginResult.Username.Should().BeNullOrEmpty();
        }
    }
}
