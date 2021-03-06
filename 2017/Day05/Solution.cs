﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day05 {

    class Solution : Solver {

        public string GetName() => "A Maze of Twisty Trampolines, All Alike"; 
        
        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => GetStepCount(input, x => x + 1);

        int PartTwo(string input) => GetStepCount(input, x => x < 3 ? x + 1 : x - 1);

        int GetStepCount(string input, Func<int, int> update) {
            var numbers = input.Split('\n').Select(int.Parse).ToArray();
            var i = 0;
            var stepCount = 0;
            while (i < numbers.Length && i >= 0) {
                var jmp = numbers[i];
                numbers[i] = update(numbers[i]);
                i += jmp;
                stepCount++;
            }
            return stepCount;
        }
    }
}
