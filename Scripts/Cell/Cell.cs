using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Mutation(Cell cell);

public class Cell : MonoBehaviour
{
    [SerializeField] private float minTemperature;
    [SerializeField] private float maxTemperature;
    [SerializeField] private float size;
    [SerializeField] private float splitEnergy;
    [SerializeField] private float childEnergy;
    [SerializeField] private float movingForce;
    [SerializeField] private float movingAngle;
    [SerializeField] private float energy;
    [SerializeField] private int mutationCount;
    [SerializeField] private float geneticStability;

    public float MinTemperature
    {
        get
        {
            return minTemperature;
        }
        set
        {
            minTemperature = value;
            if (minTemperature >= maxTemperature) minTemperature = maxTemperature - 1f;
        }
    }
    public float MaxTemperature
    {
        get
        {
            return maxTemperature;
        }
        set
        {
            maxTemperature = value;
            if (maxTemperature <= minTemperature) maxTemperature = minTemperature + 1f;
        }
    }
    public float Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
            if (size < 0.1f) size = 0.1f;
        }
    }
    public float SplitEnergy // Mininmum energy to split
    {
        get
        {
            return splitEnergy;
        }
        set
        {
            splitEnergy = value;
            if (splitEnergy < 1f) splitEnergy = 1f;
        }
    }
    public float ChildEnergy // Transfer of energy to the child cell when cell splits
    {
        get
        {
            return childEnergy;
        }
        set
        {
            childEnergy = value;
            if (childEnergy < 1f) childEnergy = 1f;
        }
    }
    public float EnergySpending { get; private set; }
    public float EnergyCapacity { get; private set; }
    public float MovingForce
    {
        get
        {
            return movingForce;
        }
        set
        {
            movingForce = value;
            if (movingForce < 0f) movingForce = 0f;
        }
    }
    public float MovingAngle
    {
        get
        {
            return movingAngle;
        }
        set
        {
            movingAngle = value;
            if (movingAngle >= 360f)
            {
                movingAngle -= 360f;
            }
            else if (movingAngle < 0)
            {
                movingAngle += 360f;
            }
        }
    }
    public float SplitCost { get; private set; } // Decrease in energy when cell splits
    public float Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = value;

            if (energy <= 0)
            {
                energy = 0;
                Die();
            }
            else if (energy > EnergyCapacity)
            {
                energy = EnergyCapacity;
            }
        }
    }
    public float Speed { get; private set; }
    public Vector2 Velocity { get; private set; }
    private float _VelocityAngle;
    internal float VelocityAngleRad { get; private set; } // Velocity Angle in radians
    public float VelocityAngle // Angle between velocity vector and x-axis in degrees
    {
        get
        {
            return _VelocityAngle;
        }
        set
        {
            _VelocityAngle = value;

            if (_VelocityAngle >= 360f)
            {
                _VelocityAngle -= 360f;
            }
            else if (_VelocityAngle < 0)
            {
                _VelocityAngle += 360f;
            }

            VelocityAngleRad = Mathf.Deg2Rad * _VelocityAngle;
            Velocity = new Vector2(Speed * Mathf.Cos(VelocityAngleRad), Speed * Mathf.Sin(VelocityAngleRad));
        }
    }
    public int MutationCount
    {
        get
        {
            return mutationCount;
        }
        set
        {
            mutationCount = value;
            if (mutationCount < 0) mutationCount = 0;
        }
    }
    public float GeneticStability // %0-100
    {
        get
        {
            return geneticStability;
        }

        set
        {
            geneticStability = Mathf.Clamp(value, 0f, 100f);
        }
    }
    private static Mutation[] Mutations =
    {
        new Mutation(MutateMinTemperature),
        new Mutation(MutateMaxTemperature),
        new Mutation(MutateSize),
        new Mutation(MutateSplitEnergy),
        new Mutation(MutateChildEnergy),
        new Mutation(MutateMovingForce),
        new Mutation(MutateMovingAngle),
        new Mutation(MutateMutationCount),
        new Mutation(MutateGeneticStability),
    };

    private void Start()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        float temp = GameManager.GlobalEnviroment.Temperature;
        if (temp > MaxTemperature || temp < MinTemperature)
        {
            Die();
        }
        else
        {
            Energy -= EnergySpending * Time.fixedDeltaTime;

            if (Energy > SplitEnergy)
            {
                Split();
            }
        }

        Move();
    }

    private void Move()
    {
        VelocityAngle += Random.Range(-MovingAngle / 2f, MovingAngle / 2f);
        transform.position += (Vector3)Velocity * Time.fixedDeltaTime;
    }

    public void Initialize()
    {
        EnergySpending = (Size + (MaxTemperature - MinTemperature) + MovingForce * GameManager.instance.C_MoveEnergy) * GameManager.instance.C_EnergySpending;
        EnergyCapacity = Size * GameManager.instance.C_EnergyCapacity;
        SplitCost = ChildEnergy + Size * GameManager.instance.C_EnergyOfSize; ///////////////////////////
        if (Energy == 0) Energy = ChildEnergy;
        Speed = MovingForce / Size * GameManager.instance.C_Speed;
        VelocityAngle = Random.Range(0f, 360f);
        transform.localScale = Vector2.one * Size;
        GameManager.instance.Cells.Add(this);
    }

    public void Split()
    {
        if (SplitCost >= Energy || GameManager.instance.isPopulationCapacityFull) return;

        Cell child = Instantiate(GameManager.instance.CellPrefab, transform.position, Quaternion.identity).GetComponent<Cell>();
        child.MinTemperature = MinTemperature;
        child.MaxTemperature = MaxTemperature;
        child.Size = Size;
        child.SplitEnergy = SplitEnergy;
        child.ChildEnergy = ChildEnergy;
        child.MovingForce = MovingForce;
        child.MovingAngle = MovingAngle;
        child.MutationCount = MutationCount;
        child.GeneticStability = GeneticStability;
        child.Mutate();

        Energy -= SplitCost;
    }

    public void Die()
    {
        //Food food = Instantiate(GameManager.instance.FoodPrefab, transform.position, Quaternion.identity).GetComponent<Food>();
        //food.Energy = Size * GameManager.instance.C_EnergyOfSize;
        GameManager.instance.Cells.Remove(this);
        Destroy(gameObject);
    }

    public void Mutate()
    {
        for (int i = 0; i < MutationCount; i++)
        {
            float x = Random.Range(0f, 100f);
            if (x > GeneticStability)
            {
                Mutations[Random.Range(0, Mutations.Length)].Invoke(this);
            }
        }
    }

    #region Mutations
    private static void MutateMinTemperature(Cell cell)
    {
        float k = GameManager.instance.M_MinTemp;
        cell.MinTemperature += Random.Range(-k, k);
    }
    private static void MutateMaxTemperature(Cell cell)
    {
        float k = GameManager.instance.M_MaxTemp;
        cell.MaxTemperature += Random.Range(-k, k);
    }
    private static void MutateSize(Cell cell)
    {
        float k = GameManager.instance.M_Size;
        cell.Size += Random.Range(-k, k);
    }
    private static void MutateSplitEnergy(Cell cell)
    {
        float k = GameManager.instance.M_SplitEnergy;
        cell.SplitEnergy += Random.Range(-k, k);
    }
    private static void MutateChildEnergy(Cell cell)
    {
        float k = GameManager.instance.M_ChildEnergy;
        cell.ChildEnergy += Random.Range(-k, k);
    }
    private static void MutateMovingForce(Cell cell)
    {
        float k = GameManager.instance.M_MovingForce;
        cell.MovingForce += Random.Range(-k, k);
    }
    private static void MutateMovingAngle(Cell cell)
    {
        float k = GameManager.instance.M_MovingAngle;
        cell.MovingAngle += Random.Range(-k, k);
    }
    private static void MutateMutationCount(Cell cell)
    {/*
        int k = GameManager.instance.M_MutationCount;
        cell.MutationCount += Random.Range(-k, k);*/
    }
    private static void MutateGeneticStability(Cell cell)
    {/*
        float k = GameManager.instance.M_GeneticStability;
        cell.GeneticStability += Random.Range(-k, k);*/
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        if (tag == "Food")
        {
            Energy += collision.GetComponent<Food>().Energy;
            Destroy(collision.gameObject);
        }
        else if (tag == "VerticalBorder")
        {
            Vector2 newV = new Vector2(Velocity.x, -Velocity.y);
            VelocityAngle = Mathf.Asin(newV.y / Speed) * Mathf.Rad2Deg;
        }
        else if (tag == "HorizontalBorder")
        {
            Vector2 newV = new Vector2(-Velocity.x, Velocity.y);
            VelocityAngle = Mathf.Acos(newV.x / Speed) * Mathf.Rad2Deg;
        }
    }
}