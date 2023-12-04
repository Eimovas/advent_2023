import re 

def read_lines(path):
    with open(path) as file:
        return [line.rstrip() for line in file]
    
def parse_game(line):
    split = list(filter(lambda x: x != " ", re.split(':|\|', line)))
    game_number = list(filter(lambda x: x != "", re.split(' ', split[0])))[1]
    winning = filter(lambda x: x != "", re.split(' ', split[1]))
    ours = filter(lambda x: x != "", re.split(' ', split[2]))
    return int(game_number), set(winning), set(ours)

def get_hit_count(winning_set, our_set):
    return len(winning_set.intersection(our_set))

def process_cards(cards):
    result = {}
    # set all games to have 1 card
    for (game,winning,ours) in cards:
        result[game] = 1   

    for (game,winning,ours) in cards:
        hit_count = get_hit_count(winning, ours)
        previous_hits = result.get(game) # expecting to always have a value

        for i in range(hit_count):
            index = game + i + 1 # moving to game+1
            game_hits = result.get(index) # expecting value to be here always
            result[index] = game_hits + previous_hits

    return result
    
input = read_lines('input')
cards = [ parse_game(x) for x in input ]
print(sum(list(process_cards(cards).values())))   