﻿using System.Collections.Generic;

namespace DncIds4.IdentityServer.Config
{
    public class ApiResourceDefinition
    {
        public enum Apis
        {
            AccountApi,
            ResourceApi,
            Ocelot
        }

        public static Dictionary<Apis, string> ApiResources => new Dictionary<Apis, string>
        {
            { Apis.AccountApi, $"{nameof(Apis.AccountApi)}"},
            { Apis.ResourceApi, $"{nameof(Apis.ResourceApi)}" },
            { Apis.Ocelot, $"{nameof(Apis.Ocelot)}" }
        };
    }
}
