using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributionSimulation : MonoBehaviour
{
    public float CalculationPeriod = 3;
    public GraphPanel graph;
    public Dropdown DataType;
    [Header("Data")]
    public float AverageMinTemp;
    public float AverageMaxTemp;
    public float AverageSize;
    public float AverageSplitEnergy;
    public float AverageChildEnergy;
    public float AverageEnergySpending;
    public float AverageEnergyCapacity;
    public float AverageMovingForce;
    public float AverageMovingAngle;
    public float AverageSplitCost;
    public float AverageEnergy;
    public float AverageSpeed;
    public float AverageMutationCount;
    public float AverageGeneticStability;
    public int Population;
    
    internal List<float> TimeValues = new List<float>();

    public List<float>[] Data = new List<float>[15];

    private List<Cell> Cells;

    private void Start()
    {
        for (int i = 0; i < Data.Length; i++)
        {
            Data[i] = new List<float>();
        }

        InvokeRepeating("Calculate", 0.1f, CalculationPeriod);
        Cells = GameManager.instance.Cells;
    }

    public void Calculate()
    {
        float T_MinTemp = 0;
        float T_MaxTemp = 0;
        float T_Size = 0;
        float T_SplitEnergy = 0;
        float T_ChildEnergy = 0;
        float T_EnergySpending = 0;
        float T_EnergyCapacity = 0;
        float T_MovingForce = 0;
        float T_MovingAngle = 0;
        float T_SplitCost = 0;
        float T_Energy = 0;
        float T_Speed = 0;
        float T_MutationCount = 0;
        float T_GeneticStability = 0;

        float count = Cells.Count;

        for (int i = 0; i < count; i++)
        {
            T_MinTemp += Cells[i].MinTemperature;
            T_MaxTemp += Cells[i].MaxTemperature;
            T_Size += Cells[i].Size;
            T_SplitEnergy += Cells[i].SplitEnergy;
            T_ChildEnergy += Cells[i].ChildEnergy;
            T_EnergySpending += Cells[i].EnergySpending;
            T_EnergyCapacity += Cells[i].EnergyCapacity;
            T_MovingForce += Cells[i].MovingForce;
            T_MovingAngle += Cells[i].MovingAngle;
            T_SplitCost += Cells[i].SplitCost;
            T_Energy += Cells[i].Energy;
            T_Speed += Cells[i].Speed;
            T_MutationCount += Cells[i].MutationCount;
            T_GeneticStability += Cells[i].GeneticStability;
        }

        AverageMinTemp = T_MinTemp / count;
        AverageMaxTemp = T_MaxTemp / count;
        AverageSize = T_Size / count;
        AverageSplitEnergy = T_SplitEnergy / count;
        AverageChildEnergy = T_ChildEnergy / count;
        AverageEnergySpending = T_EnergySpending / count;
        AverageEnergyCapacity = T_EnergyCapacity / count;
        AverageMovingForce = T_MovingForce / count;
        AverageMovingAngle = T_MovingAngle / count;
        AverageSplitCost = T_SplitCost / count;
        AverageEnergy = T_Energy / count;
        AverageSpeed = T_Speed / count;
        AverageMutationCount = T_MutationCount / count;
        AverageGeneticStability = T_GeneticStability / count;
        Population = Cells.Count;

        TimeValues.Add(Time.time);
        Data[0].Add(AverageMinTemp);
        Data[1].Add(AverageMaxTemp);
        Data[2].Add(AverageSize);
        Data[3].Add(AverageSplitEnergy);
        Data[4].Add(AverageChildEnergy);
        Data[5].Add(AverageEnergySpending);
        Data[6].Add(AverageEnergyCapacity);
        Data[7].Add(AverageMovingForce);
        Data[8].Add(AverageMovingAngle);
        Data[9].Add(AverageSplitCost);
        Data[10].Add(AverageEnergy);
        Data[11].Add(AverageSpeed);
        Data[12].Add(AverageMutationCount);
        Data[13].Add(AverageGeneticStability);
        Data[14].Add(Population);
    }

    public void VisualizeData(int dataNumber)
    {
        graph.graph.xValues = TimeValues.ToArray();
        graph.graph.yValues = new float[TimeValues.Count];
        for (int i = 0; i < TimeValues.Count; i++)
        {
            graph.graph.yValues[i] = Data[dataNumber][i];
        }

        graph.DrawGraph();
    }

    public void VisualizeDataButton()
    {
        Debug.Log(DataType.value);
        VisualizeData(DataType.value);
    }

    public void ExitButton()
    {

    }
}