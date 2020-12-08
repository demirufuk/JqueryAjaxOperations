using JqueryAjaxOperations.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JqueryAjaxOperations.Controllers
{
    public class EmployeeController : Controller
    {

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllEmployee());
        }

        IEnumerable<Employee> GetAllEmployee()
        {
            using (DBModel db = new DBModel())
            {
                return db.Employee.ToList();
            }
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Employee emp = new Employee();

            if (id != 0)
            {
                using (DBModel db = new DBModel())
                {
                    emp = db.Employee.Where(x => x.Id == id).FirstOrDefault();
                }
            }

            return View(emp);

        }

        [HttpPost]
        public ActionResult AddOrEdit(Employee emp)
        {
            try
            {
                if (emp.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                    string extension = Path.GetExtension(emp.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    emp.ImagePath = "~/AppFiles/Images/" + fileName;
                    emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                using (DBModel db = new DBModel())
                {
                    if(emp.Id == 0)
                    {
                        db.Employee.Add(emp);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()),
                    message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "false" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using(DBModel db = new DBModel())
                {
                    Employee emp = db.Employee.Where(x => x.Id == id).FirstOrDefault<Employee>();
                    db.Employee.Remove(emp);
                    db.SaveChanges();
                }

                return Json(new
                {
                    success = true,
                    html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()),
                    message = "Deleted Successfully"
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "false" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}