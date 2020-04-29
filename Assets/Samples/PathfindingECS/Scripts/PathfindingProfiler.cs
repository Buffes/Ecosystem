using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class PathfindingProfiler : MonoBehaviour
{
    public static readonly ProfilerMarker ProfilerMarker = new ProfilerMarker("Pathfinding");

    [SerializeField]
    private int frameRange = 60;

    [SerializeField]
    private Text textTimeAverage = default;
    [SerializeField]
    private Text textTimeHighest = default;

    private float[] buffer;
    private int bufferIndex;
    private Recorder pathfindingRecorder;

    private float averageTime;
    private float highestTime;
    private float lowestTime;

    void Start()
    {
        pathfindingRecorder = Recorder.Get("Pathfinding");
        pathfindingRecorder.enabled = true;
    }

    void Update()
    {
        if (buffer == null || buffer.Length != frameRange)
        {
            InitializeBuffer();
        }

        UpdateBuffer();
        CalculateExecutionTime();

        if (textTimeAverage != null) textTimeAverage.text = "Average (ms/frame): " + averageTime;
        if (textTimeHighest != null) textTimeHighest.text = "Highest (ms/frame): " + highestTime;
    }

    private void InitializeBuffer()
    {
        if (frameRange <= 0)
        {
            frameRange = 1;
        }
        buffer = new float[frameRange];
        bufferIndex = 0;
    }

    void UpdateBuffer()
    {
        if (!pathfindingRecorder.isValid) return;

        buffer[bufferIndex++] = pathfindingRecorder.elapsedNanoseconds / 1000000;
        if (bufferIndex >= frameRange)
        {
            bufferIndex = 0;
        }
    }

    void CalculateExecutionTime()
    {
        float sum = 0;
        float highest = 0;
        float lowest = float.MaxValue;
        for (int i = 0; i < frameRange; i++)
        {
            float time = buffer[i];
            sum += time;
            if (time > highest)
            {
                highest = time;
            }
            if (time < lowest)
            {
                lowest = time;
            }
        }
        averageTime = sum / frameRange;
        highestTime = highest;
        lowestTime = lowest;
    }
}
