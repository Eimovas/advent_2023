import re 

red_max = 12
green_max = 13
blue_max = 14

def read_lines(path):
    with open(path) as file:
        return [line.rstrip() for line in file]

def parse_line(line):
    return list(filter(lambda x: x != "", re.split(' |:|,|;', line)))

def get_line_result(arr):
    if len(arr) % 2 != 0:
        raise "cannot continue because line length should be % 2 = 0"
    
    game_id = int(arr[1])

    for index in range(len(arr)):
        if index % 2 == 0 and index + 3 <= len(arr):
            count = int(arr[index + 2])
            key = arr[index + 3]

            if key == "red" and count > red_max:
                return 0
            elif key == "green" and count > green_max:
                return 0
            elif key == "blue" and count > blue_max:
                return 0
    
    return game_id

result = 0
for line in read_lines('input'):
    result += get_line_result(parse_line(line))

print(result)
