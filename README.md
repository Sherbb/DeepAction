# Status
Right now im using this system in another project with lots of changes. I'll update THIS repo with all the lessons learned from then when im done.

IN OTHER WORDS...THIS REPO IS LIKE 4 MONTHS BEHIND AND COMPLETELY UNFINISHED.



.

.

.

.
  
   
   


# DeepAction
A platform to create ability based action games in Unity.
This HEAVILY relies on ODIN, and can not function without it. 

https://odininspector.com/

THIS IS STIL A WORK IN PROGRESS!

![image](https://user-images.githubusercontent.com/13370191/118415689-de314100-b679-11eb-8341-ad8b1f94cc23.png)

## Structure

### DeepEntity
A <b>DeepEntity</b> has Attributes, Resources, and Behaviors.

DeepEntity is a monobehavior you can put on anything. It will not do anything by itself.

### DeepAttribute
An <b>Attribute</b> is just a float with extra functionality. They can be modified a number of ways.

DeepAttributes can be anything from DamageReduction to EntityMoveSpeed. These don't do anything by themselves, If you want your entity to use EntityMoveSpeed you must refernece it in your movement controller.

### DeepResource
A <b>DeepResource</b> is similair to an attribute, with different scaling behavior. It serves the same purpose of being a value that your other scripts look at to do things.

### DeepBehavior
A <b>DeepBehavior</b> is an object that attaches an entity and executes actions based on that entities state. An ability is a behavior. A status effect is a behavior. A modifer is a behavior. 

The DeepEntity will trigger generic events like OnTakeDamage, OnUpdate, and OnKillEnemy on all behaviors currently attached to the Entity. You can also <b>Trigger</b> a specific behavior on a DeepEntity.

### DeepAction
A <b>DeepAction</b> is a class that holds code that gets triggered by a DeepBehavior. For example a DeepAction can have code that gets triggered when the DeepEntity a behavior is on takes damage, spawns, or just on update.

These are the building blocks that the majority of the gameplay will come from. 

## ...Why?
The purpose of this is to create a really solid base that allows for a ton flexibility and freedom in design. I wanted create a very deep and connected system, without getting overly complicated and convoluted. It's not done yet so we shall see if I will suceed.

## How To Use
Ill explain this when it's closer to done and I have examples.
