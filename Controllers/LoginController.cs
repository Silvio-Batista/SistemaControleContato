using ControleDeContato.Helper;
using ControleDeContato.Models;
using ControleDeContato.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContato.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        private readonly IEmail _email;

        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao, IEmail email)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;
        }
        public IActionResult RedefinirSenha() {
            return View();
        }
        public IActionResult Index()
        {
            //se o usuario tiver logado, direcionar p home
            if(_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");
            return View();
        }
        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(login: loginModel.Login);
                    if(usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoUsuario(usuario);
                            return RedirectToAction("Index", "Home");
                        }
                        TempData["MensagemErro"] = $"Não foi possivel entrar, senha incorreta. Tente novamente";
                    }
                    TempData["MensagemErro"] = $"Não foi possivel entrar, login ou senha incorreto. Tente novamente";
                }
                return View("index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não foi possivel fazer seu login, mais informações: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost] 
        public IActionResult EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmailELogin(redefinirSenha.Email, redefinirSenha.Login);
                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        string mensagem = $"Sua nova senha é: {novaSenha}";
                        bool emailEnviado  = _email.Enviar(usuario.Email, "Sistema de Contato - Nova senha", mensagem);
                        if (emailEnviado)
                        {
                            _usuarioRepositorio.Atualizar(usuario);
                            TempData["MensagemSucesso"] = $"Enviamos uma nova senha para o seu Email";
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Não conseguimos enviar o email, por favor tente novamente";
                        }
                        
                        return RedirectToAction("Index", "Login");
                    }
                    TempData["MensagemErro"] = $"Não foi possivel redifinir sua senha, por favor verifique os dados informados";
                }
               return View("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não foi possivel redefinir sua senha, mais informações: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
