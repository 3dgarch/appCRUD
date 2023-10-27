using CRUD.Models;
using CRUD.Repositorios.Contrato;

using System.Data;
using System.Data.SqlClient;

namespace CRUD.Repositorios.Implementacion
{
    public class CategoriaRepository:IGenericRepository<Categoria>

    {
        private readonly string _cadenaSQL = "";

        public CategoriaRepository(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSql");
        }


        public async Task<List<Categoria>> Lista()
        {
            List<Categoria> _lista = new List<Categoria>();

            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("sp_listarCategorias", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while(await dr.ReadAsync())
                    {
                        _lista.Add(new Categoria
                        {
                            idCat = Convert.ToInt32(dr["idCat"]),
                            nombre = dr["nombre"].ToString()

                        });
                    }
                    
                }
            }

            return _lista;
        }

        public Task<bool> Guardar(Categoria modelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Editar(Categoria moddelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        

        
    }
}
