using System.CommandLine;
using CreallaCore.Comandos;

namespace CreallaCore;

class Program
{

   public static int Uno()
   {
      return 1;
   }

   static async Task<int> Main(string[] args)
   {
      RootCommand comandoPrincipal = new("Crealla una aplicación cli para crear plantillas");

      comandoPrincipal.AddCommand(ComandosComponente.Crear());

      return await comandoPrincipal.InvokeAsync(args);
   }
}