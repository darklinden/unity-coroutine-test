using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Profiling;
using System.Diagnostics;

public class Simulator : IDisposable
{
    private int RoutineTimeFrame = 10;
    private int RoutineCheckCount = 200;

    public void Dispose()
    {

    }

    // Customize the counter to prevent the stopwatch from getting timestamps too often
    bool StopwatchIncreaseChecker(Stopwatch stopwatch, ref int counter)
    {
        if (counter++ > RoutineCheckCount)
        {
            counter = 0;
            if (stopwatch.ElapsedMilliseconds > RoutineTimeFrame)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator Generate(Stopwatch stopwatch, bool yieldOnSpendTimeTooLong)
    {
        // Profiler.BeginSample("Simulator.Generate => Test");
        int counter = 0;

        // init stopwatch checker
        if (yieldOnSpendTimeTooLong)
        {
            stopwatch.Reset();
            stopwatch.Start();
        }

        for (int i = 0; i < int.MaxValue; i++)
        {
            // if stopwatch spend too long, yield return
            if (yieldOnSpendTimeTooLong && StopwatchIncreaseChecker(stopwatch, ref counter))
            {
                yield return null;
                stopwatch.Reset();
                stopwatch.Start();
            }
        }

        // Profiler.EndSample();
    }
}

