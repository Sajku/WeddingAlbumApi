﻿using System;
using System.Security.Cryptography;
using WeddingAlbum.Common.Auth;

namespace WeddingAlbum.Api.Infrastructure
{
    public class Encrypter : IEncrypter
    {
        private static readonly int DeriveBytesIterationsCount = 10000;
        private static readonly int SaltSize = 40;

        public string GetSalt(string value)
        {
            if (value == "")
            {
                throw new ArgumentException("Can not use an empty salt from hashing value.", nameof(value));
            }

            var random = new Random();
            var saltBytes = new byte[SaltSize];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        public string GetHash(string value, string salt)
        {
            if (value == "")
            {
                throw new ArgumentException("Can not use an empty salt from hashing value.", nameof(value));
            }
            if (salt == "")
            {
                throw new ArgumentException("Can not use an empty salt from hashing value.", nameof(salt));
            }

            var pbkdf2 = new Rfc2898DeriveBytes(value, GetBytes(salt), DeriveBytesIterationsCount);

            return Convert.ToBase64String(pbkdf2.GetBytes(SaltSize));
        }

        private static byte[] GetBytes(string value)
        {
            var bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
    }
}
