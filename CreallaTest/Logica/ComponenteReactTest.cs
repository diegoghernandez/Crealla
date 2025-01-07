using CreallaCore.Constantes;
using CreallaCore.Logica;
using CreallaTest.Fixturas;

namespace CreallaTest.Logica;

[Collection(nameof(SharedFixtureCollection))]
public class ComponenteReactTest : IClassFixture<DirectorioFixturas>
{
   readonly static string _DIRECTORIO_RAIZ = $"{Directory.GetCurrentDirectory()}/src/";
   readonly static string _DIRECTORIO_COMPONENTES = _DIRECTORIO_RAIZ + "components/";

   const string _COMPONENTE_REACT_JS = """
   /**
   * @typedef {Object} Props
   * @property {null} primero
   */

   /**
   * @param {Props} props
   * @returns
   */
   export function Prueba({}) {
      return (
         <div></div>
      )
   }

   """;

   const string TEST_REACT = """
   import { PruebaTest } from '@/components/PruebaTest'
   import { cleanup,render, screen } from '@testing-library/react'
   import {afterEach,describe, expect, it } from 'vitest'

   describe('PruebaTest component tests', () => {
      afterEach(() => cleanup())

      it('Should render properly', () => {
         render(<PruebaTest />)
      })
   })

   """;


   [Fact(DisplayName = "Debería crear un componente de React en el directorio componentes")]
   public void CrearComponente_Sin_Directorio()
   {
      const string rutaComponente = "Prueba.jsx";
      new ComponenteReact().CrearComponente(rutaComponente);

      var contendidoComponente = File.ReadAllText(_DIRECTORIO_COMPONENTES + rutaComponente);

      Assert.Equal(_COMPONENTE_REACT_JS, contendidoComponente);
   }

   [Fact(DisplayName = "Debería crear un componente de React en el directorio componentes y si no existe el directorio, crearlo")]
   public void CrearComponente_Y_Directorio()
   {
      const string rutaComponente = "common/Prueba.jsx";

      Assert.False(Directory.Exists(_DIRECTORIO_COMPONENTES + "common"));
      new ComponenteReact().CrearComponente(rutaComponente);
      Assert.True(Directory.Exists(_DIRECTORIO_COMPONENTES + "common"));

      var contendidoComponente = File.ReadAllText(_DIRECTORIO_COMPONENTES + rutaComponente);

      Assert.Equal(_COMPONENTE_REACT_JS, contendidoComponente);
   }

   [Fact(
      DisplayName = @"Debería crear un componente de React en el directorio componentes, pero, 
      al tener como nombre index, debería de usar el nombre del directorio como nombre del componente"
   )]
   public void CrearComponente_Usando_Index_Como_Nombre()
   {
      const string rutaComponente = "Prueba/index.jsx";

      Assert.False(Directory.Exists(_DIRECTORIO_COMPONENTES + "Prueba"));
      new ComponenteReact().CrearComponente(rutaComponente);
      Assert.True(Directory.Exists(_DIRECTORIO_COMPONENTES + "Prueba"));

      var contendidoComponente = File.ReadAllText(_DIRECTORIO_COMPONENTES + rutaComponente);

      Assert.Equal(_COMPONENTE_REACT_JS, contendidoComponente);
   }

   [Fact(
      DisplayName = @"Debería crear un componente de React en el directorio componentes, pero, 
      al tener como extension tsx, debería usar typescript en vez de jsdoc"
   )]
   public void CrearComponente_Usando_TSX()
   {
      const string rutaComponente = "Prueba.tsx";
      new ComponenteReact().CrearComponente(rutaComponente);

      const string componenteReactTS = """
      interface Props {}

      export function Prueba({}: Props) {
         return (
            <div></div>
         )
      }

      """;

      var contendidoComponente = File.ReadAllText(_DIRECTORIO_COMPONENTES + rutaComponente);

      Assert.NotEqual(_COMPONENTE_REACT_JS, contendidoComponente);
      Assert.Equal(componenteReactTS, contendidoComponente);
   }

   [Fact(
      DisplayName = @"Debería crear un test de un componente de React en el mismo directorio del componente"
   )]
   public void CrearComponenteTest_Con_El_Tipo_MISMO()
   {
      var rutaTest = _DIRECTORIO_COMPONENTES + "PruebaTest/" + "PruebaTest.test.jsx";
      var archivoTest = new FileInfo(rutaTest);
      Assert.False(archivoTest.Exists);

      new ComponenteReact().CrearComponente("PruebaTest/index.jsx");
      new ComponenteReact().CrearComponenteTest("PruebaTest/index.jsx", TiposDirectorioTest.MISMO);

      var contendidoComponente = File.ReadAllText(rutaTest);
      Assert.Equal(TEST_REACT, contendidoComponente);
   }

   [Fact(
      DisplayName = @"Debería crear un test de un componente de React en el directorio test/components"
   )]
   public void CrearComponenteTest_Con_El_Tipo_DIRECTORIO()
   {
      var rutaTest = _DIRECTORIO_RAIZ + "test/components/" + "PruebaTest.test.jsx";
      var archivoTest = new FileInfo(rutaTest);
      Assert.False(archivoTest.Exists);

      new ComponenteReact().CrearComponenteTest("PruebaTest.jsx", TiposDirectorioTest.DIRECTORIO);

      var contendidoComponente = File.ReadAllText(rutaTest);
      Assert.Equal(TEST_REACT, contendidoComponente);
   }

   [Fact(
      DisplayName = @"Debería crear un test de un componente de React en el directorio __tests__ dentro del directorio componentes"
   )]
   public void CrearComponenteTest_Con_El_Tipo_TESTS()
   {
      var rutaTest = _DIRECTORIO_COMPONENTES + "__tests__/" + "PruebaTest.test.jsx";
      var archivoTest = new FileInfo(rutaTest);
      Assert.False(archivoTest.Exists);

      new ComponenteReact().CrearComponenteTest("PruebaTest.jsx", TiposDirectorioTest.TESTS);

      var contendidoComponente = File.ReadAllText(rutaTest);
      Assert.Equal(TEST_REACT, contendidoComponente);
   }
}