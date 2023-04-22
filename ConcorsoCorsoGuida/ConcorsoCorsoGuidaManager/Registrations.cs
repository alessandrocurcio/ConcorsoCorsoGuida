using ConcorsoCorsoGuidaEntities;
using ConcorsoCorsoGuidaRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcorsoCorsoGuidaManager
{
    public class Registrations
    {
        private readonly ApplicationDbContext _context;
        public Registrations(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Registration> GetAll()
        {
            return _context.Registrations.ToList();
        }

        public IEnumerable<Registration> GetIdx(List<int> idx)
        {
            var listReg = new List<Registration>();
            foreach (var item in idx)
            {
                var reg = _context.Registrations.Where(x => x.Id == item).FirstOrDefault();
                if (reg != null) listReg.Add(reg);
            }
            return listReg;
        }

        public Registration Save(Registration inputUser)
        {
            _context.Add(inputUser);
            _context.SaveChanges();
            return inputUser;
        }

    }
}
