﻿using System;
using System.Threading.Tasks;
using System.Reactive.Linq;
using QbservableProvider.Common;

namespace QbservableProvider.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO: Support anonymous types for this query
            //new StreamDbContext("https://localhost:5001")
            //    .FromAll()
            //    .GroupBy(e => e.Category)
            //    .SelectMany(x => x.Scan(new { Count = 0, Category = "" }, (s, e) => new { Count = s.Count + 1, e.Category }))
            //    .Subscribe(s => Console.WriteLine(s.Category + ": " + s.Count));

            // TODO: Support anonymous types for this query
            //(
            //    from e1 in new StreamDbContext("https://localhost:5001").FromAll()
            //    from e2 in new StreamDbContext("https://localhost:5001").FromAll()
            //    where e1.Category == e1.Category
            //    select e1.Id + " - " + e2.Id
            //).Subscribe(e => Console.WriteLine(e));

            new StreamDbContext("https://localhost:5001")
                .FromAll()
                .GroupBy(e => e.Category)
                .SelectMany(g =>
                    g.Scan(
                        $"{g.Key}:0",
                        (s, e) => $"{e.Category}:{int.Parse(s.Split(':', StringSplitOptions.None)[1]) + 1}"
                    )
                )
                .Subscribe(
                    onNext: s => Console.WriteLine(s),
                    onError: e => Console.WriteLine("ERROR: " + e),
                    onCompleted: () => Console.WriteLine("DONE")
                );

            while (!Console.KeyAvailable && Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                await Task.Delay(50);
            }
        }
    }
}
