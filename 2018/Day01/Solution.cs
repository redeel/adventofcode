using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day01 {

    class Solution : Solver {

        public string GetName() => "Chronal Calibration";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            return Frequencies(input).ElementAt(input.Split("\n").Count() - 1);
        }

        int PartTwo(string input) {
            var seen = new HashSet<int>();
            foreach (var f in Frequencies(input)) {
                if (seen.Contains(f)) {
                    return f;
                }
                seen.Add(f);
            }
            throw new Exception();
        }

        IEnumerable<int> Frequencies(string input) {
            var f = 0;
            while (true) {
                foreach (var d in input.Split("\n").Select(int.Parse)) {
                    f += d;
                    yield return f;
                }
            }
        }
    }
}