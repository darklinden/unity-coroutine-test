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

                var tick = DateTime.UtcNow.Ticks;
                UnityEngine.Debug.Log("Start Regenerate FlowField");
                yield return flowField.Generate(stopwatch, true);
                var tick2 = DateTime.UtcNow.Ticks;
                UnityEngine.Debug.Log("End Regenerate FlowField " + (tick2 - tick) / 10000f);

                tick = tick2;
                UnityEngine.Debug.Log("Start Regenerate FlowField No Yield");
                yield return flowField.Generate(stopwatch, false);
                tick2 = DateTime.UtcNow.Ticks;
                UnityEngine.Debug.Log("End Regenerate FlowField No Yield " + (tick2 - tick) / 10000f);

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
