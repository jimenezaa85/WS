using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TaxRay.Contracts;
using TaxRay.Model;
using TaxRay.Web.Models;

namespace TaxRay.Web.Controllers
{
    public class LookupsController : BaseApiController
    {
        public LookupsController(ITaxRayUow uow) 
            : base(uow)
        { }

        //GET /api/lookups/users
        [ActionName("users")]
        public IEnumerable<User> GetUsers()
        {
            return Uow.Users.GetAll();
        }

    }
}