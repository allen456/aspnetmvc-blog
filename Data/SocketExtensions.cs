namespace aspnetmvc_blog.Data
{
    public static class SocketExtensions
    {
        public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SocketMiddleware>();
        }

        public static IServiceCollection AddWebSocketServerConnectionManager(this IServiceCollection services)
        {
            services.AddSingleton<SocketManager>();
            return services;
        }
    }
}
  