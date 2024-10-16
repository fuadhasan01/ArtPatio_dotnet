using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ArtPatio.Controllers
{
    public class DemoController : Controller
    {
        private DataTable getAll()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            dt.Rows.Add(1, "Apple");
            dt.Rows.Add(2, "Mango");
            dt.Rows.Add(3, "Banana");

            return dt;
        }

        [HttpPost]
        public JsonResult GetTable() 
        {
            DataTable dt = getAll();
            string TableData = "";

            foreach (DataRow row in dt.Rows)
            {
                TableData += "<tr>"
                + "<td>" + row["Id"] + "</td>"
                + "<td>" + row["Name"] + "</td>"
                + "</tr>";

            }

            return Json(new { TableData = TableData });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
