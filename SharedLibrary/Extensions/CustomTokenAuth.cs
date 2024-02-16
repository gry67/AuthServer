using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth
    {
        public static void AddCustomTokenAuth(this IServiceCollection services,CustomTokenOption tokenOptions)
        {
            services.AddAuthentication(options =>
            {
                //jwt kimlik doğrulama şemasını ayarla
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                

                // Token doğrulama parametrelerini ayarla
                opts.TokenValidationParameters = new TokenValidationParameters()
                {
                    // Token'in geçerli olduğu yayıncı (issuer)
                    ValidIssuer = tokenOptions.Issuer,
                    // Token'in geçerli olduğu hedef domainler (audience)
                    ValidAudience = tokenOptions.Audience[0],
                    // Token'in imzalanmış olması için kullanılan gizli anahtarı
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                    // İmzalama anahtarını doğrula
                    ValidateIssuerSigningKey = true,
                    // Token'in hedef domain'i doğrula
                    ValidateAudience = true,
                    // Token'in yayıncıyı doğrula
                    ValidateIssuer = true,
                    // Token'in ömrünü doğrula
                    ValidateLifetime = true,
                    // tokenin ömrünü fazladan uzatmasını engelledik.
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
