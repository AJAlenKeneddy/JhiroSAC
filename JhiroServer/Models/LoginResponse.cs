﻿namespace JhiroServer.Models
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; } 
    }
}
