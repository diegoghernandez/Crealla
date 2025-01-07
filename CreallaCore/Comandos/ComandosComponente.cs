using System.CommandLine;
using System.Text.RegularExpressions;
using CreallaCore.Constantes;
using CreallaCore.Contratos;
using CreallaCore.Logica;

namespace CreallaCore.Comandos;

public class ComandosComponente
{
   public static Command Crear()
   {
      Argument<string> argumentoRuta = new(
         "ruta",
         "La ruta del componente ha ser creado"
      );

      Option<TiposDirectorioTest> opcionTest = new(
         name: "--test",
         description: "Crear un archivo test de diferentes tipos",
         getDefaultValue: () => TiposDirectorioTest.NINGUNO
      );
      opcionTest.AddAlias("-t");
      /* opcionTest.AddCompletions((ctx) =>
      {
         var tipos = new List<CompletionItem>();

         foreach (var tipo in Enum.GetNames(typeof(TiposDirectorioTest)))
         {
            tipos.Add(new CompletionItem(tipo));
         }

         return tipos;
      }); */

      Option<bool> opcionCSS = new(
         name: "--css",
         description: "Crea un archivo css"
      );

      var comandoComponente = new Command("componente", "Crea plantillas para un componente de React o Vue con o sin test y css"){
         argumentoRuta,
         opcionTest,
         opcionCSS
      };
      comandoComponente.AddAlias("c");

      comandoComponente.SetHandler(SeleccionarComponente, argumentoRuta, opcionTest, opcionCSS);

      return comandoComponente;
   }

   private static void SeleccionarComponente(
      string rutaComponente,
      TiposDirectorioTest tiposDirectorioTest,
      bool isCSS
   )
   {
      IComponentes componente;

      if (rutaComponente.Contains("vue")) componente = new ComponenteVue();
      else if (Regex.IsMatch(rutaComponente, "[j,t]sx")) componente = new ComponenteReact();
      else
      {
         Console.WriteLine("Componente no soportado");
         return;
      }

      componente.CrearComponente(rutaComponente);
      if (tiposDirectorioTest != TiposDirectorioTest.NINGUNO) componente.CrearComponenteTest(rutaComponente, tiposDirectorioTest);
      if (isCSS) componente.CrearComponente(rutaComponente);

   }
}
