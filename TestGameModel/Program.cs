﻿using SlotGame.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestGameModel
{
    class Lol
    {
        public int Mult;

        public Lol(int mult)
        {
            this.Mult = mult;
        }
        public bool Check()
        {
            return Mult == 5;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            
            //List<Lol> list = new List<Lol>() {
            //    new Lol(1),
            //    new Lol(5),
            //    new Lol(10),
            //    new Lol(-5),
            //    new Lol(3),
            //    new Lol(2),
            //    new Lol(6),
            //    new Lol(1)
            //};
            //Console.WriteLine(list.OrderByDescending(v=>v.Mult).ThenBy(v=>Guid.NewGuid()).First(v=>v.Check()).Mult);
            //Console.WriteLine(list.OrderByDescending(v => v.Mult).ThenBy(v => Guid.NewGuid()).ToList().First(v=>v.Check()).Mult);
            //return;
            int spinCount = 100000;
            int cashCount = 100000;
            Cash bet = new Cash(Currency.EUR, 100);
            Console.WriteLine($"SpinCount: {spinCount}");
            Console.WriteLine($"CashCount: {cashCount}");
            Console.WriteLine($"Bet: {bet}");

            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            //int taskCount = 100;
            //Task<decimal>[] tasks = new Task<decimal>[taskCount];
            //for (int t = 0; t < taskCount; t++)
            //{
            //    tasks[t] = Task<decimal>.Factory.StartNew(() => SimulateGame(new Cash(Currency.EUR, cashCount), spinCount, bet));
            //}

            //Task.WaitAll(tasks);
            //watch.Stop();
            //Console.WriteLine("time: " + watch.ElapsedMilliseconds);

            //decimal[] results = tasks.Select(t => t.Result).ToArray();
            //Console.WriteLine("Average: " + results.Average());
            SimulateGame(new Cash(Currency.EUR, cashCount), spinCount, bet);
            Console.WriteLine("end");
            Console.ReadKey();
            // list 108590
        }

        public static decimal SimulateGame(Cash cash, int spinCount, Cash bet)
        {
            HappySlotGame game = new HappySlotGame(cash);

            Dictionary<WinResponse, int> winningInfo = new Dictionary<WinResponse, int>();
            decimal i;
            decimal max = game.Cash.Count;
            decimal min = game.Cash.Count;
            for (i = 0; i < spinCount; i++)
            {
                var wintype = game.Spin(bet);

                if (game.Cash.Count > max)
                    max = game.Cash.Count;
                if (game.Cash.Count < min)
                    min = game.Cash.Count;


                winningInfo[wintype] = winningInfo.ContainsKey(wintype) ? winningInfo[wintype] + 1 : 1;

                //if (game.Cash.Count < 0)
                //    break;

                if (i % (spinCount / 100) == 0)
                    Console.WriteLine(i / (spinCount / 100));
            }

            string resultInfo = String.Empty;
            resultInfo += $"ThreadID: {Thread.CurrentThread.ManagedThreadId}\n" +
                                $"TaskID: {Task.CurrentId ?? 0}\n";
            decimal fullWin = 0;

            foreach (var wininfo in winningInfo.OrderByDescending(wi => (wi.Value * wi.Key.Multiplier) / (i * bet.Count)))
            {
                var name = wininfo.Key.Name;
                decimal count = wininfo.Value;
                decimal coef = wininfo.Key.Multiplier;
                fullWin += count * coef * bet.Count;
                resultInfo += $"{Thread.CurrentThread.ManagedThreadId}|{Task.CurrentId ?? 0}" +
                                $"| {name,-40}: {count,-6} | {wininfo.Key.Multiplier,-4}" +
                                $"| Probability: {count / i:P5} " +
                                $"| return: {count * coef * bet.Count / (i * bet.Count):P5}\n";
            }

            resultInfo += $"{Thread.CurrentThread.ManagedThreadId}|{Task.CurrentId ?? 0}| All return: {fullWin / (i * bet.Count):P5}\n";

            Console.WriteLine(resultInfo);
            Console.WriteLine($"Max: {max}");
            Console.WriteLine($"Min: {min}");
            Console.WriteLine($"Current: {game.Cash.Count}");
            return fullWin / (i * bet.Count);
        }

    }
}
