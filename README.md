# DeepAction
A platform to create complex rpg/moba like action games in Unity.

THIS IS STIL A WORK IN PROGRESS!

![image](https://user-images.githubusercontent.com/13370191/118415689-de314100-b679-11eb-8341-ad8b1f94cc23.png)

## Structure

### DeepEntity
A <b>DeepEntity</b> has Attributes, Resources, and Behaviors.

This is the core gameplay object. 

### DeepBehavior
A <b>DeepBehavior</b> is an object that attaches to an entity and executes code based on that entities state. 
An ability is a behavior. A status effect is a behavior. A modifer is a behavior. 

The DeepEntity will trigger generic events like OnTakeDamage, OnUpdate, and OnKillEnemy on all behaviors currently attached to the Entity.

## ...Why?
The purpose of this is to create a really solid base that allows for a ton flexibility and freedom in design. I wanted create a very deep and connected system, without getting overly complicated and convoluted. It's not done yet so we shall see if I will suceed.

## How To Use
Ill explain this when it's closer to done and I have examples.
