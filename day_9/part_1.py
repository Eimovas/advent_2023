import re 
from more_itertools import pairwise

def read_input(path):
    with open(path) as file:
        return [ line.rstrip() for line in file ]

def parse(line):
    return list(filter(lambda x: x != "", re.split(' ', line)))

def extrapolate(starting_number, history):
    new_number = starting_number
    for line in history:
        new_number += int(line[-1])

    return new_number

def process_sequence(sequence, history):
    pairwise_list = pairwise(sequence)
    new_arr = list(map(lambda x: int(x[1]) - int(x[0]), pairwise_list))

    if all([ x == 0 for x in new_arr]):
        return extrapolate(0, history)
    else:
        history.append(new_arr)
        return process_sequence(new_arr, history)


input = [parse(line) for line in read_input('input')]
processed = [ process_sequence(line, [line]) for line in input ]
print(sum(processed))
