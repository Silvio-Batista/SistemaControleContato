using ControleDeContato.Data;
using ControleDeContato.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContato.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BancoContext _bancoContext;
        public UsuarioRepositorio(BancoContext bancoContext) {
            this._bancoContext = bancoContext;
        }
        public UsuarioModel BuscarPorEmailELogin(string email, string login)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper() && x.Login.ToUpper() == login.ToUpper());
        }
        public UsuarioModel BuscarPorLogin(string login)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Login.ToUpper() == login.ToUpper());
        }
        public UsuarioModel ListarPorId(int id) 
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Id == id);
        }
        public List<UsuarioModel> BuscarTodos()
        {
            return _bancoContext.Usuarios
                .Include(x => x.Contatos)
                .ToList();
        }
        public UsuarioModel Adicionar(UsuarioModel usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            usuario.SetSenhaHash();
            _bancoContext.Usuarios.Add(usuario);
            _bancoContext.SaveChanges();
            return usuario;
        }

        public UsuarioModel Atualizar(UsuarioModel usuario)
        {
            UsuarioModel usuarioDB = ListarPorId(usuario.Id) ?? throw new SystemException("Houve um erro na atualização do Usuário");
            usuarioDB.Nome = usuario.Nome;
            usuarioDB.Email = usuario.Email;
            usuario.Login = usuario.Login;
            usuarioDB.DataAtualizacao = DateTime.UtcNow;
            usuarioDB.Perfil = usuario.Perfil;

            _bancoContext.Usuarios.Update(usuarioDB);
            _bancoContext.SaveChanges();

            return usuarioDB;   
        }
        public UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenha) 
        {
            UsuarioModel usuarioDB = ListarPorId(alterarSenha.Id);
            if (usuarioDB == null) throw new Exception("Houve um erro na atualização da senha, usuário não encontrado");
            if (!usuarioDB.SenhaValida(alterarSenha.SenhaAtual)) throw new Exception("Senha atual não confere");
            if (usuarioDB.SenhaValida(alterarSenha.NovaSenha)) throw new Exception("Você já está utilizando essa senha");
            usuarioDB.SetNovaSenha(alterarSenha.NovaSenha);
            usuarioDB.DataAtualizacao = DateTime.Now;

            _bancoContext.Usuarios.Update(usuarioDB);
            _bancoContext.SaveChanges();
            return usuarioDB;
        }
        public bool Apagar(int id)
        {
            UsuarioModel usuarioDB = ListarPorId(id) ?? throw new SystemException("Houve um erro na deleção do Usuário");
            _bancoContext.Usuarios.Remove(usuarioDB);
            _bancoContext.SaveChanges();

            return true;


        }
       
    }
}
