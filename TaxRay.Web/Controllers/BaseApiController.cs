using System.Web.Http;
using TaxRay.Contracts;
using TaxRay.Web.Models;

namespace TaxRay.Web.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected ITaxRayUow Uow { get; set; }
        ModelFactory _modelFactory;

        protected BaseApiController(ITaxRayUow uow)
        {
            Uow = uow;
        }

        protected ModelFactory ModelFactory
        {
            get { return _modelFactory ?? (_modelFactory = new ModelFactory(Request)); }
        }

        protected override void Dispose(bool disposing)
        {
            if (Uow != null)
            {
                Uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}