import re 

def read_lines(path):
    with open(path) as file:
        return [line.rstrip() for line in file]

def parse_game(line):
    split = list(filter(lambda x: x != " ", re.split(':|\|', line)))
    return set(re.split(' ', split[1])), set(re.split(' ', split[2]))

def get_card_value(winning, ours):
    intersection = winning.intersection(ours)

    # tiny bit functional with a fold
    f = lambda acc, x: 1 if acc == 0 else acc * 2 
    acc = 0; [acc := f(acc,x) for x in filter(lambda x: x != "", intersection) ]

    # card_result = 0
    # for num in intersection:
    #     if num == "":
    #         continue

    #     if card_result == 0:
    #         card_result = 1
    #         continue

    #     card_result *= 2
    
    return acc

result = 0
for line in read_lines('input'):
    winning,ours = parse_game(line)
    result += get_card_value(winning,ours)

print(result)