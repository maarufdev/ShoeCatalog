using ShoeCatalog.DataModels.ViewModels.Common;

namespace ShoeCatalog.DataModels.ViewModels.ShoeVM
{
    public class ShoeDetailsVM
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? BrandName { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? ImageFileName { get; set; }
        public string? Color { get; set; }
        public decimal Size { get; set; }
        public Gender Gender { get; set; }
        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public bool IsActive { get; set; }
        public List<string>? ShoeCategories { get; set; }
    }
}
