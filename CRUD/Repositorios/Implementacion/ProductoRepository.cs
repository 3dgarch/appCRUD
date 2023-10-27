using CRUD.Models;
using CRUD.Repositorios.Contrato;
using System.Data;
using System.Data.SqlClient;


namespace CRUD.Repositorios.Implementacion
{
    public class ProductoRepository : IGenericRepository<Producto>
    {
        private readonly string _cadenaSQL = "";
        public ProductoRepository(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSql");
        }
        public async Task<List<Producto>> Lista()
        {
            List<Producto> _lista = new List<Producto>();

            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("sp_listarProductos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        _lista.Add(new Producto
                        {
                            idProd = Convert.ToInt32(dr["idProd"]),
                            nombre = dr["nombre"].ToString(),
                            descripcion = dr["descripcion"].ToString(),
                            precio = Convert.ToDecimal(dr["precio"]),
                            stock = Convert.ToInt32(dr["cantStock"]),
                            refCategoria = new Categoria()
                            {
                                idCat = Convert.ToInt32(dr["idCat"]),
                                nombre = dr["nombre"].ToString()
                            },

                            //fechaReg = dr["fechaRegistro"].ToString(),

                        });
                    }

                }
            }

            return _lista;
        }
        public async Task<bool> Guardar(Producto modelo)
        {
           
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("sp_registrarProducto", conexion);
                    cmd.Parameters.AddWithValue("nombre", modelo.nombre);
                    cmd.Parameters.AddWithValue("descripcion", modelo.descripcion);
                    cmd.Parameters.AddWithValue("precio", modelo.precio);
                    cmd.Parameters.AddWithValue("cantStock", modelo.stock);
                    cmd.Parameters.AddWithValue("idCat", modelo.refCategoria.idCat);
                    cmd.Parameters.AddWithValue("fechaRegistro", modelo.fechaReg);

                    cmd.CommandType = CommandType.StoredProcedure;

                    int filas_afectadas = await cmd.ExecuteNonQueryAsync();

                    if (filas_afectadas > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            
        }
        public async Task<bool> Editar(Producto modelo)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", conexion);
                    cmd.Parameters.AddWithValue("idProd", modelo.idProd);
                    cmd.Parameters.AddWithValue("nombre", modelo.nombre);
                    cmd.Parameters.AddWithValue("descripcion", modelo.descripcion);
                    cmd.Parameters.AddWithValue("precio", modelo.precio);
                    cmd.Parameters.AddWithValue("cantStock", modelo.stock);
                    cmd.Parameters.AddWithValue("idCat", modelo.refCategoria.idCat);
                    cmd.Parameters.AddWithValue("fechaRegistro", modelo.fechaReg);

                    cmd.CommandType = CommandType.StoredProcedure;

                    int filas_afectadas = await cmd.ExecuteNonQueryAsync();

                    if (filas_afectadas > 0)
                        return true;
                    else
                        return false;

                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
           
        }

        public async Task<bool> Eliminar(int id)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_eliminarProducto", conexion);
                cmd.Parameters.AddWithValue("idProd", id);


                cmd.CommandType = CommandType.StoredProcedure;

                int filas_afectadas = await cmd.ExecuteNonQueryAsync();

                if (filas_afectadas > 0)
                    return true;
                else
                    return false;

            }
        }

       

        
    }
}
