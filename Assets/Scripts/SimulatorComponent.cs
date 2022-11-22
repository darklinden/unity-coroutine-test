using System.Diagnostics;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class SimulatorComponent : MonoBehaviour
{
    private Simulator simulator;
    public Simulator GetSimulator()
    {
        if (simulator == null && Application.isPlaying)
        {
            simulator = new Simulator();
        }
        return simulator;
    }

    private Coroutine m_SimulatorCoroutine;
    void OnEnable()
    {
        m_SimulatorCoroutine = StartCoroutine(RoutineRegenerateFlowField());
    }

    void OnDisable()
    {
        if (m_SimulatorCoroutine != null)
        {
            StopCoroutine(m_SimulatorCoroutine);
            m_SimulatorCoroutine = null;
        }
    }

    private bool m_IsCalculating = false;
    private float m_LastRefreshTime = 0f;
    private IEnumerator RoutineRegenerateFlowField()
    {
        Simulator flowField = GetSimulator();
        Stopwatch stopwatch = new Stopwatch();

        while (true)
        {
            if (Time.time - m_LastRefreshTime > 1)
            {
                m_IsCalculating = true;

                yield return flowField.Generate(stopwatch, true);
                // var gen = flowField.Generate(stopwatch, true);
                // while (gen.MoveNext())
                // {
                //     yield return gen.Current;
                // }

                m_IsCalculating = false;

                m_LastRefreshTime = Time.time;
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (simulator != null)
        {
            simulator.Dispose();
            simulator = null;
        }
    }
}
