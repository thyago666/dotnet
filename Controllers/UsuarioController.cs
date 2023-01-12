using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContatos.Controllers
{
	public class UsuarioController : Controller
	{
		private readonly IUsuarioRepositorio _usuarioRepositorio;
		public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
		{
			_usuarioRepositorio = usuarioRepositorio;
		}
		public IActionResult Index()
		{
			List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();
			return View(usuarios);
		}

		public IActionResult Criar()
		{
			return View();
		}

		public IActionResult Editar(int id)
		{
			UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
			return View(usuario);
		}

		public IActionResult Alterar(UsuarioSemSenhaModel usuarioSemSenhaModel)
		{
			try
			{
				UsuarioModel usuario = null;

				if (ModelState.IsValid)
				{
					usuario = new UsuarioModel()
					{
						Id = usuarioSemSenhaModel.Id,
						Nome = usuarioSemSenhaModel.Nome,
						Login = usuarioSemSenhaModel.Login,
						Email = usuarioSemSenhaModel.Email,
						Perfil = usuarioSemSenhaModel.Perfil
					};

					usuario = _usuarioRepositorio.Atualizar(usuario);
					TempData["MensagemSucesso"] = "Usuário alterado com sucesso";
					return RedirectToAction("Index");
				}
				return View("Editar", usuario);

			}
			catch (Exception erro)
			{

				TempData["MensagemErro"] = $"Houve algum problema ao tentar alterar, detalhe do problema:{erro.Message}";
				return RedirectToAction("Index");
			}
		}

		public IActionResult Apagar(int id)
		{
			try
			{
				bool apagado = _usuarioRepositorio.Apagar(id);

				if (apagado)
				{
					TempData["MensagemSucesso"] = "Usuário excluido com sucesso";
				}
				else
				{
					TempData["MensagemSucesso"] = "Ops não conseguimos excluir seu usuário";
				}
				return RedirectToAction("Index");
			}
			catch (Exception erro)
			{

				TempData["MensagemErro"] = $"Houve algum problema ao tentar excluir, detalhe do problema:{erro.Message}";
				return RedirectToAction("Index");
			}
		}
		public IActionResult ApagarConfirmacao(int id)
		{
			UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
			return View(usuario);
		}

		[HttpPost]
		public IActionResult Criar(UsuarioModel usuario)
		{

			try
			{
				if (ModelState.IsValid)
				{
					_usuarioRepositorio.Adicionar(usuario);
					TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso";
					return RedirectToAction("Index");
				}

				return View(usuario);
			}
			catch (Exception erro)
			{

				TempData["MensagemErro"] = $"Houve algum problema ao tentar cadastrar, detalhe do problema:{erro.Message}";
				return RedirectToAction("Index");
			}

		}
	}
}
