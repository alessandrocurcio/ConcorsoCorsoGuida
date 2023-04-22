using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ConcorsoCorsoGuidaEntities;
using ConcorsoCorsoGuidaRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Security.Cryptography;

namespace ConcorsoCorsoGuidaManager.Services
{
    public interface IExtractionService
    {
        Task<string> Save(List<int> model);
    }

    public class ExtractionService : IExtractionService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AppSettings _appSettings;

        public ExtractionService(ApplicationDbContext applicationDbContext, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<string> Save(List<int> model)
        {
            Extractions extractions = new Extractions(_applicationDbContext);
            var currentDate = DateTime.Now;
            foreach (var item in model)
            {
                Extraction extraction = new Extraction();
                extraction.IdRegistration = item;
                extraction.Date = currentDate;
                extractions.Save(extraction);

            }

            return "OK";
        }

    }
}
