using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler : MonoSingleton<ObjectPooler>
{
    [SerializeField] private List<ObjectPooledItem> itemsToPool;
    [SerializeField] private List<ObjectPooledItem> ballsToPool;
    [SerializeField] private GameObject pooledObjectHolder;

    private List<SlideText> pooledText;
    private List<MergeableBall> pooledBalls;

    private void Awake()
    {
        pooledText = new List<SlideText>();
        foreach (ObjectPooledItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.transform.SetParent(pooledObjectHolder.transform);
                obj.SetActive(false);
                pooledText.Add(obj.GetComponent<SlideText>());
            }
        }

        pooledBalls = new List<MergeableBall>();
        foreach (ObjectPooledItem item in ballsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.transform.SetParent(pooledObjectHolder.transform);
                obj.transform.localPosition = new Vector3(0, 0, 100);
                obj.SetActive(false);
                pooledBalls.Add(obj.GetComponent<MergeableBall>());
            }
        }
    }

    public SlideText GetPooledText()
    {
        for (int i = pooledText.Count - 1; i > -1; i--)
        {
            if (!pooledText[i].gameObject.activeInHierarchy && pooledText[i].tag == tag)
            {
                return pooledText[i];
            }
        }
        foreach (ObjectPooledItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    SlideText slideText = obj.GetComponent<SlideText>();
                    obj.SetActive(false);
                    pooledText.Add(slideText);
                    obj.transform.SetParent(pooledObjectHolder.transform);
                    return slideText;
                }
            }
        }
        return null;
    }

    public MergeableBall GetPooledBall(int level)
    {
        for (int i = pooledBalls.Count - 1; i > -1; i--)
        {
            if (!pooledBalls[i].gameObject.activeInHierarchy)
            {
                if (pooledBalls[i].GetComponent<MergeableBall>().GetBallLevel == level)
                    return pooledBalls[i];
            }
        }
        foreach (ObjectPooledItem item in ballsToPool)
        {
            if (item.shouldExpand)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                MergeableBall objSc = obj.GetComponent<MergeableBall>();
                obj.SetActive(false);
                pooledBalls.Add(objSc);
                obj.transform.SetParent(pooledObjectHolder.transform);
                return objSc;
            }
        }
        return null;
    }

    public void ClosePooledBalls()
    {
        for (int i = pooledBalls.Count - 1; i > -1; i--)
        {
            if (pooledBalls[i].gameObject.activeInHierarchy)
            {
                pooledBalls[i].gameObject.SetActive(false);
            }
        }
    }

    public void ClosePooledObjects()
    {
        for (int i = pooledBalls.Count - 1; i > -1; i--)
        {
            if (pooledBalls[i].gameObject.activeInHierarchy)
            {
                pooledBalls[i].gameObject.SetActive(false);
            }
        }

        for (int i = pooledText.Count - 1; i > -1; i--)
        {
            if (pooledText[i].gameObject.activeInHierarchy && pooledText[i].tag == tag)
            {
                pooledText[i].gameObject.SetActive(false);
            }
        }
    }
}