namespace CreallaTest.Fixturas;

public class DirectorioFixturas : IDisposable
{
   readonly static string _DIRECTORIO_RAIZ = $"{Directory.GetCurrentDirectory()}/src";

   public DirectorioFixturas()
   {
      var directorioComponentes = Directory.CreateDirectory(_DIRECTORIO_RAIZ + "/components");
      var directorioComponentesTests = Directory.CreateDirectory(_DIRECTORIO_RAIZ + "/components/__tests__");
      var directorioTest = Directory.CreateDirectory(_DIRECTORIO_RAIZ + "/test/components");

      if (directorioComponentes.Exists) Console.WriteLine("Directorio componentes creado correctamente");
      else Console.WriteLine("Directorio componentes no ha podido ser creado");

      if (directorioComponentesTests.Exists) Console.WriteLine("Directorio __tests__ en componentes creado correctamente");
      else Console.WriteLine("Directorio __tests__ en componentes no ha podido ser creado");

      if (directorioTest.Exists) Console.WriteLine("Directorio test creado correctamente");
      else Console.WriteLine("Directorio test no ha podido ser creado");

      Console.WriteLine("");
   }

   public void Dispose()
   {
      var directorio = new DirectoryInfo(_DIRECTORIO_RAIZ);
      directorio.Delete(true);

      if (!directorio.Exists) Console.WriteLine("Directorio src ha sido eliminado correctamente");
      else Console.WriteLine("Directorio src no ha podido ser eliminado");

      Console.WriteLine("");
   }
}
