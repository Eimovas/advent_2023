import sys

def argmin(a):
    return min(range(len(a)), key=lambda x : a[x])
def argmax(a):
    return max(range(len(a)), key=lambda x : a[x])
def read_lines(path):
    with open(path) as file:
        return [line.rstrip() for line in file]

str_nums = ['zero', 'one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine']
nums = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']
total = 0

for line in read_lines('input'):
    # print(line)
    first_num_idxs = [line.find(x) for x in nums]
    last_num_idxs = [line.rfind(x) for x in nums]
    first_str_idxs = [line.find(x) for x in str_nums]
    last_str_idxs = [line.rfind(x) for x in str_nums]

    first_num_idxs[:] = [x if x != -1 else len(line) for x in first_num_idxs]
    first_str_idxs[:] = [x if x != -1 else len(line) for x in first_str_idxs]

    if min(first_num_idxs) < min(first_str_idxs):
        tens = argmin(first_num_idxs)
    else:
        tens = argmin(first_str_idxs)

    if max(last_num_idxs) > max(last_str_idxs):
        ones = argmax(last_num_idxs)
    else:
        ones = argmax(last_str_idxs)
    t = tens*10 + ones
    print(tens, ones, t)
    total += t
    
print(total)