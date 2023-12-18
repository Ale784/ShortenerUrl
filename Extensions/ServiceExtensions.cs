using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShortenerUrl.Entities;

namespace ShortenerUrl.Extensions
{
    public static class ServiceExtension
    {

        public static void ExtensionCors(this IServiceCollection services)
        {
            services.AddCors(option => {
                option.AddPolicy("cors-policy", (opt) => {
                    opt.AllowAnyMethod();
                    opt.AllowAnyHeader();
                    opt.AllowAnyOrigin();
                });
            });
        }

        public static string GenerateRandomKey()
        {
            int SIZE = 6;
            char[] charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();


            byte[] data = new byte[SIZE * 6];

            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }

            StringBuilder str = new();

            for (int i = 0; i < SIZE; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 6);
                var idx = rnd % charset.Length;

                str.Append(charset[idx]);
            }


            return str.ToString();
        }
    }

}