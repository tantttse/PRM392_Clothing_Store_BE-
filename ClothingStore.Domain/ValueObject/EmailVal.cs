using ValueOf;

namespace ClothingStore.Domain.ValueObjects
{
    public class EmailVal : ValueOf<string, EmailVal>
    {
        protected override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Value))
                return;

            if (!Value.Contains("@"))
                throw new ArgumentException("Email must contain '@'");

            if (Value.Length > 255)
                throw new ArgumentException("Email is too long");
        }
    }
}
