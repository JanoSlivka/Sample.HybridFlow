using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
                new ApiScope("projects", "Projekty"),
                new ApiScope("store", "Sklad"),
            };

        public static IEnumerable<Client> Clients
        {
            get
            {
                var secret = new Secret { Value = "mysecret".Sha512() };
                return new Client[]
                    {
                    // m2m client credentials flow client
                    new Client
                    {
                        ClientId = "m2m.client",
                        ClientName = "Client Credentials Client",

                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                        AllowedScopes = { "scope1" }
                    },

                    // interactive client using code flow + pkce
                    new Client
                    {
                        ClientId = "interactive",
                        ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                        AllowedGrantTypes = GrantTypes.Code,

                        RedirectUris = { "https://localhost:44300/signin-oidc" },
                        FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                        PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                        AllowOfflineAccess = true,
                        AllowedScopes = { "openid", "profile", "scope2" }
                    },

                    // Postmant
                    new Client
                    {
                        ClientId = "postman",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
                        AccessTokenLifetime = 10000,
                        AllowedScopes = { "projects" }
                    },
                    new Client{
                        ClientId = "StavebnyDennik",
                        ClientName = "Stavebný denník",
                        AllowedGrantTypes = GrantTypes.Hybrid,
                        RequireConsent = true,
                        Enabled = true,
                        ClientSecrets = new List<Secret> { secret },
                        AllowedScopes = { "projects", "profile", "openid" },
                        RedirectUris = { "https://localhost:5001/projects2", "https://localhost:5001/test" },
                        AccessTokenType = AccessTokenType.Jwt,
                        AllowOfflineAccess = true,
                        RequirePkce = false,
                        AbsoluteRefreshTokenLifetime =  int.MaxValue,
                        RefreshTokenUsage = TokenUsage.ReUse
                    },
                    new Client{
                        ClientId = "StavebnyDennikZaloha",
                        AllowedGrantTypes  = GrantTypes.Implicit,
                        RequireConsent = true,
                        Enabled = true,
                        RequirePkce = true,
                        AllowedScopes = { "projects", "email", "openid" },
                        RedirectUris = { "https://localhost:5001/projects" },
                    }};
            }
        }
    }
}