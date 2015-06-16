using System;
using System.Web.Http.Routing;

namespace TaxRay.Models
{


    /// <summary>
    /// Class creates all URL links need for  view pagination
    /// </summary>
    public class LinkBuilder
    {

        private const String PAGE_NUMBER = "pageNumber";
        private const String PAGE_SIZE = "pageSize";

        /// <summary>
        /// Identification of the URI related with first page in view pagination.
        /// </summary>
        public Uri FirstPage { get; private set; }
        /// <summary>
        /// Identification of the URI related with last page in view pagination.
        /// </summary>
        public Uri LastPage { get; private set; }
        /// <summary>
        /// Identification of the URI related with next page in view pagination.
        /// </summary>
        public Uri NextPage { get; private set; }
        /// <summary>
        /// Identification of the URI related with previoust page in view pagination.
        /// </summary>
        public Uri PreviousPage { get; private set; }

        /// <summary>
        /// Create linkBuilder instance
        /// </summary>
        /// <param name="urlHelper">instance of UrlHelper class </param>
        /// <param name="routeName">Name of Route WebApiConfig</param>
        /// <param name="pageNo">id of current page</param>
        /// <param name="pSize">number of items per page</param>
        /// <param name="totalRecordCount">Total count of items</param>
        public LinkBuilder(UrlHelper urlHelper, string routeName, int pageNo, int pSize, long totalRecordCount)
        {
            object routeValues = null ;
            // Determine total number of pages
            var pageCount = totalRecordCount > 0 ? (int)Math.Ceiling(totalRecordCount / (double)pSize) : 0;

            // Create them page links 
            String link=String.Empty;
            link=urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
            {
                {PAGE_NUMBER,  1},
                {PAGE_SIZE, pSize}
            });
            if(link!=null) FirstPage = new Uri(link);

            link = urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
            {
                {PAGE_NUMBER,  pageCount},
                {PAGE_SIZE, pSize}
            });
            if (link != null) LastPage = new Uri(link);
            
            if (pageNo > 1)
            {

                link = urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
                {
                    {PAGE_NUMBER,   pageNo - 1},
                    {PAGE_SIZE, pSize}
                });
                if (link != null) PreviousPage = new Uri(link);

                
            }
            if (pageNo < pageCount)
            {
                link = urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
                {
                    {PAGE_NUMBER,  pageNo + 1},
                    {PAGE_SIZE, pSize}
                });
                if (link != null) NextPage = new Uri(link);

            }
        }
    }


}