const std = @import("std");

pub fn main() !void {
    parse_game("what the hell");
}

pub fn parse_game(line: []const u8) void {
    var split = std.mem.split(u8, line, " ");
    while (split.next()) |x| {
        std.debug.print("word: {s}\n", .{x});
    }

    // TODO: FINISHED HERE!
}
