> Requires Unity 2021.3+

> WIP not production ready with multiple dead ends currently.

# DeepAction
A platform to create complex rpg/moba like action games in Unity.

All entities behavior and composition is defined in c#
```csharp
public static EntityTemplate ExampleEnemy()
{
    EntityTemplate t = BaseEntity();

    t.attributes[D_Attribute.MoveSpeed] = new A(Random.Range(20f, 40f));
    t.attributes[D_Attribute.MaxMoveSpeed] = new A(Random.Range(20f, 40f));

    t.behaviors = new DeepBehavior[]{
        new MoveTowardsPlayer(),
        new AvoidOtherEntities(D_Team.Enemy,D_EntityType.Actor,60f),
        new VFXOnDeath(
            new VFX.Sparks(new Color(1f,.256f,.256f),5),
            new VFX.SquarePop(new Color(1f,.256f,.256f),5f,.2f)
        ),
    };

    t.team = D_Team.Enemy;
    t.type = D_EntityType.Actor;

    return t;
}
```

Its basically just an entity component system, with some supporting systems I like. Global AppState, VFX, etc.

## Structure

### DeepEntity
A <b>DeepEntity</b> has Attributes, Resources, Flags, and Behaviors.

This is the core gameplay object. 

### DeepBehavior
A <b>DeepBehavior</b> is an object that attaches to an entity and executes code based on that entities state. 
Functions like a monoBehavior.

The DeepEntity will trigger generic events like OnTakeDamage, OnUpdate, and OnKillEnemy on all behaviors currently attached to the Entity.

> Todo: actually explain this properly ^ LOL

## Goal
The purpose of this is to create a really solid base that allows for a ton flexibility and freedom in design.
Beyond freedom, a major goal is creating a system that is very FAST to work with.

I find that doing a system like this through mono-behaviors is very messy and error prone (and a little less performant).

Defining entity behaviors inside c# is both very fast and EXTREMLY flexible. Creating this kind of flexibility in an editor GUI is possible, but a huge waste of time imo.

## Examples

> TODO

## How To Use

> TODO
