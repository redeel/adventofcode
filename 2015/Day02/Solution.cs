using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day02 {

    class Solution : Solver {

        public string GetName() => "I Was Told There Would Be No Math";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => (
                from nums in Parse(input) 
                select 2 * (nums[0] * nums[1] + nums[1] * nums[2] + nums[0] * nums[2]) + nums[0] * nums[1]
            ).Sum();

        int PartTwo(string input) => (
                from nums in Parse(input) 
                select nums[0] * nums[1] * nums[2] + 2 * (nums[0] + nums[1])
            ).Sum();

        IEnumerable<int[]> Parse(string input) {
            return (from line in input.Split('\n')
                    let parts = line.Split('x')
                    let nums = parts.Select(int.Parse).OrderBy(x => x).ToArray()
                    select nums);
        }
    }
}