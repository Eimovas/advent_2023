import numpy as np 

def read_input(path):
    with open(path) as file:
        return [ line.strip() for line in file.readlines() ]

def build_grid(lines):
    result = []
    for line in lines:
        row = []
        for char in line:
            row.append(char)
        result.append(row)
    return np.array(result) 

def expand_universe(grid):
    y = 0
    while y < len(grid):
        if all([ char == '.' for char in grid[y]]):
            grid = np.insert(grid, y + 1, grid[y].copy(), axis=0)
            y += 2
        else:
            y += 1

    x = 0
    while x < len(grid[0]):
        if all([char == '.' for char in grid[:,x]]):
            grid = np.insert(grid, x + 1, grid[:,x].copy(), axis=1)
            x += 2
        else:
            x += 1

    return grid 

def get_galaxy_coordinate_map(grid):
    coordinates = [ (y,x) for y,row in enumerate(grid) for x, char in enumerate(row) if char == '#' ]
    map = {}
    for idx, coordinate in enumerate(coordinates):
        map[idx] = coordinate

    return map

def get_shortest_path(first_coordinates, second_coordinates):
    first_y, first_x = first_coordinates
    second_y, second_x = second_coordinates
    steps = 0

    # adjusting to always move the same direction - seems easier to manage
    start_y, finish_y = (first_y, second_y) if first_y < second_y else (second_y, first_y)
    start_x, finish_x = (first_x, second_x) if first_x < second_x else (second_x, first_x)
    
    while start_y < finish_y:
        start_y += 1
        steps += 1 

    while start_x < finish_x:
        start_x += 1
        steps += 1

    return steps 

def get_pair_count(n):
    return n * (n - 1) / 2

def get_all_pairs(n):
    return [ (x,y) for x in range(0,n) for y in range(x,n) if x != y ]

def get_paired_coordinates(paired_indexes, coordinate_map):
    return [ (coordinate_map[first], coordinate_map[second]) for (first,second) in paired_indexes ]

# compose all together
# grid = expand_universe(build_grid(read_input('input')))
# coordinate_map = get_galaxy_coordinate_map(grid)
# paired_indexes = get_all_pairs(len(coordinate_map))
# paired_coordinates = get_paired_coordinates(paired_indexes, coordinate_map)
# distances = [ get_shortest_path(first,second) for (first,second) in paired_coordinates ]
# print("Distances: ", distances)
# print("Sum: ", sum(distances))

# print(all([ x == '.' for x in grid[3]]))
# print(get_shortest_path((0,4), (1,9)))