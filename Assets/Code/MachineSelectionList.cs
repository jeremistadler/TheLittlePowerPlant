﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineSelectionList : MonoBehaviour
{
    public Action<Turbine> MachineSelected;
    public ItemList ItemList;

	// Use this for initialization
	void Start ()
	{
	    ItemList.Items = new List<object>()
	        {
	            new Turbine() { Name = "Turbine", Cost = 10000 },
                new Turbine() { Name = "Coal", Cost = 20000 },
                new Turbine() { Name = "Nuclear", Cost = 30000},
                new Turbine() { Name = "Wind", Cost = 40000 },
	        };
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void Build()
    {
        if (MachineSelected != null)
        {
            MachineSelected((Turbine) ItemList.Selected);
        }
    }
}