using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractMapTransferFunction : MonoBehaviour {


    public static float GetSigmoidValue(float x, float h, float s, float r)
    {
        float eulerPower = s * (r - x);
        float value = h / (1 + Mathf.Pow((float) Math.E, eulerPower));

        return value;
    }
}
