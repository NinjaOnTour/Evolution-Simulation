using UnityEngine;
using UnityEngine.UI;

public class GraphPanel : MonoBehaviour
{
    public Graph graph;
    public LineRenderer GraphLine;
    public Transform Origin; 
    public LineRenderer xAxis;
    public LineRenderer yAxis;
    public Text xTitle;
    public Text yTitle;
    public float xAxisLenght { get; private set; } // Real distance between minimum x and maximum x values
    public float yAxisLenght { get; private set; } // Real distance between minimum y and maximum y values
    public float xAxisUILenght { get; private set; } // Lenght of x axis in the user interface
    public float yAxisUILenght { get; private set; } // Lenght of y axis in the user interface

    // Magnification = UILenght / AxisLenght
    public float xMagnification { get; private set; }  
    public float yMagnification { get; private set; }

    // Maximum and minimum values of x and y:
    public float yMin { get; private set; }
    public float xMin { get; private set; }
    public float yMax { get; private set; }
    public float xMax { get; private set; }

    public void DrawGraph()
    {
        xTitle.text = graph.xTitle;
        yTitle.text = graph.yTitle;

        xMin = graph.MinValue_X();
        yMin = graph.MinValue_Y();
        xMax = graph.MaxValue_X();
        yMax = graph.MaxValue_Y();
        xAxisLenght = xMax;
        yAxisLenght = yMax;
        xAxisUILenght = Vector3.Distance(xAxis.GetPosition(0), xAxis.GetPosition(1));
        yAxisUILenght = Vector3.Distance(yAxis.GetPosition(0), yAxis.GetPosition(1));
        xMagnification = xAxisUILenght / xAxisLenght;
        yMagnification = yAxisUILenght / yAxisLenght;

        if (xMin < 0f)
        {
            xAxisLenght = xMax - xMin;
            xMagnification = xAxisUILenght / xAxisLenght;
            float xAxisLenght_negative = -xMin;
            Origin.localPosition += new Vector3(xAxisLenght_negative * xMagnification, 0, 0);
            yAxis.transform.localPosition += new Vector3(xAxisLenght_negative * xMagnification, 0, 0);
        }
        if (yMin < 0f)
        {
            yAxisLenght = yMax - yMin;
            yMagnification = yAxisUILenght / yAxisLenght;
            float yAxisLenght_negative = -yMin;
            Origin.localPosition += new Vector3(0f, yAxisLenght_negative * yMagnification, 0f);
            xAxis.transform.localPosition += new Vector3(0f, yAxisLenght_negative * yMagnification, 0f);
        }

        GraphLine.positionCount = graph.xValues.Length;
        for (int i = 0; i < graph.xValues.Length; i++)
        {
            GraphLine.SetPosition(i, Origin.localPosition + new Vector3(graph.xValues[i] * xMagnification, graph.yValues[i] * yMagnification, 0f));
        }
    }
}