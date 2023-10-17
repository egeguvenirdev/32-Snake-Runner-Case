using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeGrid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private Color32 green;
    [SerializeField] private Color32 red;
    [SerializeField] private LayerMask layer;
    [SerializeField] private bool isSelecterGrid;
    [SerializeField] private bool isEmpty = true;

    public bool EmptyCheck
    {
        get => isEmpty;
    }

    public bool GetIsSelecterGrid
    {
        get => isSelecterGrid;
    }

    public void CheckSphere()
    {
        isEmpty = !Physics.CheckSphere(transform.position, 0.05f, layer);
    }

    public void SetColor(bool check)
    {

        if (check)
        {
            if (isEmpty)
            {
                image.color = green;
                return;
            }
            image.color = red;
            return;
        }
        image.color = new Color(0.7075472f, 0.7075472f, 0.7075472f, 1f);
    }
}