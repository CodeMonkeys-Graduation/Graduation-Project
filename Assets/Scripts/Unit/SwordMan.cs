﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMan : Unit, IClickable
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Run();
    }

    public void OnClick()
    {
        Debug.Log(gameObject.name + " OnClick");
    }


}