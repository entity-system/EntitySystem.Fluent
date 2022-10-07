using EntitySystem.Shared.Domain;

namespace EntitySystem.Shared.Fluent.Domain
{
    public class Entity : IEntity
    {
        public virtual long Id { get; set; }

        public override bool Equals(object @object)
        {
            return @object is Entity entity && GetType().IsInstanceOfType(@object) && Id.Equals(entity.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
