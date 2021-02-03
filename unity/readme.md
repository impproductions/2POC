# dots-to-circles
Create a path that resembles a circle passing through n dots.

## Target
I was asked to provide a system for the player to place different objects in the scene, then connect them to create a cicular(ish) mesh to use as an emitter for a generic particle system.

![circle example](./circlepath.png)

## Implementation
- Project all points on the ground
- Define a centroid (average)
- Rotate around the centroid clockwise and draw the shape passing once through each point
- When n points share the same radius, start and end the connection from the closest/furthest from the centroid (to avoid "spikes" in the shape)

Relevant scripts: https://github.com/impproductions/2POC/tree/master/unity/dots-to-circles/Assets/Scripts