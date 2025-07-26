using CriFs.V2.Hook.Interfaces;

namespace CriExtensions;

internal static class ICriFsRedirectorApiExtensions
{
    public static void AddBind(
        this ICriFsRedirectorApi api,
        string file,
        string bindPath,
        string modId)
    {
        api.AddBindCallback(context =>
        {
            context.RelativePathToFileMap[$@"R2\{bindPath}"] = new()
            {
                new()
                {
                    FullPath = file,
                    LastWriteTime = DateTime.UtcNow,
                    ModId = modId,
                },
            };
        });
    }
}