using System;
using System.Diagnostics.CodeAnalysis;

namespace TaxRay.Web.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// Describes a type model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class ModelDescription
    {
        public string Documentation { get; set; }

        public Type ModelType { get; set; }

        public string Name { get; set; }
    }
}