import numpy as np 
from part_1 import build_grid, read_input, get_galaxy_coordinate_map, get_all_pairs, get_paired_coordinates, get_shortest_path

def expand_universe_part_2(grid):

    # This will clearly not work. Meaning I have to re-do the whole thing :(
    y = 0
    while y < len(grid):
        if all([ char == '.' for char in grid[y]]):
            copy = np.tile(grid[y], (1_000_000, 1))
            grid = np.insert(grid, y + 1, copy, axis=0)
            y += 1_000_0001
        else:
            y += 1

    x = 0
    while x < len(grid[0]):
        if all([char == '.' for char in grid[:,x]]):
            copy = np.tile(grid[:,x], (1_000_000, 1))
            grid = np.insert(grid, x + 1, copy, axis=1)
            x += 1_000_001
        else:
            x += 1

    return grid 

# 
grid = expand_universe_part_2(build_grid(read_input('test_input')))
coordinate_map = get_galaxy_coordinate_map(grid)
paired_indexes = get_all_pairs(len(coordinate_map))
paired_coordinates = get_paired_coordinates(paired_indexes, coordinate_map)
distances = [ get_shortest_path(first,second) for (first,second) in paired_coordinates ]
print("Distances: ", distances)
print("Sum: ", sum(distances))
