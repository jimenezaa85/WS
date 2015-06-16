using System.Diagnostics.CodeAnalysis;

namespace TaxRay.Web.Areas.HelpPage.ModelDescriptions
{
    [ExcludeFromCodeCoverage]
    public class KeyValuePairModelDescription : ModelDescription
    {
        public ModelDescription KeyModelDescription { get; set; }

        public ModelDescription ValueModelDescription { get; set; }
    }
}