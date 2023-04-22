using ConcorsoCorsoGuidaEntities.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ConcorsoCorsoGuidaEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConcorsoCorsoGuidaRepository;
using ConcorsoCorsoGuidaManager.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using OfficeOpenXml;

namespace ConcorsoCorsoGuidaController
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<User> _logger;
        private IRegistrationService _registrationService;
        private IExtractionService _extractionService;

        public RegistrationController(ApplicationDbContext applicationDbContext, ILogger<User> logger, IRegistrationService registrationService, IExtractionService extractionService)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _registrationService = registrationService;
            _extractionService = extractionService;
        }

        [HttpPost]
        [Route("Save")]
        public async Task<ApiResponse<string>> Save(RegistrationRequest model)
        {
            try
            {
                var response = await _registrationService.Save(model);
                return new ApiResponse<string>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ApiResponse<string>(ex);

            }
        }

        [HttpPost]
        [Route("GetAll")]
        public async Task<ApiResponse<IEnumerable<Registration>>> GetAll(RegistrationRequest model)
        {
            try
            {
                var response = await _registrationService.GetAll();
                return new ApiResponse<IEnumerable<Registration>>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ApiResponse<IEnumerable<Registration>>(ex);

            }
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export()
        {
            var fileName = "registrations.xlsx";
            var mimeType = "application/octet-stream";
            var response = await _registrationService.GetAll();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Surname");
            dt.Columns.Add("Email");
            dt.Columns.Add("Phone");
            foreach (var item in response)
            {
                DataRow row = dt.NewRow();
                row["ID"] = item.Id;
                row["Name"] = item.Name;
                row["Surname"] = item.Surname;
                row["Email"] = item.Email;
                row["Phone"] = item.Phone;
                dt.Rows.Add(row);
            }

            Stream stream = new MemoryStream();
            using (ExcelPackage pck = new ExcelPackage(stream))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Registrations");
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                pck.Save();
            }
            stream.Position = 0;
            return new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = fileName
            };

        }


        [HttpPost]
        [Route("ExportRnd")]
        public async Task<IActionResult> ExportRnd(RandomRequest randomRequest)
        {
            var currentDate = new DateTime();
            Random rnd = new Random();
            var rowList = new List<int>();
            for (int j = 0; j < randomRequest.NRow; j++)
            {
                repeat: var newNum = rnd.Next(randomRequest.TotalRow) + 1;
                if (!rowList.Contains(newNum))
                    rowList.Add(newNum); // returns random integers >= 10 and < 19
                else
                    goto repeat;
            }
            await _extractionService.Save(rowList);
            
            var fileName = "registrations.xlsx";
            var mimeType = "application/octet-stream";
            var response = await _registrationService.GetIdx(rowList);
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Surname");
            dt.Columns.Add("Email");
            dt.Columns.Add("Phone");
            foreach (var item in response)
            {
                DataRow row = dt.NewRow();
                row["ID"] = item.Id;
                row["Name"] = item.Name;
                row["Surname"] = item.Surname;
                row["Email"] = item.Email;
                row["Phone"] = item.Phone;
                dt.Rows.Add(row);
            }

            Stream stream = new MemoryStream();
            using (ExcelPackage pck = new ExcelPackage(stream))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Registrations");
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                pck.Save();
            }
            stream.Position = 0;
            return new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = fileName
            };

        }

    }
}
