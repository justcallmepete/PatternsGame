using System.Diagnostics;
using System;
using UnityEngine;
using System.Collections;

public interface Command
{
    void Execute();
}

class MoveForward : Command
{
    private MovementComponent mc;

    public MoveForward(MovementComponent mc)
    {
        this.mc = mc;
    }

    public void Execute()
    {
        mc.SetMovementValues();
    }
}

class MoveBackwards : Command
{
    private MovementComponent mc;

    public MoveBackwards(MovementComponent mc)
    {
        this.mc = mc;
    }

    public void Execute()
    {
        mc.SetMovementValues();
    }
}

class Teleport : Command
{
    private TeleportComponent tc;

    public Teleport(TeleportComponent tc)
    {
        this.tc = tc;
    }

    public void Execute()
    {
        tc.StartTeleport();
        tc.Channelling(tc.GetClosestPlayer());
    }
}

class DoNothing : Command
{
    private MovementComponent mc;

    public DoNothing(MovementComponent mc)
    {
    }

    public void Execute()
    {
        mc.SetMovementValues();
    }
}
