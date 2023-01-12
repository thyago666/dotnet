using ControleDeContatos.Data;
using ControleDeContatos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContatos.Repositorio
{
	public class UsuarioRepositorio : IUsuarioRepositorio
	{

		private readonly BancoContext _bancoContext;
		public UsuarioRepositorio(BancoContext bancoContext)
		{
			_bancoContext = bancoContext;
		}

		public List<UsuarioModel> BuscarTodos()
		{
			return _bancoContext.Usuarios.ToList();
		}
		public UsuarioModel Adicionar(UsuarioModel usuario)
		{
			usuario.DataCadastro = DateTime.Now;
			_bancoContext.Usuarios.Add(usuario);
			_bancoContext.SaveChanges();
			return usuario;
		}

		public UsuarioModel ListarPorId(int id)
		{
			return _bancoContext.Usuarios.FirstOrDefault(x => x.Id == id);
		}

		public UsuarioModel Atualizar(UsuarioModel usuario)
		{
			UsuarioModel UsuarioDB = ListarPorId(usuario.Id);

			if (UsuarioDB == null) throw new SystemException("Houve um erro na atualização do usuario");

			UsuarioDB.Nome = usuario.Nome;
			UsuarioDB.Email = usuario.Email;
			UsuarioDB.Login = usuario.Login;
			UsuarioDB.Perfil = usuario.Perfil;
			UsuarioDB.DataAtualizacao = DateTime.Now;
			_bancoContext.Usuarios.Update(UsuarioDB);
			_bancoContext.SaveChanges();
			return UsuarioDB;
		}

		public bool Apagar(int id)
		{
			UsuarioModel UsuarioDB = ListarPorId(id);

			if (UsuarioDB == null) throw new SystemException("Houve um erro na deleção do usuário");

			_bancoContext.Usuarios.Remove(UsuarioDB);
			_bancoContext.SaveChanges();
			return true;
		}

		public UsuarioModel BuscarPorLogin(string login)
		{
			return _bancoContext.Usuarios.FirstOrDefault(x => x.Login.ToUpper() == login.ToUpper());
		}
	}
}
