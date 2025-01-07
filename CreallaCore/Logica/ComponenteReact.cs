using CreallaCore.Constantes;
using CreallaCore.Contratos;

namespace CreallaCore.Logica;


public class ComponenteReact : IComponentes
{
   readonly string _DIRECTORIO_RAIZ = $"{Directory.GetCurrentDirectory()}/src";

   public void CrearComponente(string rutaComponente)
   {
      if (!Directory.Exists($"{_DIRECTORIO_RAIZ}/components"))
      {
         Console.WriteLine("El directorio de componentes no existe");
         return;
      }

      var nombreComponente = ObtenerNombreComponent(rutaComponente);

      var componenteReactJS = $$"""
      /**
      * @typedef {Object} Props
      * @property {null} primero
      */

      /**
      * @param {Props} props
      * @returns
      */
      export function {{nombreComponente}}({}) {
         return (
            <div></div>
         )
      }

      """;

      var componenteReactTS = $$"""
      interface Props {}

      export function {{nombreComponente}}({}: Props) {
         return (
            <div></div>
         )
      }

      """;

      try
      {
         var componenteReact = rutaComponente.Contains("jsx") ? componenteReactJS : componenteReactTS;
         var path = $"{_DIRECTORIO_RAIZ}/components/{rutaComponente}";
         Directory.CreateDirectory(path[..path.IndexOf(path.Split("/").Last())]);
         File.WriteAllText(path, componenteReact);

         Console.WriteLine($"Componente creado correctamente en: {_DIRECTORIO_RAIZ}/{rutaComponente}");
      }
      catch (Exception e)
      {
         Console.WriteLine(e.Message);
      }
   }

   public void CrearComponenteTest(string rutaComponente, TiposDirectorioTest directorioTest)
   {
      var nombreComponente = ObtenerNombreComponent(rutaComponente);

      var archivoTest = rutaComponente.Contains("jsx") ? nombreComponente + ".test.jsx" : nombreComponente + ".test.tsx";
      var rutaComponenteSinExtras = rutaComponente.Contains("index") ? rutaComponente[..^"/index.jsx".Length] : rutaComponente[..^".jsx".Length];
      var ruta = directorioTest switch
      {
         TiposDirectorioTest.MISMO => $"{_DIRECTORIO_RAIZ}/components/{rutaComponenteSinExtras}/",
         TiposDirectorioTest.DIRECTORIO => $"{_DIRECTORIO_RAIZ}/test/components/",
         TiposDirectorioTest.TESTS => $"{_DIRECTORIO_RAIZ}/components/__tests__/",
         _ => throw new NotImplementedException(),
      };

      if (!Directory.Exists(ruta))
      {
         Console.WriteLine("El directorio no existe");
         return;
      }

      var componenteReact = $$"""
      import { {{nombreComponente}} } from '@/components/{{rutaComponenteSinExtras}}'
      import { cleanup,render, screen } from '@testing-library/react'
      import {afterEach,describe, expect, it } from 'vitest'

      describe('{{nombreComponente}} component tests', () => {
         afterEach(() => cleanup())

         it('Should render properly', () => {
            render(<{{nombreComponente}} />)
         })
      })

      """;

      try
      {
         File.WriteAllText(ruta + archivoTest, componenteReact);
         Console.WriteLine("Test creado correctamente en: " + ruta + archivoTest);
      }
      catch (Exception e)
      {
         Console.WriteLine(e.Message);
      }
   }

   private static string ObtenerNombreComponent(string rutaComponente)
   {
      var rutaElementos = rutaComponente.Split("/");

      if (rutaComponente.Contains("index"))
      {
         return rutaElementos.ElementAt(rutaElementos.Length - 2);
      }

      return rutaElementos.Last().Split(".").First();
   }
}