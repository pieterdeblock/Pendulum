using Microsoft.Extensions.DependencyInjection;

namespace Wpf3DUtils
{
    public static class ServiceExtensions
    {
        public static void AddWpf3DUtils(this ServiceCollection services)
        {
            services.AddTransient<ICameraController, CameraController>();
        }

    }
}
