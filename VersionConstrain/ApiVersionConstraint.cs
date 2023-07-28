namespace AgroExpressAPI.VersionConstrain
{
    public class ApiVersionConstraint : IRouteConstraint
    {
        private readonly int _supportedVersion;

        public ApiVersionConstraint(int supportedVersion)
        {
            _supportedVersion = supportedVersion;
        }
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue("apiVersion", out var value))
                return false;

            if (int.TryParse(value.ToString(), out var version))
            {
                return version == _supportedVersion;
            }

            return false;
        }
    }
}
