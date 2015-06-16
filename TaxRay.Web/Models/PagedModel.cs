using System.Collections.Generic;

namespace TaxRay.Web.Models
{
    /// <summary>
    /// Class used to convert paginated results
    /// </summary>
    /// <typeparam name="T">Model type of the results</typeparam>
    public class PagedModel<T> where T: class
    {

        /// <summary>
        /// Current Page
        /// </summary>
        public int currentPage { get; set; }
        /// <summary>
        /// Total number of records
        /// </summary>
        public int  TotalCount { get; set; }
        /// <summary>
        /// Total number of records.
        /// </summary>
        public double TotalPage { get; set; }
        /// <summary>
        /// Previous page Url
        /// </summary>
        public string PrevPageUrl { get; set; }
        /// <summary>
        /// Next page Url
        /// </summary>
        public string NextPageUrl { get; set; }
        /// <summary>
        /// First page Url
        /// </summary>
        public string FirstPageUrl { get; set; }
        /// <summary>
        /// Last page Url
        /// </summary>
        public string LastPageUrl { get; set; }
        /// <summary>
        /// Typed Results collection
        /// </summary>
        public IEnumerable<T> Results { get; set; }
    }
}