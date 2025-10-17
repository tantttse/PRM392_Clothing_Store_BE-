using Shared.Domain.Common.DDD;

namespace ClothingStore.Domain.Entities
{
    public class Category : AggregateRoot<Guid>
    {
        public string CategoryName { get; private set; } = default!;

        public ICollection<Product> Products { get; private set; } = new List<Product>();

        private Category() { }

        public static Category Create(string name)
        {
            return new Category
            {
                CategoryName = name,
            };
        }

        public void Rename(string newName)
        {
            CategoryName = newName;
        }

        protected override void Apply(IDomainEvent @event) { }
    }
}
