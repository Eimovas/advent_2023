import re 

def read_lines(path: str) -> list[str]:
    with open(path) as file:
        return [ line.strip() for line in file ]
    
def parse_line(line: str) -> tuple[str, list[int]]:
    split_line = re.split(' ', line)
    split_counts = list(map(lambda x: int(x) , re.split(',', split_line[1])))
    return split_line[0], split_counts

def solve_line(row, counts):
    # take a count, and try to match it with the chars;
    # some observations:
    # - if a count is surrounded by '.' or '?', its a hit,
    # - 


    # for i, char in enumerate(row):
    return "not implemented"

# print(parse_line("???.### 1,1,3"))

# TODO: FINISHED HERE. Continue thinking :shrug: