using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HcLevelManager : MonoSingleton<HcLevelManager>
{

    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private int levelIndex = 0;

    private int _globalLevelIndex = 0;
    private bool _inited = false;
    private GameObject _currentLevel;

    public void Init()
    {
        _globalLevelIndex = PlayerPrefs.GetInt(ConstantVariables.LevelValue.Level);

        levelIndex = _globalLevelIndex;

        if (levelIndex >= levelPrefabs.Length)
        {
            levelIndex = Random.Range(0, levelPrefabs.Length);
        }

        GenerateCurrentLevel();
    }

    public void DeInit()
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
    }

    public void GenerateCurrentLevel()
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        _currentLevel = Instantiate(levelPrefabs[levelIndex]);
    }

    public GameObject GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void LevelUp()
    {
        _globalLevelIndex++;
        PlayerPrefs.SetInt(ConstantVariables.LevelValue.Level, _globalLevelIndex);
        levelIndex = _globalLevelIndex;

        if (levelIndex >= levelPrefabs.Length)
        {
            levelIndex = Random.Range(0, levelPrefabs.Length);
        }

        Init();
    }

    public int GetGlobalLevelIndex()
    {
        return _globalLevelIndex;
    }
}
