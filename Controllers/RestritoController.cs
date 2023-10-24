using ControleDeContato.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContato.Controllers
{
    [PaginaParaUsuarioLogado]
    public class RestritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
