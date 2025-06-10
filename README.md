# Hands of the Hive

## Primary Focus

For this project, I took inspiration from the boids algoirthm to implement swarming bees. Through a lot of trial, error and research, I was able to implement a simple architecture for creating dynamic AI movement handlers, more formally: Steering Behaviors. Lower level Steering Behaviors could be composed within higher level ones to create more sophisticated and focused AI movement. I wrote four high level Steering Behaviors: *BoidsBehavior, GoThereBehavior, FlockingBehavior* and *BuzzingBehavior*; and a large number of second and first level Steering Behaviors, and attached them to game objects through a brain-body inspired decision-making/movement framework. 

### Tools and Additional Features

To support the PathFollowing behavior, I wrote an A* Pathfinding algorithm, which which took in as input a pre-computed navigation mesh. In addition to this, to support myself in debugging and give a visual indication for the behavior being executed at any given time, I integrated a graph-shader into Unity's rendering pipieline via their new Rendering Graph API. I also used that same API to insert a blur shader into the rendering pipeline. Lastly, I built a maze to present the features of the demo and, again, help with debugging.

### What's next

After writing more than a handful of level two and level three behaviors, I noticed a trend between the way Steering Behaviors were composed on any given level. Level Two behaviors either directly composed with a level one behavior, and delegated everything but targeting to the level one behavior, inherited from *BlendedSteering* and composed with a behavior to handle rotation, or inherited from *PrioritySteering* and again composed with another behavior to handle rotation. Level three behaviors just built up on this framework in order to create more sophisticated motion, and thus composed with the level two behaviors by inheriting from the two classes listed above and instatiating level two behaviors within their groups variable. 

Using these cases as a simple starting point, I could write a Unity Editor Tool for easiy creating and attaching Steering Behaviors to Game Objects. There would still need to be a higher level system to handle mathcing AI decision to Behaviors, but this could also be left to the developer after creating their own mix and match steering behaviors within the editor. 
