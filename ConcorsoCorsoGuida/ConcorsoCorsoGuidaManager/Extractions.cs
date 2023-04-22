using ConcorsoCorsoGuidaEntities;
using ConcorsoCorsoGuidaRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcorsoCorsoGuidaManager
{
    public class Extractions
    {
        private readonly ApplicationDbContext _context;
        public Extractions(ApplicationDbContext context)
        {
            _context = context;
        }

        public Extraction Save(Extraction inputUser)
        {
            _context.Add(inputUser);
            _context.SaveChanges();
            return inputUser;
        }

    }
}
