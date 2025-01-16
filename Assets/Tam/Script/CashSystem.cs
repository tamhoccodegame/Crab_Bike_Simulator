using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashSystem
{
    public float currentPayment;

    public int CalculatePayment(Vector3 a, Vector3 b)
    {
		//Quy định 100 Unity Unit = 1km. 1km = 5000VND. 
		currentPayment = (Vector3.Distance(a, b) / 100f) * 5000;
        currentPayment = (int)currentPayment;
        return (int)currentPayment;
    }
}
