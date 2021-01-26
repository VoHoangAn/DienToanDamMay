using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TestDockerWeb.Models;

namespace TestDockerWeb.MakeItWork
{
    public static class Feature
    {

        static Feature()
        {

        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string Login(string username, string password)
        {
            Account loginAccount = GetAccount(username, password);

            if(loginAccount != null)
            {
                if (loginAccount.isadmin)
                    return "Admin";
                else
                    return "User";
            }
            else
                return "Fail";
        }

        public static Account GetAccount(string username, string password)
        {
            using (DienToanDamMayEntities db=new DienToanDamMayEntities())
            {
                string inputPasswordMD5 = Feature.CreateMD5(password);

                Account accountFromDB = 
                    db.Account.Where(user => user.username == username&&user.password==inputPasswordMD5)
                    .FirstOrDefault();

                return accountFromDB;
            }
        }

        public static void SaveContainer(ManageContainer newContainer)
        {
            using (DienToanDamMayEntities db=new DienToanDamMayEntities())
            {
                db.ManageContainer.Add(newContainer);
                db.SaveChanges();
            }
        }

        public static void RemoveContainer(string containerRemovedName)
        {
            using (DienToanDamMayEntities db = new DienToanDamMayEntities())
            {
                ManageContainer removedContainer = db.ManageContainer
                    .Where(mc => mc.containername == containerRemovedName).FirstOrDefault();

                db.ManageContainer.Remove(removedContainer);
                db.SaveChanges();
            }
        }
    }
}