using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContatos.Controllers
{
	public class ContatoController : Controller
	{
		private readonly IContatoRepositorio _contatoRepositorio;
		public ContatoController(IContatoRepositorio contatoRepositorio)
		{
			_contatoRepositorio = contatoRepositorio;
		}

		public IActionResult Index()
		{
			//var contatos = _contatoRepositorio.BuscarTodos();

			List<ContatoModel> contatos = _contatoRepositorio.BuscarTodos();
			return View(contatos);
		}

		public IActionResult Criar()
		{
			return View();
		}

		public IActionResult Editar(int id)
		{
			ContatoModel contato =  _contatoRepositorio.ListarPorId(id);
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
				bool apagado = _contatoRepositorio.Apagar(id);

				if (apagado)
				{
					TempData["MensagemSucesso"] = "Contato excluido com sucesso";
				}
				else
				{
					TempData["MensagemSucesso"] = "Ops não conseguimos excluir seu contato";
				}
				return RedirectToAction("Index");
			}
			catch (Exception erro)
			{

				TempData["MensagemErro"] = $"Houve algum problema ao tentar excluir, detalhe do problema:{erro.Message}";
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
					_contatoRepositorio.Adicionar(contato);
					TempData["MensagemSucesso"] = "Contato cadastrado com sucesso";
					return RedirectToAction("Index");
				}

				return View(contato);
			}
			catch (Exception erro)
			{

				TempData["MensagemErro"] = $"Houve algum problema ao tentar cadastrar, detalhe do problema:{erro.Message}";
				return RedirectToAction("Index");
			}
			
		}

		public IActionResult Alterar(ContatoModel contato)
		{
			try
			{

				if (ModelState.IsValid)
				{
					_contatoRepositorio.Atualizar(contato);
					TempData["MensagemSucesso"] = "Contato alterado com sucesso";
					return RedirectToAction("Index");
				}
				return View("Editar", contato);

			}
			catch (Exception erro)
			{

				TempData["MensagemErro"] = $"Houve algum problema ao tentar alterar, detalhe do problema:{erro.Message}";
				return RedirectToAction("Index");
			}
		}
	}
}
