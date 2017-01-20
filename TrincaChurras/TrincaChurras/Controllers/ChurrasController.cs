using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using TrincaChurras.Database;
using TrincaChurras.Models;
using TrincaChurras.Support.Extensions;
using TrincaChurras.Support.Models;

namespace TrincaChurras.Controllers
{
    public class ChurrasController : Controller
    {
        private IRepository _repository = new FlatFileRepository();

        [HttpGet]
        public ActionResult New()
        {
            ViewBag.Title = "Novo Churras";
            ViewBag.SugestaoComBebida = decimal.Parse(ConfigurationManager.AppSettings["DefaultValorComBebida"]);
            ViewBag.SugestaoSemBebida = decimal.Parse(ConfigurationManager.AppSettings["DefaultValorSemBebida"]);

            return View("Crud");
        }

        [HttpPost]
        public ActionResult Save(ChurrasModel pModel)
        {
            try
            {
                var idChurras = _repository.SaveOrUpdate(pModel.ToEntity());

                TempData["SuccessMessageView"] = "Churras confirmado com sucesso! :D";

                return Json(new
                {
                    success = true,
                    id = idChurras
                }
                , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = string.Format("Churras não confirmado, erro no sistema (Exception: {0}) :(. Ainda bem que esse form é AJAX e não perdemos os dados :P", ex.Message),
                }
                , JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult View(Guid id)
        {
            var model = (ChurrasModel)_repository.GetById(id);
            ViewBag.Title = model.Descricao;
            return View(model);
        }

        public ActionResult ListarParticipantes(Guid id)
        {
            var churras = (ChurrasModel)_repository.GetById(id);
            ViewBag.Title = string.Format("Participantes - {0} - {1}", churras.Descricao, churras.Data.ToString("dd/MM/yyyy HH:mm"));
            ViewBag.IdChurras = churras.Id;
            return View();
        }

        public ActionResult ParticipantesSource(int draw, Dictionary<string, string>[] order, ClientPagination pPagination, Dictionary<string, string> pModelSearch = null, Dictionary<string, string> pExtraData = null) {
            var participantesModels = _repository.GetById(Guid.Parse(pExtraData["IdChurras"])).Participantes.Select(p => (ParticipanteGridModel)p);

            return Content(participantesModels.DataTablesSerialize(draw, order, pPagination, pModelSearch), "application/json");
        }
    }
}