using ControleDeContato.Filters;
using ControleDeContato.Helper;
using ControleDeContato.Models;
using ControleDeContato.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContato.Controllers
{
    [PaginaParaUsuarioLogado]
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly ISessao _sessao;
        public ContatoController(IContatoRepositorio contatoRepositorio, ISessao sessao) { 
            _contatoRepositorio = contatoRepositorio;
            _sessao = sessao;
        }
        public IActionResult Index()
        {
            UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            var contatos = _contatoRepositorio.BuscarTodos(usuarioLogado.Id);
            return View(contatos);
        } 
        public IActionResult Criar()
        { 
            return View();
        }
        public IActionResult Editar(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }
        public IActionResult ApagarConfirmacao(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }
        public IActionResult Apagar(int id)
        {
            try
            {
                _contatoRepositorio.Apagar(id);
                TempData["MensagemSucesso"] = "Contato apagado com sucesso";
                return RedirectToAction("Index");
            } catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos apagar seu contato, Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
           try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                    contato.UsuarioID = usuarioLogado.Id;
                    _contatoRepositorio.Adicionar(contato);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso";
                    return RedirectToAction("Index");
                }
                return View();
            } catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu contato, Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
            
        }
        [HttpPost]
        public IActionResult Alterar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                    contato.UsuarioID = usuarioLogado.Id;
                    _contatoRepositorio.Atualizar(contato);
                    TempData["MensagemSucesso"] = "Contato editado com sucesso";
                    return RedirectToAction("Index");
                }
                return View("Editar", contato);
            } catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos editar seu contato, Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}

