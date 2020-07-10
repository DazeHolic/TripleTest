using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class TripleGame
{
    private enum ACT
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    private System.Random rnd = new System.Random();
    private int rowMax;
    private int columnMax;
    private int colourMax;
    private int[,] colours;
    private bool show;

    public void Start(bool show = false, int rowMax = 7, int columnMax = 7, int colourMax = 4)
    {
        this.show = show;
        this.rowMax = rowMax;
        this.columnMax = columnMax;
        this.colourMax = colourMax;
        this.colours = new int[rowMax, columnMax];
        GameReset();
    }

    public void GetObservation(VectorSensor sensor)
    {
        for (int i = 0; i < this.rowMax; i++)
        {
            for (int j = 0; j < this.columnMax; j++)
            {
                sensor.AddOneHotObservation(this.colours[i, j] - 1, this.colourMax);
            }
        }
    }

    // 执行操作
    public bool DoAction(int row, int column, int act)
    {
        if (CheckActionValid(row, column, act))
        {
            int colour = this.GetColour(row, column);
            int targetRow = -1, targetColumn = -1;
            switch (act)
            {
                case (int)ACT.UP:
                    {
                        targetRow = row - 1;
                        targetColumn = column;
                    }
                    break;
                case (int)ACT.DOWN:
                    {
                        targetRow = row + 1;
                        targetColumn = column;
                    }
                    break;
                case (int)ACT.LEFT:
                    {
                        targetRow = row;
                        targetColumn = column - 1;
                    }
                    break;
                case (int)ACT.RIGHT:
                    {
                        targetRow = row;
                        targetColumn = column + 1;
                    }
                    break;
                default:
                    break;
            }

            int targetColour = GetColour(targetRow, targetColumn);
            SetColour(row, column, targetColour);
            SetColour(targetRow, targetColumn, colour);
            EnsureValid();
            return true;
        }
        if (this.show)
        {
            System.Console.WriteLine("Action Error!");
        }
        return false;
    }

    public void Show()
    {
        if (!this.show)
        {
            return;
        }
        System.Console.Write("  ");
        for (int k = 0; k < this.columnMax; k++)
        {
            System.Console.Write(k);
            System.Console.Write(" ");
        }
        for (int i = 0; i < this.rowMax; i++)
        {
            System.Console.Write("\n");
            System.Console.Write(i);
            System.Console.Write(" ");
            for (int j = 0; j < this.columnMax; j++)
            {
                int colour = GetColour(i, j);
                string s = "";
                switch (colour)
                {
                    case 1:
                        s = "☆";
                        break;
                    case 2:
                        s = "●";
                        break;
                    case 3:
                        s = "□";
                        break;
                    case 4:
                        s = "△";
                        break;
                    case 5:
                        s = "○";
                        break;
                    default:
                        s = "X";
                        break;
                }
                System.Console.Write(s);
            }
        }
        System.Console.Write("\n");
    }

    private int GetColour(int row, int column)
    {
        if (row < 0 || row >= this.rowMax || column < 0 || column >= this.columnMax)
        {
            return -1;
        }
        return this.colours[row, column];
    }

    private int SetColour(int row, int column, int colour)
    {
        if (row < 0 || row >= this.rowMax || column < 0 || column >= this.columnMax)
        {
            return -1;
        }
        this.colours[row, column] = colour;
        return colour;
    }

    private bool IsColourValid(int colour)
    {
        if (colour > 0 || colour <= this.colourMax)
        {
            return true;
        }
        return false;
    }


    private bool CheckActionValid(int row, int column, int act)
    {
        int colour = this.GetColour(row, column);
        if (!IsColourValid(colour))
        {
            return false;
        }
        int row1 = -1, row2 = -1, column1 = -1, column2 = -1;
        switch (act)
        {
            // 注释中 O 为操作目标
            case (int)ACT.UP:
                {
                    // OOA
                    // BBO
                    row1 = row - 1;
                    column1 = column - 2;
                    column2 = column - 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row1, column2) == colour)
                        return true;


                    // OAO
                    // BOB
                    row1 = row - 1;
                    column1 = column - 1;
                    column2 = column + 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row1, column2) == colour)
                        return true;

                    // AOO
                    // OBB
                    row1 = row - 1;
                    column1 = column + 1;
                    column2 = column + 2;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row1, column2) == colour)
                        return true;

                    // AOB
                    // BOA
                    // ABB
                    // BOB
                    row1 = row - 3;
                    row2 = row - 2;
                    column1 = column;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row2, column1) == colour)
                        return true;
                }
                break;
            case (int)ACT.DOWN:
                {
                    // BBO
                    // OOA
                    row1 = row + 1;
                    column1 = column - 1;
                    column2 = column - 2;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row1, column2) == colour)
                        return true;

                    // BOB
                    // OAO
                    row1 = row + 1;
                    column1 = column - 1;
                    column2 = column + 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row1, column2) == colour)
                        return true;

                    // OBB
                    // AOO
                    row1 = row + 1;
                    column1 = column + 2;
                    column2 = column + 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row1, column2) == colour)
                        return true;

                    // BOA
                    // ABB
                    // AOB
                    // BOA
                    row1 = row + 2;
                    row2 = row + 3;
                    column1 = column;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row2, column1) == colour)
                        return true;

                }
                break;
            case (int)ACT.LEFT:
                {
                    // OB
                    // OB
                    // AO
                    row1 = row - 1;
                    row2 = row - 2;
                    column1 = column - 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row2, column1) == colour)
                        return true;

                    // OB
                    // AO
                    // OB
                    row1 = row - 1;
                    row2 = row + 1;
                    column1 = column - 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row2, column1) == colour)
                        return true;

                    // AO
                    // OB
                    // OB
                    row1 = row + 1;
                    row2 = row + 2;
                    column1 = column - 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row2, column1) == colour)
                        return true;

                    // ABA
                    // OOBO
                    // BAA
                    row1 = row;
                    column1 = column - 3;
                    column2 = column - 2;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row1, column2) == colour)
                        return true;
                }
                break;
            case (int)ACT.RIGHT:
                {
                    // BO
                    // BO
                    // OA
                    row1 = row - 1;
                    row2 = row - 2;
                    column1 = column + 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row2, column1) == colour)
                        return true;

                    // BO
                    // OA
                    // BO
                    row1 = row + 1;
                    row2 = row - 1;
                    column1 = column + 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row2, column1) == colour)
                        return true;

                    // OA
                    // BO
                    // BO
                    row1 = row + 1;
                    row2 = row + 2;
                    column1 = column + 1;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row2, column1) == colour)
                        return true;

                    //  ABA
                    // OBOO
                    //  BAA
                    row1 = row;
                    column1 = column + 2;
                    column2 = column + 3;
                    if (this.GetColour(row1, column1) == colour && this.GetColour(row1, column2) == colour)
                        return true;
                }
                break;
        }
        return false;
    }

    private int GetRandomColour()
    {
        return rnd.Next(1, colourMax + 1);
    }

    private void ColourReset()
    {
        for (int i = 0; i < rowMax; i++)
        {
            for (int j = 0; j < columnMax; j++)
            {
                SetColour(i, j, GetRandomColour());
            }
        }
    }

    private void GameReset()
    {
        ColourReset();
        EnsureValid();
    }

    // 直接暴力循环处理已经形成三连的元素(直接以每个元素为中心，判断四周即可)
    private void ProcessTriple()
    {
        // 保证不存在已经三连的元素
        bool find = true;
        while (find)
        {
            find = false;
            for (int i = 0; i < rowMax; i++)
            {
                for (int j = 0; j < columnMax; j++)
                {
                    int colour = GetColour(i, j);

                    if (GetColour(i - 1, j) == colour && GetColour(i + 1, j) == colour)
                    {
                        SetColour(i, j, GetRandomColour());
                        SetColour(i - 1, j, GetRandomColour());
                        SetColour(i + 1, j, GetRandomColour());
                        find = true;
                    }
                    else if (GetColour(i, j - 1) == colour && GetColour(i, j + 1) == colour)
                    {
                        SetColour(i, j, GetRandomColour());
                        SetColour(i, j - 1, GetRandomColour());
                        SetColour(i, j + 1, GetRandomColour());
                        find = true;
                    }
                }
            }
        }
    }

    // 直接暴力循环检查是否有通过一步即可形成三连的地方
    private bool CheckTripleValid()
    {
        for (int i = 0; i < this.rowMax; i++)
        {
            for (int j = 0; j < this.columnMax; j++)
            {
                for (int act = (int)ACT.UP; act <= (int)ACT.RIGHT; act++)
                {
                    if (CheckActionValid(i, j, act))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // 形成既没有已经三连的情况，又有单步可消除的环境
    private void EnsureValid()
    {
        while (true)
        {
            ProcessTriple();
            if (!CheckTripleValid())
            {
                ColourReset();
            }
            else
            {
                break;
            }
        }
        if (this.show)
        {
            Show();
        }
    }
}



public class RollerAgent : Agent
{
    Rigidbody rBody;
    TripleGame game;
    int count = 0;
    void Start () 
    {
        rBody = GetComponent<Rigidbody>();
        game = new TripleGame();
    }

    public Transform Target;

    public override void OnEpisodeBegin()
    {
        this.game.Start(false, 7, 7, 3);
        RequestDecision();
        //if (this.transform.localPosition.y < 0)
        //{
        //    // If the Agent fell, zero its momentum
        //    this.rBody.angularVelocity = Vector3.zero;
        //    this.rBody.velocity = Vector3.zero;
        //    this.transform.localPosition = new Vector3( 0, 0.5f, 0);
        //}

        //// Move the target to a new spot
        //Target.localPosition = new Vector3(Random.value * 8 - 4,
        //                                0.5f,
        //                                Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        this.game.GetObservation(sensor);
        //// Target and Agent positions
        //sensor.AddObservation(Target.localPosition);
        //sensor.AddObservation(this.transform.localPosition);

        //// Agent velocity
        //sensor.AddObservation(rBody.velocity.x);
        //sensor.AddObservation(rBody.velocity.z);
    }

    public float speed = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        bool success = this.game.DoAction((int)vectorAction[0], (int)vectorAction[1], (int)vectorAction[2]);
        if (success)
        {
            SetReward(1.0f);
            this.count += 1;
            if (count % 50 == 0)
            {
                EndEpisode();
            }
        }
        else
        {
            SetReward(-1.0f);
        }

        RequestDecision();
        //// Actions, size = 2
        //Vector3 controlSignal = Vector3.zero;
        //controlSignal.x = vectorAction[0];
        //controlSignal.z = vectorAction[1];
        //rBody.AddForce(controlSignal * speed);

        //// Rewards
        //float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        //// Reached target
        //if (distanceToTarget < 1.42f)
        //{
        //    SetReward(1.0f);
        //    EndEpisode();
        //}

        //// Fell off platform
        //if (this.transform.localPosition.y < 0)
        //{
        //    EndEpisode();
        //}
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}
