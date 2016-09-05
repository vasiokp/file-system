using FileSystem.Interfaces;
using FileSystem.Managers;
using System.Web.Mvc;

namespace FileSystem.Controllers
{
	public class HomeController : Controller
    {
		private IDirectoryManager directoryManager;

		public HomeController()
		{
			this.directoryManager = new DirectoryManager();
		}

        public ActionResult Index()
        {
            return View();
        }

		public JsonResult Browse(string currentPath, string pathToBrowse)
		{
			return Json(directoryManager.Read(currentPath, pathToBrowse));
		}

		public JsonResult GetCounts(string currentPath, string pathToBrowse)
		{
			return Json(directoryManager.GetCounts(currentPath, pathToBrowse));
		}
    }
}