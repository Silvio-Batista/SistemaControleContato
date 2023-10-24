using ControleDeContato.Helper;
using ControleDeContato.Models;
using ControleDeContato.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContato.Controllers
{
    public class AlterarSenhaController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        public AlterarSenhaController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Alterar(AlterarSenhaModel alterarSenha)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                    UsuarioModel usuario = _sessao.BuscarSessaoDoUsuario();
                    alterarSenha.Id = usuario.Id;   
                    _usuarioRepositorio.AlterarSenha(alterarSenha);
                    TempData["MensagemSucesso"] = "Senha alterada com sucesso";
                    return View("Index", alterarSenha);

                }
                return View("Index", alterarSenha);
            } catch (Exception ex) 
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar sua senha, Detalhes do erro: {ex.Message}";
                return View("Index", alterarSenha);

            }
        }
    }
}
