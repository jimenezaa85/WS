namespace TaxRay.Web.Models
{
    /// <summary>
    /// Class that convey tax return data as request by the client
    /// </summary>
    public class TaxReturnModel
    {
        /// <summary>
        /// Identification of the tax return
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Identification of the AtlasId
        /// </summary>
        public string AtlasId { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Customer name
        /// </summary>
        public string Client { get; set; }
        /// <summary>
        /// Tax payer full name
        /// </summary>
        public string TaxPayer { get; set; }
        /// <summary>
        /// Expected date of submission.
        /// </summary>
        public string DueDate { get; set; }
        /// <summary>
        /// Current status of the transaction
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Url to request full data of the tax return
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Present the year to which corresponds the tax return
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Present the user to which assigned task
        /// </summary>
        public string AssignedToId { get; set; }

        /// <summary>
        /// Present the user name to which assigned task
        /// </summary>
        public string UserNameAssignedTo { get; set; }

        /// <summary>
        /// Present the user to which created task
        /// </summary>
        public string CreatedById { get; set; }

        /// <summary>
        /// Present the user name to which assigned task
        /// </summary>
        public string UserNameCreatedBy { get; set; }


    }
}