using System;
using System.Linq;
using System.Web.Mvc;
using TrincaChurras.Database;
using TrincaChurras.Models;

namespace TrincaChurras.Controllers
{
    public class ParticipanteController : Controller
    {
        private IRepository _repository = new FlatFileRepository();

        [HttpGet]
        public ActionResult New(Guid Id)
        {
            ViewBag.Title = "Adicionar Participante";

            ViewBag.Churras = (ChurrasModel)_repository.GetById(Id);

            return View("Crud", new ParticipanteModel() {
                IdChurras = Id
            });
        }

        [HttpGet]
        public ActionResult Edit(Guid Id, Guid pIdChurras)
        {
            var churras = _repository.GetById(pIdChurras);
            ViewBag.Title = "Editar Participante";

            ViewBag.Churras = (ChurrasModel)churras;

            return View("Crud", (ParticipanteModel)churras.Participantes.First(p => p.Id == Id));
        }

        [HttpPost]
        public ActionResult Save(ParticipanteModel pModel)
        {
            try
            {
                _repository.AddOrUpdateParticipante(pModel.ToEntity());

                return Json(new
                {
                    success = true
                }
                , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = string.Format("Participante não adicionado, erro no sistema (Exception: {0}) :(. Ainda bem que esse form é AJAX e não perdemos os dados :P", ex.Message),
                }
                , JsonRequestBehavior.AllowGet);
            }
        }
    }
}