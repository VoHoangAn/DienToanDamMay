using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDockerWeb.MakeItWork;
using TestDockerWeb.Models;
using System.Data.Entity;


namespace TestDockerWeb.Controllers
{
    
    public class HomeController : Controller
    {
        DienToanDamMayEntities db = new DienToanDamMayEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            string loginState = Feature.Login(username, password);

            if(loginState!="Fail")
            {
                CreateSession(Feature.GetAccount(username,password));

                if(loginState=="Admin")
                {
                    return Content("Admin");
                }
                else
                {
                    return Content("User");
                }
            }
            else 
            {
                return Content("Fail");
            }
        }

        private void CreateSession(Account loginAccount)
        {
            Session["User"] = loginAccount;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password)
        {
            Account checkUserName = db.Account.Where(a => a.username == username).FirstOrDefault();
            if(checkUserName!=null)
            {
                return Content("UserNameExist");
            }

            //return View();
            Account newAcc = new Account
            {
                username = username,
                password = Feature.CreateMD5(password)
            };

            db.Account.Add(newAcc);
            db.SaveChanges();

            return Content("Success");
        }

        [LoginController]
        public Account GetCurrentUserInfo()
        {
            return Session["User"] as Account;
        }

        [LoginController]
        public ActionResult InitialContainer()
        {
            return View();
        }

        [LoginController]
        [HttpPost]
        public ActionResult InitialContainer(string containerName, string sshKey, double cpu, string ram)
        {
            bool checkContainerNameExist = db.ManageContainer.Any(mc => mc.containername == containerName);
            if (checkContainerNameExist)
            {
                return Content("ContainerNameExist");
            }

            Account curUser = GetCurrentUserInfo();
            DockerCommand.InitialContainer(curUser.username, containerName, cpu, ram, sshKey);
            //ManageContainer newContainer = new ManageContainer
            //{
            //    username = "anh",
            //    containername = "test",
            //    ip = "192.168.1.2",
            //    port = 7001,
            //    cpu = 1,
            //    ram = "1g",
            //    sshkey = "",
            //    enddate = DateTime.Now.AddDays(3),
            //    status = "Đang chạy",
            //    sshcommand = "ssh root@192.168.1.2 -p 7001"
            //};

            //db.ManageContainer.Add(newContainer);
            //db.SaveChanges();

            return Content("Success");
        }

        //[LoginController]
        //[HttpPost]
        //public ActionResult RunContainer(string containerName)
        //{
        //    DockerCommand.RunContainer(containerName);
        //    return new EmptyResult();
        //}

        [LoginController]
        [HttpPost]
        public ActionResult StopContainer(string containerName)
        {
            DockerCommand.StopContainer(containerName);
            return new EmptyResult();
        }

        [LoginController]
        public ActionResult UserContainer()
        {
            Account curUser = GetCurrentUserInfo();

            List<ManageContainer> manageContainers = db.ManageContainer
                .Where(c => c.username == curUser.username).ToList();

            return View(manageContainers);
        }


        [LoginController]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [LoginController]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [LoginController]
        [HttpPost]
        public ActionResult ChangePassword(string newPass)
        {
            Account curUser = GetCurrentUserInfo();

            curUser.password = Feature.CreateMD5(newPass);

            db.Entry(curUser).State = EntityState.Modified;
            db.SaveChanges();

            return Content("Success");
        }
    }
}