﻿namespace TaskManagement.Application.Interfaces.Services
{
    public interface IPasswordService
    {
        string GenerateSalt();

        string HashPassword(string password, string salt);

        bool VerifyPassword(string password, string salt, string hash);
    }
}