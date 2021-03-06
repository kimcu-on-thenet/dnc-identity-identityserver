﻿using DncIds4.IdentityServer.Config;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;

namespace DncIds4.IdentityServer.Data
{
    public static class Database
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                //List of associated user claim types that should be included in the identity token.
                new IdentityResource(name:Constants.IdentityResource.UserRoles, new []{ Constants.IdentityResource.UserRoles }),
                new IdentityResource(name:Constants.IdentityResource.UserScopes, new []{ Constants.IdentityResource.UserScopes })
            };

        public static IEnumerable<ApiResource> ApiResources => 
            new ApiResource[]
            {
                new ApiResource(ApiResourceDefinition.ApiResources[ApiResourceDefinition.Apis.ResourceApi], "The resource Api is protected by IdentityServer4")
                {
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //List of associated user claim types that should be included in the access token. The claims specified here will be added to the list of claims specified for the API.
                    UserClaims =
                    {
                        Constants.IdentityResource.UserRoles,
                        Constants.IdentityResource.UserScopes
                    }
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName, "Identity Server Api"){
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    UserClaims =
                    {
                        Constants.IdentityResource.UserRoles,
                        Constants.IdentityResource.UserScopes
                    }
                },
            };

        public static IEnumerable<Client> Clients => 
            new Client[]
            {
                new Client
                {
                    ClientId = ApiResourceDefinition.ApiResources[ApiResourceDefinition.Apis.ResourceApi],
                    AllowAccessTokensViaBrowser = true,
                    AlwaysSendClientClaims = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //By default a client has no access to any resources - specify the allowed resources by adding the corresponding scopes names
                    AllowedScopes = {
                        ApiResourceDefinition.ApiResources[ApiResourceDefinition.Apis.ResourceApi]
                    },
                    Claims =
                    {
                        new Claim(Constants.IdentityResource.UserRoles, ApiRoleDefinition.ApiRoles[ApiRoleDefinition.Roles.Admin])
                    }
                },
                new Client
                {
                    ClientId = ApiResourceDefinition.ApiResources[ApiResourceDefinition.Apis.IdentityServerApi],
                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        ApiResourceDefinition.ApiResources[ApiResourceDefinition.Apis.IdentityServerApi]
                    },
                    Claims =
                    {
                        new Claim(Constants.IdentityResource.UserRoles, ApiRoleDefinition.ApiRoles[ApiRoleDefinition.Roles.Admin])
                    }
                }
            };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "sub1",
                    Username = "admin",
                    Password = "admin",
                    Claims = new List<Claim>
                    {
                        new Claim("role", "api::admin")
                    }
                },
                new TestUser
                {
                    SubjectId = "sub2",
                    Username = "user",
                    Password = "user123",
                    Claims = new List<Claim>
                    {
                        new Claim("role", "api::user")
                    }
                },
            };
    }
}
