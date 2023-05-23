using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public GraphPanel graph;

    float func(float x)
    {
        return x*x*x;
    }

    private void Start()
    {
        graph.graph.function = func;
        graph.graph.CreateGraphWithFunction(-25f, 25f);
        graph.DrawGraph();
    }
}
