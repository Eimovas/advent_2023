from part_1 import read_lines, parse_line

def get_line_score_part2(arr):
    if len(arr) % 2 != 0:
        raise "cannot continue because line length should be % 2 = 0"
    
    red_max = 0
    green_max = 0
    blue_max = 0

    for index in range(len(arr)):
        if index % 2 == 0 and index + 3 <= len(arr):
            count = int(arr[index + 2])
            key = arr[index + 3]

            if key == "red" and count > red_max:
                red_max = count
            elif key == "green" and count > green_max:
                green_max = count
            elif key == "blue" and count > blue_max:
                blue_max = count
    
    return red_max * green_max * blue_max

result = 0
for line in read_lines('input'):
    result += get_line_score_part2(parse_line(line))

print(result)
