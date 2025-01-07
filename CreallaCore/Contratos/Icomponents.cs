using CreallaCore.Constantes;

namespace CreallaCore.Contratos;

public interface IComponentes
{
   void CrearComponente(string rutaComponente);

   void CrearComponenteTest(string rutaComponente, TiposDirectorioTest directorioTest);
}
