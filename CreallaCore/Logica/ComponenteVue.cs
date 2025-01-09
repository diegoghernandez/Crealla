using CreallaCore.Constantes;
using CreallaCore.Contratos;

namespace CreallaCore.Logica;


public class ComponenteVue : IComponentes
{
   readonly string _DIRECTORIO_RAIZ = $"{Directory.GetCurrentDirectory()}/src";

   public void CrearComponente(string rutaComponente)
   {
      if (!Directory.Exists($"{_DIRECTORIO_RAIZ}/components"))
      {
         Console.WriteLine("El directorio de componentes no existe");
         return;
      }

      var ruta = $"{_DIRECTORIO_RAIZ}/components/{rutaComponente}";
      if (File.Exists(ruta))
      {
         Console.WriteLine("El componente ya existe");
         return;
      }

      var componenteVue = """
      <script setup></script>

      <template>
         <div></div>
      </template>

      <style scope>
         div {}
      </style>

      """;

      try
      {
         Directory.CreateDirectory(ruta[..ruta.IndexOf(ruta.Split("/").Last())]);
         File.WriteAllText(ruta, componenteVue);

         Console.WriteLine($"Componente creado correctamente en: {ruta}");
      }
      catch (Exception e)
      {
         Console.WriteLine(e.Message);
      }
   }

   public void CrearComponenteTest(string rutaComponente, TiposDirectorioTest directorioTest)
   {
      var nombreComponente = rutaComponente.Split("/").Last().Split(".").First();

      var archivoTest = nombreComponente + ".spec.js";
      var ruta = directorioTest switch
      {
         TiposDirectorioTest.MISMO => $"{_DIRECTORIO_RAIZ}/components/{rutaComponente[..^$"{nombreComponente}.vue".Length]}",
         TiposDirectorioTest.DIRECTORIO => $"{_DIRECTORIO_RAIZ}/test/components/",
         TiposDirectorioTest.TESTS => $"{_DIRECTORIO_RAIZ}/components/__tests__/",
         _ => throw new NotImplementedException(),
      };

      if (!Directory.Exists(ruta))
      {
         Console.WriteLine($"El directorio: {ruta} no existe");
         return;
      }

      var componenteVue = $$"""
      import {{nombreComponente}} from '@/components/{{rutaComponente}}'
      import { cleanup, render, screen } from '@testing-library/vue'
      import { afterEach, describe, expect, it } from 'vitest'

      describe('{{nombreComponente}} component tests', () => {
         afterEach(() => cleanup())

         it('Should render properly', () => {
            render({{nombreComponente}})
         })
      })

      """;

      try
      {
         File.WriteAllText(ruta + archivoTest, componenteVue);
         Console.WriteLine("Test creado correctamente en: " + ruta + archivoTest);
      }
      catch (Exception e)
      {
         Console.WriteLine(e.Message);
      }
   }
}