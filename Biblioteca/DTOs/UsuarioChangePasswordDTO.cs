﻿namespace Biblioteca.DTOs
{
    public class UsuarioChangePasswordDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}