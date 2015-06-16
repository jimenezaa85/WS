using System;
using System.Net.Http;
using System.Web.Http.Routing;
using TaxRay.Model;

namespace TaxRay.Web.Models
{
    /// <summary>
    /// Factory that create api response models to make sure only necesary information is sent to the client
    /// </summary>
    public class ModelFactory
    {
        private readonly UrlHelper _urlHelper;

        /// <summary>
        /// Only present constructor
        /// </summary>
        /// <param name="request">Current request object</param>
        public ModelFactory(HttpRequestMessage request)
        {
            _urlHelper = new UrlHelper(request);
        }

        private string GetDate(DateTime? date)
        {
            if (date.HasValue)
                return string.Format("{0}.{1}.{2}",
                                     date.Value.Day.ToString().PadLeft(2, '0'),
                                     date.Value.Month.ToString().PadLeft(2, '0'),
                                     date.Value.Year
                                     );
            return "";
        }

        /// <summary>
        /// Create Tax Return model instance
        /// </summary>
        /// <param name="taxReturn">Tax Return Business Object</param>
        /// <returns></returns>

        public TaxReturnModel Create(TaxReturn taxReturn)
        {
            return new TaxReturnModel
            {
                Id = taxReturn.Id,
                AtlasId= taxReturn.AtlasId,
                Description = taxReturn.Description,
                Client = taxReturn.Client,
                TaxPayer = taxReturn.TaxPayer,
                DueDate =GetDate(taxReturn.DueDate),
                Status = taxReturn.Status,
                Url = CreateItemUrl(taxReturn.Id),
                Year = taxReturn.Year,
                AssignedToId= taxReturn.AssignedToId,
                userNameAssignedTo = taxReturn.AssignedTo == null ? null : taxReturn.AssignedTo.Username,
                CreatedById= taxReturn.CreatedById,
                userNameCreatedBy = taxReturn.CreatedBy == null ? null : taxReturn.CreatedBy.Username
            };
        }

        private string CreateItemUrl(int? identification)
        {
            if (identification.HasValue)
                return _urlHelper.Link(WebApiConfig.ControllerAndId, new { id = identification });
            return "";
        }

        /// <summary>
        /// Create a navigable url the to item details action
        /// </summary>
        /// <param name="number">Item ID</param>
        /// <returns></returns>
        public string CreatePageUrl(int? number)
        {
            if (number.HasValue)
                return _urlHelper.Link(WebApiConfig.ControllerAndId, new { pageNumber = number });
            return "";
        }

        /// <summary>
        /// Create a navigable url the to item details action
        /// </summary>
        /// <param name="number">page ID</param>
        /// <param name="size">Items per page </param>
        /// <param name="total"> total items</param>
        /// <returns> a new instance of LinkBuilder class</returns>
        public LinkBuilder CreatePageUrlPagination(int? number, int? size, long? total)
        {
            if (number.HasValue && size.HasValue)
            {
                return new LinkBuilder(_urlHelper, WebApiConfig.ControllerAction, number.Value, size.Value, total ?? 0);
            }                
            return null;
        }
    }
}