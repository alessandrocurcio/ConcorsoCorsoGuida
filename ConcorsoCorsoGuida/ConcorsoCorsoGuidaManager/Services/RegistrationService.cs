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
    public interface IRegistrationService
    {
        Task<string> Save(RegistrationRequest model);
        Task<IEnumerable<Registration>> GetAll();
        Task<IEnumerable<Registration>> GetIdx(List<int> idx);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AppSettings _appSettings;

        public RegistrationService(ApplicationDbContext applicationDbContext, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<string> Save(RegistrationRequest model)
        {
            Registrations registrations = new Registrations(_applicationDbContext);

            Registration registration = new Registration();
            registration.Name = model.Name;
            registration.Surname = model.Surname;
            registration.Email = model.Email;
            registration.Phone = model.Phone;
            if (model.BirthDate != null) registration.BirthDate = (DateTime)model.BirthDate;
            if (model.ReadPrivacy != null) registration.ReadPrivacy = (bool)model.ReadPrivacy;

            registrations.Save(registration);

            return "OK";
        }

        public async Task<IEnumerable<Registration>> GetAll()
        {
            Registrations registrations = new Registrations(_applicationDbContext);

            return registrations.GetAll();
        }
        public async Task<IEnumerable<Registration>> GetIdx(List<int> idx)
        {
            Registrations registrations = new Registrations(_applicationDbContext);

            return registrations.GetIdx(idx);
        }
    }
}
