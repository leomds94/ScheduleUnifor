using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ScheduleUnifor.Helpers;
using ScheduleUnifor.Model;

namespace ScheduleUnifor.Controllers
{
    [Route("api/[controller]")]
    public class ImportExportController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImportExportController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public void Import()
        {
            ImportResponse response = ImportHelper.Import(_hostingEnvironment.WebRootPath, @"Turmas.xlsx", Request.Scheme, Request.Host);
            List<Reserva> reservas = ConvertImportToObject.Convert(response);
        }
    }
}
