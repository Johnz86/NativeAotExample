using System;
using System.Diagnostics;

var input = int.Parse(args[0]);

var sw = Stopwatch.StartNew();

var number = FindPrimeNumber(input);
Console.WriteLine($"Found {number} in {sw.ElapsedMilliseconds}ms");
sw.Stop();
long memoryAfter = GC.GetTotalMemory(true);
Console.WriteLine("Memory used: {0} bytes", memoryAfter);
Console.WriteLine("Press any key to exit");
Console.ReadKey();

long FindPrimeNumber(int n) {
    int count = 0;
    long a = 2;
    long memoryBefore = GC.GetTotalMemory(true);
    while (count < n) {
        long b = 2;
        int prime = 1;
        while (b * b <= a) {
            if (a % b == 0) {
                prime = 0;
                break;
            }
            b++;
        }
        if (prime > 0) {
            count++;
        }
        a++;
    }
    return (--a);
}