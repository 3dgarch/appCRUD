using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using CRUD.Repositorios.Contrato;

namespace CRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // variables
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IGenericRepository<Categoria> _categoriaRepository;

        public HomeController(ILogger<HomeController> logger,
            IGenericRepository<Producto> productoRepository,
            IGenericRepository<Categoria> categoriaRepository)
        {
            _logger = logger;
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // lista Categorias
        [HttpGet]
        public async Task<IActionResult> ListaCategorias()
        {
            List<Categoria> _lista = await _categoriaRepository.Lista();

            return StatusCode(StatusCodes.Status200OK, _lista);
        }

        // lista productos
        [HttpGet]
        public async Task<IActionResult> ListaProductos()
        {
            List<Producto> _lista = await _productoRepository.Lista();

            return StatusCode(StatusCodes.Status200OK, _lista);
        }


        // guardar producto
        [HttpPost]
        public async Task<IActionResult> GuardarProducto([FromBody] Producto modelo)
        {
            bool _resultado = await _productoRepository.Guardar(modelo);

            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, smg = "OK" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, smg = "Error" });

        }

        // editar producto
        [HttpPut]
        public async Task<IActionResult> EditarProducto([FromBody] Producto modelo)
        {
            bool _resultado = await _productoRepository.Editar(modelo);

            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, smg = "OK" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, smg = "Error" });

        }

        // eliminar producto
        [HttpDelete]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            bool _resultado = await _productoRepository.Eliminar(id);

            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, smg = "OK" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, smg = "Error" });

        }


























        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}