def read_lines(path):
    with open(path) as file:
        return [line.rstrip() for line in file]

def extract_digits(input):
    return [ ord(x) - ord('0') for x in input if x.isnumeric()]

def combine_relevant_digits(list):
    if len(list) == 0:
        raise "Not expecting any empty lists here"
    
    return (list[0] * 10) + list[-1]

digits = [ combine_relevant_digits(extract_digits(line)) for line in read_lines('input') ]
print(sum(digits))
