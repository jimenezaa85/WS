using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaxRay.Models;
using TaxRay.Web.Models;

namespace TaxRay.Controllers
{
    /// <summary>
    /// Tax return information controller
    /// </summary>
    /// 
    [Authorize]
    public class TaxReturnController : BaseApiController
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();


        public IHttpActionResult Get(int id)
        {
            TaxReturn tax = _context.Tasks.Find(id);
            if (tax == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return Ok(ModelFactory.Create(tax));
        }

        /// <summary>
        /// Return  all current tax-return
        /// </summary>
        /// <returns>
        /// The method returns a list of TaxReturnModel
        /// </returns>
        [ActionName("all")]
        public IEnumerable<TaxReturnModel> GetAll()
        {
            IQueryable<TaxReturn> query = _context.Tasks.OrderBy(t => t.Description);
            var results = query.ToList().Select(m => ModelFactory.Create(m));


            return results;
        }

        /// <summary>
        /// Return current tax-return summaries, ordered by description and paged
        /// </summary>
        /// <param name="page">Page. Default value is zero</param>
        /// <param name="pageSize">Page size. Default size is 10</param>
        /// <param name="sortColumn">Field. Default value is description</param>
        /// <param name="sortDirection">Dir. Default value is asc</param>
        /// <returns>
        /// The method returns an new object PagModel.
        /// </returns>
        [ActionName("assigned")]
        public PagedModel<TaxReturnModel> GetAssigned(int page = 1, int pageSize = 10, string sortColumn = "description", string sortDirection = "asc")
        {
            var userName = User.Identity.Name;
            //userName = "zwillis";

            IEnumerable<TaxReturn> query = _context.Tasks.GetAssignedTasks(userName).AsQueryable();

            Func<TaxReturn, Object> order;
            switch (sortColumn)
            {
                case "description":
                    order = t => t.Description;
                    break;
                case "client":
                    order = t => t.Client;
                    break;
                case "dueDate":
                    order = t => t.DueDate;
                    break;
                case "status":
                    order = t => t.Status;
                    break;
                case "taxPayer":
                    order = t => t.TaxPayer;
                    break;
                case "year":
                    order = t => t.Year;
                    break;
                case "atlasId":
                    order = t => t.AtlasId;
                    break;
                case "userNameAssignedTo":
                    order = t => t.AssignedTo.UserName;
                    break;
                case "userNameCreatedBy":
                    order = t => t.CreatedBy.UserName;
                    break;
                default:
                    order = t => t.Description;
                    break;
            }

            switch (sortDirection)
            {
                case "asc":
                    query = query.OrderBy(order).AsQueryable();
                    break;
                case "desc":
                    query = query.OrderByDescending(order).AsQueryable();
                    break;
            }

            var totalCount = query.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            if (page > totalPages) { page = Convert.ToInt32(totalPages); }
            if (page < 0 || page == 0) { page = 1; }
            if (pageSize > totalCount) { pageSize = totalCount; }

            // Determine the number of records to skip
            int skip = (page - 1) * pageSize;

            var linkBuilder = ModelFactory.CreatePageUrlPagination(page, pageSize, totalCount);

            var results = query.Skip(skip)
                .Take(pageSize)
                .ToList()
                .Select(t => ModelFactory.Create(t));

            return new PagedModel<TaxReturnModel>
            {
                TotalCount = totalCount,
                TotalPage = totalPages,
                FirstPageUrl = linkBuilder.FirstPage != null ? linkBuilder.FirstPage.ToString() : "",
                NextPageUrl = linkBuilder.NextPage != null ? linkBuilder.NextPage.ToString() : "",
                PrevPageUrl = linkBuilder.PreviousPage != null ? linkBuilder.PreviousPage.ToString() : "",
                LastPageUrl = linkBuilder.NextPage != null ? linkBuilder.LastPage.ToString() : "",
                currentPage = page,
                Results = results
            };
        }

        /// <summary>
        /// Return HttpResponseMessage
        /// </summary>
        /// <param name="taxId">Identifier tax we will update</param>
        /// <param name="currentId">Identifier user logged</param>
        /// <param name="assignUserId">Identifier user assign to task</param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage Assign(int taxId, string currentId, string assignUserId)
        {
            try
            {
                _context.Tasks.Assign(taxId, currentId, assignUserId);
                _context.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                //Log
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        /// <summary>
        /// Return HttpResponseMessage
        /// </summary>
        /// <param name="tax">Tax to update</param>
        /// <returns></returns>
        public HttpResponseMessage Put(TaxReturn tax)
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            try
            {
                _context.Tasks.Update(tax, _context);
                _context.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                //Log
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        /// <summary>
        /// Return IHttpActionResult
        /// </summary>
        /// <param name="id">Identifier tax to delete</param>
        /// <returns></returns>
        public IHttpActionResult Delete(int id)
        {
            _context.Tasks.Delete(id, _context);
            _context.SaveChanges();
            return Ok();
        }

    }
}