using ConcorsoCorsoGuidaEntities;
using ConcorsoCorsoGuidaRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcorsoCorsoGuidaManager
{
    public class Users
    {
        private readonly ApplicationDbContext _context;
        public Users(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User Save(User inputUser)
        {
            if (inputUser.Id > 0)
            {
                var user = _context.Users.Where(x => x.Id == inputUser.Id).FirstOrDefault();
                if (user == null) throw new Exception("Utente non trovato");
                else
                {
                    user.Username = inputUser.Username;
                    user.Enabled = inputUser.Enabled;
                    user.FirstName = inputUser.FirstName;
                    user.LastName = inputUser.LastName;
                    user.Email = inputUser.Email;
                    user.Role = inputUser.Role;
                }
            }
            else
            {
                _context.Add(inputUser);
            }
            _context.SaveChanges();
            return inputUser;
        }

        public bool Delete(User inputUser)
        {
            if (inputUser.Id > 0)
            {
                var user = _context.Users.Where(x => x.Id == inputUser.Id).FirstOrDefault();
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }      
    }
}
