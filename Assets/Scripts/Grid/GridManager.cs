using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridManager : MonoSingleton<GridManager>
{
    [SerializeField] private GameObject gridHolder;
    [SerializeField] private MergeGrid[] grids;
    //[SerializeField] private BuyButton mergeButton;

    public void Init()
    {
        gridHolder.SetActive(true);
        ActionManager.FindEmptyGrid += OnFindEmptyGrid;
        ActionManager.MergeColorCheck += OnColorCheck;
    }

    public void DeInit()
    {
        gridHolder.SetActive(false);
        ActionManager.FindEmptyGrid -= OnFindEmptyGrid;
        ActionManager.MergeColorCheck -= OnColorCheck;
    }

    public void OnFindEmptyGrid(GameObject refGun)
    {
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].CheckSphere();
            OnColorCheck(false);
            if (grids[i].EmptyCheck)
            {
                refGun.transform.position = grids[i].transform.position;
                refGun.transform.rotation = grids[i].transform.rotation;
                PlayDoPunch(refGun.transform);
                return;
            }
        }
    }

    public bool EmptyCheck()
    {
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].CheckSphere();

            if (grids[i].EmptyCheck)
            {
                return true;
            }
        }
        return false;
    }

    private void OnColorCheck(bool check)
    {
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].CheckSphere();
            grids[i].SetColor(check);
        }
    }

    private void PlayDoPunch(Transform refGun)
    {
        refGun.DOScale(new Vector3(1f, 1f, 1f), 0f);
        refGun.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2).OnComplete(() => { ActionManager.MergeColorCheck?.Invoke(false); });
    }
}