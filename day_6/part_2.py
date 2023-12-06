import re 
from part_1 import read_lines,process_game

def parse(lines):
    times = list(filter(lambda x: x != '', re.split(' |:', lines[0])))[1:]
    time = ""
    for x in times:
        time += x 

    distances = list(filter(lambda x: x != '', re.split(' |:', lines[1])))[1:]
    distance = ""
    for x in distances:
        distance += x

    return int(time), int(distance)

time,distance = parse(read_lines('input'))
print(process_game(time,distance))