
using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        public UserRepo(SofcoContext context = null) : base(context)
        { 
        }

        public override IEnumerable<User> GetAll()
        {
            return base._context.Set<User>()
                .Include(x => x.Profile).ToList();
        }
    }
}
