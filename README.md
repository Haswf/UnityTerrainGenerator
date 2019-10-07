# COMP30019 2019S2 Project 2

A simple procedural terrain generator based on Diamond Square algorithm in Unity.

![Demo](https://i.loli.net/2019/09/17/DKISmoJcWVLswOn.png)



## Project Specification

### Modelling of fractal landscape

- You must automatically generate a randomly seeded fractal landscape at each invocation
  of the program, via a correct implementation of the diamond-square algorithm.
- You must use Unity’s architecture appropriately to generate and render the landscape.
- There must be no notable problems or artifacts with the polygonal representation.
- The default parameters of the Diamond-Square algorithm should be set appropriately to
  model a realistic looking landscape.
- The landscape should be generated in a timely manner (i.e. several seconds maximum on
  the lab computers).

## Camera motion

- Moving the mouse should control the relative pitch and yaw of the camera

  + The ’w’ and ’s’ keys should cause the camera to move forwards and backwards
    respectively, relative the camera’s current orientation

  + The ’a’ and ’d’ keys should cause the camera to move left and right respectively,
    relative to the camera’s current orientation

- Allow the user to move anywhere in the world (including up into the sky), and
    prohibit the user from ever moving or seeing “underground” or outside the bounds of
    the landscape.

-  The camera must not become ‘stuck’ upon nearing or impacting the terrain, i.e. reversing
    and continuing to move must always be possible.

- You must utilise perspective projection, and choose a suitable default perspective, so that the landscape is always clearly visible from the first frame of the simulation (irrespective of the initial mouse position).

### Surface properties

- The colour of the terrain must correspond in a sensible way with the height of the terrain
  at any particular point (for example rocky outcrops or snow on top of mountains and
  grass or soil in valleys).
- Realistic lighting must be present based on the Phong illumination model (diffuse, specular
  and ambient components).
- The direction of the lighting must change with time, to simulate the effect of a sun rising
  and setting.
- The sun itself must also be rendered, in order to help verify the correctness of your
  lighting implementation. You may use any simple geometric shape such as a pyramid,
  cube or sphere to represent the sun.
- Water sections must be present. You may use a plane passing through the terrain to
  achieve this. A custom Cg/HLSL vertex shader should be used to create realistic waves
  via displacement of vertices within the plane. Ensure the plane has enough vertices for
  the effect to be sufficiently detailed.

# Feedback

Thank you for your submission, this is a good implementation with realistic generation of the terrain. There are some problems in the edge conditions of your DS as you are ignoring cases with 3 vertex. Also regarding the camera you dont have a correct mesh Collider so the camera cannot get close to the terrain, and generally the camera speed is quite slow. Unfortunately, there are also problems in the generation of the normals for the water shader. I like the details on the generated terrain. Good job! 



Please be kindly advised that we have no plan to resolve these issues in near future. 

## Contributor

[Haswf](https://github.com/Haswf)

[hanx7](https://github.com/hanx7)

[tomtom99113](https://github.com/tomtom99113)

