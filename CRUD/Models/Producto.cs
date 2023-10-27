namespace CRUD.Models
{
    public class Producto
    {
        public int idProd { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public Categoria refCategoria { get; set; }
        public string fechaReg { get; set; }
    }
}
