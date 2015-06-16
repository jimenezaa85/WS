using System.Web.Http;
using TaxRay.Models;

namespace TaxRay.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        ModelFactory _modelFactory;

        protected ModelFactory ModelFactory
        {
            get { return _modelFactory ?? (_modelFactory = new ModelFactory(Request)); }
        }
    }
}