def read_input(path):
    with open(path) as file:
        return [ line.rstrip() for line in file.readlines()]
    
def parse(lines):
    matrix = []
    for line in lines:
        matrix.append([char for char in line])

    return matrix

def find_start_position(matrix):
    for yIndex, row in enumerate(matrix):
        for xIndex, char in enumerate(row):
            if char == 'S':
                return yIndex,xIndex

def get_first_direction(matrix, starting_position):
    starting_y, starting_x = starting_position
    max_y = len(matrix)
    max_x = len(matrix[0])
    direction_chars = ['|', '-', 'L', 'J', '7', 'F']

    if starting_y + 1 < max_y and matrix[starting_y + 1][starting_x] in direction_chars:
        return starting_y + 1, starting_x
    elif starting_y - 1 >= 0 and matrix[starting_y - 1][starting_x] in direction_chars:
        return starting_y - 1, starting_x
    elif starting_x + 1 < max_x and matrix[starting_y][starting_x + 1] in direction_chars:
        return starting_y, starting_x + 1
    elif starting_x - 1 >= 0 and matrix[starting_y][starting_x - 1] in direction_chars:
        return starting_y, starting_x - 1
    else:
        raise("I'm not expecting to ever get here")

def go_east(y,x):
    return y, x + 1

def go_west(y,x):
    return y, x - 1

def go_north(y,x):
    return y - 1, x

def go_south(y,x):
    return y + 1, x

def get_next_location(matrix, previous_position, current_position):
    current_y, current_x = current_position
    previous_y, previous_x = previous_position
    current_char = matrix[current_y][current_x]

    if current_char == '|': 
        if current_y > previous_y:
            return go_south(current_y, current_x)
        else:
            return go_north(current_y, current_x)
    
    elif current_char == '-':
        if current_x > previous_x: 
            return go_east(current_y, current_x)
        else: 
            return go_west(current_y, current_x)

    elif current_char == 'L':
        if current_y == previous_y:
            return go_north(current_y, current_x)
        else:
            return go_east(current_y, current_x)
        
    elif current_char == 'J':
        if current_y == previous_y:
            return go_north(current_y, current_x)
        else:
            return go_west(current_y, current_x)
        
    elif current_char == 'F':
        if current_y == previous_y:
            return go_south(current_y, current_x)
        else:
            return go_east(current_y, current_x)
    
    elif current_char == '7':
        if current_y == previous_y:
            return go_south(current_y, current_x)
        else:
            return go_west(current_y, current_x)
    
    else:
        raise("I've arrived at a very wrong location")

def traverse_back(matrix, starting_position):
    current_position = get_first_direction(matrix, starting_position)
    previous_position = starting_position
    hops_made = 1

    while matrix[current_position[0]][current_position[1]] != 'S':
        next_y, next_x = get_next_location(matrix, previous_position, current_position)

        hops_made += 1 # increment and continue
        previous_position = current_position
        current_position = (next_y, next_x)
    
    return hops_made

parsed = parse(read_input('input')) 
start = find_start_position(parsed)
print(traverse_back(parsed, start) / 2)
