﻿using UnityEngine;

public interface IMachineType
{
    string Name { get; set; }
    int Cost { get; set; }
    bool IsPoweredOn { get; }
    float Output { get; set; }
    float MinOutput { get; }
    float MaxOutput { get; }

    void TogglePower();
    void Repair();
}

public class Turbine : IMachineType
{
    public string Name { get; set; }
    public int Cost { get; set; }
    public float Output { get; set; }
    public float RequestedOutput { get; set; }
    public bool IsPoweredOn { get; private set; }
    public bool IsRepairing { get; private set; }
    public float Durability { get; set; }

    public float MinOutput { get { return 50; } }
    public float MaxOutput { get { return 150; } }

    public Turbine()
    {
        IsPoweredOn = true;
        Output = MinOutput;
        Durability = 1;
    }

    public void TogglePower()
    {
        if (IsPoweredOn)
        {
            PowerOff();
        }
        else
        {
            PowerOn();
        }
    }

    private void PowerOn()
    {
        if(!IsRepairing)
        {
            IsPoweredOn = true;
            Output = MinOutput;
        }
    }

    private void PowerOff()
    {
        IsPoweredOn = false;
        Output = 0;
    }

    public void Repair()
    {
        PowerOff();
        IsRepairing = true;
    }

    public void RepairFinished()
    {
        PowerOn();
        IsRepairing = false;
    }

    public void Break()
    {
        Repair();
    }
}