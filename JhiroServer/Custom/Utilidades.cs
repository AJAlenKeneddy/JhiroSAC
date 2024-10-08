﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JhiroServer.Models;


namespace JhiroServer.Custom
{
    public class Utilidades
    {
        private readonly IConfiguration _configuration;
        public Utilidades(IConfiguration configuration) { 
            _configuration = configuration;
        }
        public string encriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder= new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                { 
                    builder.Append(bytes[i].ToString("X2"));

                }
                return builder.ToString();


            }
        }
    }
}
