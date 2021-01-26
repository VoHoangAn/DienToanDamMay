using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestDockerWeb.MakeItWork;
using TestDockerWeb.Models;

namespace TestDockerWeb.Controllers
{
    [LoginController]
    [AdminFilterController]
    public class AdminController : Controller
    {
        private DienToanDamMayEntities db = new DienToanDamMayEntities();

        // GET: Admin
        public ActionResult ManageContainer()
        {
            var container = db.ManageContainer.Include(c => c.Account);
            return View(container.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageContainer container = db.ManageContainer.Find(id);
            if (container == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
