import re 

def read_lines(path):
    with open(path) as file:
        return [line.rstrip() for line in file]

def parse(lines):
    times = filter(lambda x: x != '', re.split(' |:', lines[0]))
    distances = filter(lambda x: x != '', re.split(' |:', lines[1]))
    return list(zip(times, distances))[1:]
    
def process_game(time,max_distance):
    wins = []
    for current in range(1, max_distance):
        if current == time:
            return len(wins)
        else:
            potentialTime = current * (time - current)
            if potentialTime > max_distance:
                wins.append(current)

input = read_lines('input')
result = 1
parsed = parse(input)
for (time,distance) in parsed:
    result *= process_game(int(time), int(distance))

print(result)