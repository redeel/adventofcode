using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode {

    class App {

        static void Main(string[] args) {

            var tsolvers = Assembly.GetEntryAssembly().GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
                .OrderBy(t => t.FullName)
                .ToArray();

            var action =
                Command(args, Args("update", "([0-9]+)/([0-9]+)"), m => {
                    var year = int.Parse(m[1]);
                    var day = int.Parse(m[2]);
                    return () => new Updater().Update(year, day).Wait();
                }) ??
                 Command(args, Args("([0-9]+)/([0-9]+)"), m => {
                    var year = int.Parse(m[0]);
                    var day = int.Parse(m[1]);
                    var tsolversSelected = tsolvers.First(tsolver => 
                        SolverExtensions.Year(tsolver) == year && 
                        SolverExtensions.Day(tsolver) == day);
                    return () => Runner.RunAll(tsolversSelected);
                }) ??
                 Command(args, Args("[0-9]+"), m => {
                    var year = int.Parse(m[0]);
                    var tsolversSelected = tsolvers.Where(tsolver => 
                        SolverExtensions.Year(tsolver) == year);
                    return () => Runner.RunAll(tsolversSelected.ToArray());
                }) ??
                Command(args, Args("([0-9]+)/last"), m => {
                    var year = int.Parse(m[0]);
                    var tsolversSelected = tsolvers.Last(tsolver =>
                        SolverExtensions.Year(tsolver) == year);
                    return () => Runner.RunAll(tsolversSelected);
                }) ??
                Command(args, Args("([0-9]+)/all"), m => {
                    var year = int.Parse(m[0]);
                    var tsolversSelected = tsolvers.Where(tsolver =>
                        SolverExtensions.Year(tsolver) == year);
                    return () => Runner.RunAll(tsolversSelected.ToArray());
                }) ??
                Command(args, Args("all"), m => {
                    return () => Runner.RunAll(tsolvers);
                }) ??
                Command(args, Args("last"),  m => {
                    var tsolversSelected = tsolvers.Last();
                    return () => Runner.RunAll(tsolversSelected);
                }) ??
                new Action(() => {
                    Console.WriteLine(Usage.Get());
                });

            action();
        }

        static Action Command(string[] args, string[] regexes, Func<string[], Action> parse) {
            if (args.Length != regexes.Length) {
                return null;
            }
            var matches = Enumerable.Zip(args, regexes, (arg, regex) => new Regex("^" + regex + "$").Match(arg));
            if (!matches.All(match => match.Success)) {
                return null;
            }
            try {
                
                return parse(matches.SelectMany(m => m.Groups.Count > 1 ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value) : new []{m.Value}).ToArray());
            } catch {
                return null;
            }
        }

        static string[] Args(params string[] regex) {
            return regex;
        }
        
    }

    public class Usage {
        public static string Get(){
            return $@"
               > Usage: dotnet run [arguments]
               > Supported arguments:

               >  [year]/[day|last|all] Solve the specified problems
               >  [year]                Solve the whole year
               >  last                  Solve the last problem
               >  all                   Solve everything

               > To start working on new problems:
               
               >  update [year]/[day]   Prepares a folder for the given day, updates the input, 
               >                        the readme and creates a solution template.
               > ".StripMargin("> ");
        }
    }
}