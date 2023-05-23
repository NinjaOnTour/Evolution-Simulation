using UnityEngine;

public delegate float GraphFunction(float x);

[System.Serializable]
public class Graph
{
    public float[] xValues;
    public float[] yValues;
    public float Grid;
    public string xTitle;
    public string yTitle;

    public GraphFunction function;

    public Graph(float grid, string xTitle, string yTitle, GraphFunction function)
    {
        Grid = grid;
        this.xTitle = xTitle;
        this.yTitle = yTitle;
        this.function = function;
    }

    public Graph(float[] xValues, float[] yValues, string xTitle, string yTitle)
    {
        this.xValues = (float[])xValues.Clone();
        this.yValues = (float[])yValues.Clone();
        this.xTitle = xTitle;
        this.yTitle = yTitle;
    }

    public void CreateGraphWithFunction(float negativeXLimit, float positiveXLimit)
    {
        int negativeLen = (int)(negativeXLimit / Grid);
        int positiveLen = (int)(positiveXLimit / Grid);
        int lenght = -negativeLen + positiveLen + 1; // +1 is for 0
        yValues = new float[lenght];
        xValues = new float[lenght];
        for (int i = 0; i < lenght; i++)
        {
            xValues[i] = (i+negativeLen) * Grid;
            yValues[i] = function.Invoke(xValues[i]);
        }
    }

    private float MinValue(float[] data) // returns minimum value of an array
    {
        float max = Mathf.Infinity;
        for (int i = 0; i < data.Length; i++)
            if (data[i] < max) max = data[i];

        return max;
    }

    private float MaxValue(float[] data) // returns maximum value of an array
    {
        float min = Mathf.NegativeInfinity;
        for (int i = 0; i < data.Length; i++)
            if (data[i] > min) min = data[i];

        return min;
    }

    public float MinValue_Y()
    {
        return MinValue(yValues);
    }
    public float MinValue_X()
    {
        return MinValue(xValues);
    }

    public float MaxValue_Y()
    {
        return MaxValue(yValues);
    }
    public float MaxValue_X()
    {
        return MaxValue(xValues);
    }
}