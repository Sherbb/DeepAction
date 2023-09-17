using System;

namespace DeepAction
{
    /// <summary>
    /// Use to determine how the entity is being used. For example certain behaviors might want to only affector actors, etc.
    /// </summary>

    //* NEW TYPE CHECKLIST
    //1 Add to D_EntityType
    //2 Add to D_EntityTypeSelector, add to filters if relevant
    //3 Add conversion in D_EntityTypeSelectorExtensions switch

    public enum D_EntityType
    {
        Actor,//a player or enemy,
        Projectile,
        Structure,//generally stationary entities.
    }

    /// <summary>
    /// A Flagged version of D_EntityType you can use to define groups of types nicely. Check against with the HasEntityType() method.
    /// </summary>

    [Flags]
    public enum D_EntityTypeSelector
    {
        None = 0,
        All = ~0,


        Actor = 1 << 0,
        Projectile = 1 << 1,
        Structure = 1 << 2,
    }

    public static class D_EntityTypeSelectorExensions
    {
        public static D_EntityTypeSelector ConvertToSelector(this D_EntityType type)
        {
            switch (type)
            {
                case D_EntityType.Actor:
                    return D_EntityTypeSelector.Actor;
                case D_EntityType.Projectile:
                    return D_EntityTypeSelector.Projectile;
                case D_EntityType.Structure:
                    return D_EntityTypeSelector.Structure;
            }
            return D_EntityTypeSelector.None;
        }

        public static bool HasEntityType(this D_EntityTypeSelector selector, D_EntityType type)
        {
            return selector.HasFlag(type.ConvertToSelector());
        }
    }
}
