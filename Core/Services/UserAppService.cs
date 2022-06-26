using ChatApplication.Core.Domain.DTOs;
using ChatApplication.Core.Domain.Repositories;
using ChatApplication.Core.Domain.Services;
using System;

namespace ChatApplication.Core.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserAppService(IUserRepository userRepository, ITokenService tokenService)
        {
            this._userRepository = userRepository;
            this._tokenService = tokenService;
        }

        public LoginResultDTO Authenticate(LoginDTO login)
        {
            // Recupera o usuário
            var userIsValid = this._userRepository.IsValid(login.Username, login.Password);
            if (!userIsValid)
                return new LoginResultDTO();

            var token = this._tokenService.GenerateToken(login.Username);

            return new LoginResultDTO
            {
                IsAuthenticated = true,
                Username = login.Username,
                Token = token
            };
        }
    }
}
