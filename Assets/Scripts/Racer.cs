using System;
using UnityEngine;

public class Racer
{
    public Guid Id { get; private set; }
    public int Score { get; private set; }

    private VehicleController _vehicleController;

    public Racer()
    {
        Id = Guid.NewGuid();
        Score = 0;
        Debug.Log(Id);
    }

    public void SetScore(int value) => Score = value;
}