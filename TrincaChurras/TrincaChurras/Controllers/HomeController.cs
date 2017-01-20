using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrincaChurras.Database;
using TrincaChurras.Models;
using TrincaChurras.Support.Extensions;
using TrincaChurras.Support.Models;

namespace TrincaChurras.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _churrasRepository = new FlatFileRepository();

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Title = "Início";
            return View();
        }

        public ActionResult ChurrasSource(int draw, Dictionary<string, string>[] order, ClientPagination pPagination, Dictionary<string, string> pModelSearch = null)
        {
            var models = _churrasRepository.Get().Select(c => (ChurrasGridModel)c);

            return Content(models.DataTablesSerialize(draw, order, pPagination, pModelSearch), "application/json");
        }
    }
}