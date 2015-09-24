# ProceduralGenerator
Test-bed for some procedural level generation.

Currently following some tutorials at:
https://youtu.be/AsR0-wCTJl8
http://catlikecoding.com/unity/tutorials/marching-squares/

Tutorials are mainly completed. Now to design and implement my own level generator.

TODO:
1. Create a map representation
    - Random map
    - Smooth map
2. Generate mesh for the floor
    - Marching squares algorithm
3. Generate mesh for the walls
    - Determine room outlines (edge sharing a single triangle)
    - Generate wall mesh from this
4. Generate connection between the rooms
