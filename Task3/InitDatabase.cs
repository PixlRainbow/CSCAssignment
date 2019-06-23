using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CSCAssignment.Data;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CSCAssignment
{
    public static class InitDatabase{
        public static void Init(IApplicationBuilder app){
            using (var serviceScope = app.ApplicationServices
                    .GetService<IServiceScopeFactory>()
                    .CreateScope()){
            Migrate(serviceScope);
            SeedData(serviceScope);
        }
        }
        private static void SeedData(IServiceScope serviceScope){
            var context = serviceScope
                .ServiceProvider
                .GetRequiredService<ConfigurationDbContext>();
            
            if(!context.Clients.Any()){
                /*context.Clients.Add(
                    // OpenID Connect implicit flow client (MVC)
                    new Client {
                        ClientId = "mvc",
                        ClientName = "MVC Client",
                        AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                        RequireConsent = false,

                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },

                        RedirectUris = { "http://localhost:5000/signin-oidc" },
                        PostLogoutRedirectUris = { "http://localhost:5000/signout-callback-oidc" },

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "CSCAssignment"
                        },
                        AllowOfflineAccess = true
                    }.ToEntity()
                );*/
                context.Clients.Add(
                    // JavaScript AJAX/Knockout client
                    new Client {
                        ClientId = "knockout",
                        ClientName = "Knockout.js client",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        //AccessTokenType = AccessTokenType.Reference,

                        RequireConsent = false,

                        /*ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },*/

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            "CSCAssignment"
                        },
                        AllowOfflineAccess = true
                    }.ToEntity()
                );
                context.SaveChanges();
            }
            if(!context.IdentityResources.Any()){
                context.IdentityResources.Add(new IdentityResources.OpenId().ToEntity());
                context.IdentityResources.Add(new IdentityResources.Profile().ToEntity());
                context.IdentityResources.Add(new IdentityResources.Email().ToEntity());
                context.SaveChanges();
            }
            if(!context.ApiResources.Any()){
                context.ApiResources.Add(new ApiResource("CSCAssignment"){
                    //ApiSecrets = {new Secret("secret".Sha256())}
                }.ToEntity());
                context.SaveChanges();
            }
        }
        private static void Migrate(IServiceScope serviceScope){
            serviceScope.ServiceProvider
                .GetRequiredService<ConfigurationDbContext>()
                .Database
                .Migrate();
            serviceScope.ServiceProvider
                .GetRequiredService<PersistedGrantDbContext>()
                .Database
                .Migrate();
            serviceScope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>()
                .Database
                .Migrate();
        }
    }
}