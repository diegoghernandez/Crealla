using CreallaCore.Constantes;
using CreallaCore.Logica;
using CreallaTest.Fixturas;

namespace CreallaTest.Logica;

[Collection(nameof(SharedFixtureCollection))]
public class ComponenteVueTest : IClassFixture<DirectorioFixturas>
{
   readonly static string _DIRECTORIO_RAIZ = $"{Directory.GetCurrentDirectory()}/src/";
   readonly static string _DIRECTORIO_COMPONENTES = _DIRECTORIO_RAIZ + "components/";

   const string _COMPONENTE_VUE = """
   <script setup></script>

   <template>
      <div></div>
   </template>

   <style scope>
      div {}
   </style>

   """;

   const string TEST_VUE = """
   import { i18n } from '@/assets/locales/i18n'
   import PruebaTest from '@/components/common/PruebaTest.vue'
   import { LOCALES } from '@/constants/locales'
   import { t } from '@/utils/translation'
   import { cleanup, render, screen } from '@testing-library/vue'
   import { afterEach, beforeEach, describe, expect, it } from 'vitest'

   LOCALES.forEach((locale) => {
      describe(`${locale}: PruebaTest component tests`, () => {
         beforeEach(() => (i18n.global.locale.value = locale))
         afterEach(() => cleanup())

         it('Should render properly', () => {
            render(PruebaTest, {
               global: {
                  plugins: [i18n],
               },
            })
         })
      })
   })

   """;


   [Fact(DisplayName = "Debería crear un componente de Vue en el directorio componentes")]
   public void CrearComponente_Sin_Directorio()
   {
      const string rutaComponente = "Prueba.vue";
      new ComponenteVue().CrearComponente(rutaComponente);

      var contendidoComponente = File.ReadAllText(_DIRECTORIO_COMPONENTES + rutaComponente);

      Assert.Equal(_COMPONENTE_VUE, contendidoComponente);
   }

   [Fact(DisplayName = "Debería crear un componente de Vue en el directorio componentes y si no existe el directorio, crearlo")]
   public void CrearComponente_Y_Directorio()
   {
      const string rutaComponente = "common/Prueba.vue";

      Assert.False(Directory.Exists(_DIRECTORIO_COMPONENTES + "common"));
      new ComponenteVue().CrearComponente(rutaComponente);
      Assert.True(Directory.Exists(_DIRECTORIO_COMPONENTES + "common"));

      var contendidoComponente = File.ReadAllText(_DIRECTORIO_COMPONENTES + rutaComponente);

      Assert.Equal(_COMPONENTE_VUE, contendidoComponente);
   }

   [Fact(
      DisplayName = @"Debería crear un test de un componente de Vue en el mismo directorio del componente"
   )]
   public void CrearComponenteTest_Con_El_Tipo_MISMO()
   {
      var rutaTest = _DIRECTORIO_COMPONENTES + "common/" + "PruebaTest.spec.js";
      var archivoTest = new FileInfo(rutaTest);
      Assert.False(archivoTest.Exists);

      new ComponenteVue().CrearComponente("common/PruebaTest.vue");
      new ComponenteVue().CrearComponenteTest("common/PruebaTest.vue", TiposDirectorioTest.MISMO);

      var contendidoComponente = File.ReadAllText(rutaTest);
      Assert.Equal(TEST_VUE, contendidoComponente);
   }

   [Fact(
      DisplayName = @"Debería crear un test de un componente de Vue en el directorio test/components"
   )]
   public void CrearComponenteTest_Con_El_Tipo_DIRECTORIO()
   {
      var rutaTest = _DIRECTORIO_RAIZ + "test/components/" + "PruebaTest.spec.js";
      var archivoTest = new FileInfo(rutaTest);
      Assert.False(archivoTest.Exists);

      new ComponenteVue().CrearComponenteTest("common/PruebaTest.vue", TiposDirectorioTest.DIRECTORIO);

      var contendidoComponente = File.ReadAllText(rutaTest);
      Assert.Equal(TEST_VUE, contendidoComponente);
   }

   [Fact(
      DisplayName = @"Debería crear un test de un componente de Vue en el directorio __tests__ dentro del directorio componentes"
   )]
   public void CrearComponenteTest_Con_El_Tipo_TESTS()
   {
      var rutaTest = _DIRECTORIO_COMPONENTES + "__tests__/" + "PruebaTest.spec.js";
      var archivoTest = new FileInfo(rutaTest);
      Assert.False(archivoTest.Exists);

      new ComponenteVue().CrearComponenteTest("common/PruebaTest.vue", TiposDirectorioTest.TESTS);

      var contendidoComponente = File.ReadAllText(rutaTest);
      Assert.Equal(TEST_VUE, contendidoComponente);
   }
}