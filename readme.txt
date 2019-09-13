For terrain generation, the first step is creating a mesh. We used an array to store the vertices, then creating the triangles by connecting these vertices in clockwise order. 
After that, we got a flat grid. Next step is to apply the Diamond Square algorithm to generate a height map,  for which we created a 2D array to store the height of each vertex, 
then using the Diamond Square algorithm to generate the height at each vertex. The last step is mapping the height array to each vertex, 
then the terrain's shape is done.

The next part is terrain colouring. We generate a colour map based on the height map generated in the previous stage. 
When generating the terrain, the maximum and minimum height of the terrain were stored,  which are used here to calculate the height threshold for each region. 
Then, each vertice is assigned a colour depending on which 'region' its height was. This colour map is mapped to mesh.colour in mesh filter.

As for the water, we created a mesh which has the same size as the terrain. The lab4's material was adapted to generate water by applying displacement in the vertex shader.
To create a realistic wave effect, we added some noise to displacement value and give water the effect that moves up and down in an oscillating fashion. 
The height of the water layer was set to the height of the 'sand' region in terrain generator's script. Thus, the water mesh will be initialized at the right height. 