namespace W.O.API.Contracts.V1
{
    public static class ApiRoutes
    {
        private const string Root = "api";
        private const string Version = "v1";
        private const string Base = Root + "/" + Version;

        public static class WorkOrders
        {
            public const string GetAll = Base + "/workorders";
            public const string Get = Base + "/workorders/{id:Guid}";
            public const string Add = Base + "/workorders";
            public const string Update = Base + "/workorders/{id:Guid}";
            public const string Delete = Base + "/workorders/{id:Guid}";
        }

        public static class Visits
        {
            public const string GetAll = Base + "/visits";
            public const string Get = Base + "/visits/{id:Guid}";
            public const string Add = Base + "/visits";
            public const string Update = Base + "/visits/{id:Guid}";
            public const string Delete = Base + "/visits/{id:Guid}";
        }

        public static class Parts
        {
            public const string GetAll = Base + "/parts";
            public const string Get = Base + "/parts/{id:Guid}";
            public const string Add = Base + "/parts";
            public const string Update = Base + "/parts/{id:Guid}";
            public const string Delete = Base + "/parts/{id:Guid}";
        }

        public static class Currencies
        {
            public const string GetAll = Base + "/currencies";
        }
    }
}
