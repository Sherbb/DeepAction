
```
WORK IN PROGRESS
```
```
Requires Unity 2021.3+
```


# DeepAction
A platform to create complex action games in Unity.

Entities behavior is defined entirely in c#
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

## DeepEntity

The core gameplay entity has Behaviors, Attributes, Flags, and Resources

### DeepBehaviors

A behavior is the replacement for MonoBehaviors that gets attached to entities. It has a Initialize/Destroy method that you can use to hook into the parent entities events.

These events are things like: `Update`,`OnEnable`,`OnCollisionStay`. But also things like: `OnDealDamage`,`OnBounce`,`OnCast`.

```csharp
public class AreaDamageOnDeath : DeepBehavior
{
    private float radius;
    private Damage damage;
    private D_Team targetTeam;

    public AreaDamageOnDeath(float radius, Damage damage, D_Team targetTeam)
    {
        this.radius = radius;
        this.damage = damage;
        this.targetTeam = targetTeam;
    }

    public override void InitializeBehavior()
    {
        parent.events.OnEntityDie += OnDie;
    }

    public override void DestroyBehavior()
    {
        parent.events.OnEntityDie -= OnDie;
    }

    private void OnDie()
    {
        DeepActions.AreaDamage(parent.transform.position, radius, damage, targetTeam);
    }
}
```

> TODO: explain attributes

> TODO: explain resources

## Goal
The purpose of this is to create a really solid base that allows for a ton flexibility and freedom in design.
Beyond freedom, a major goal is creating a system that is very FAST to work with.

I find that doing a system like this through mono-behaviors is very messy and error prone (and a little less performant).

Defining entity behaviors inside c# is both very fast and EXTREMLY flexible. Creating this kind of flexibility in an editor GUI is possible, but a huge waste of time imo.
