using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContatos.Controllers
{
	public class LoginController : Controller
	{

		private readonly IUsuarioRepositorio _usuarioRepositorio;

		private readonly ISessao _sessao;
		public LoginController(IUsuarioRepositorio  usuarioRepositorio, ISessao sessao)
			{
			_usuarioRepositorio = usuarioRepositorio;
			_sessao = sessao;
			}

		public IActionResult Sair()
		{
			_sessao.RemoverSessaoUsuario();
			return RedirectToAction("Index", "Login");
		}
		public IActionResult Index()
		{
			//se estiver logado redirecionar para home
			if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");
			return View();
		}

		[HttpPost]

		public IActionResult Entrar(LoginModel loginModel)
		{

			try
			{

				if (ModelState.IsValid)
				{

					UsuarioModel usuario =  _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

					//if (loginModel.Login == "adm" && loginModel.Senha == "123")
					if (usuario != null)
					{
						if (usuario.SenhaValida(loginModel.Senha))
						{
							_sessao.CriarSessaoDoUsuario(usuario);
							return RedirectToAction("Index", "Home");
						}
						TempData["MensagemErro"] = $"A senha do usuário é invalida, tente novamente.";

					}
					TempData["MensagemErro"] = $"Login e/ou Senha Invalido(s), tente novamente.";

				}

				return View ("Index");

			}
			catch (Exception erro)
			{


				TempData["MensagemErro"] = $"Houve algum problema ao realizar seu login, tente novamente, detalhe do problema:{erro.Message}";
				return RedirectToAction("Index");
			}

		}
	}
}
