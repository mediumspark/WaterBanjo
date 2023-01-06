using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PauseMenu : MonoBehaviour
{
    public Button Resume, Setting, Exit;
    [SerializeField]
    private GameObject[] _MenuItems;

    private static GameObject[] _MenuItemsReferenceArray;

    private void Awake()
    {
        _MenuItemsReferenceArray = _MenuItems;
        CycleChildrenActive(); 
    }


    public static void OnPause()
    {
        Time.timeScale = Time.timeScale == 0 ? Time.timeScale = 1 : Time.timeScale = 0;
        CycleChildrenActive(); 
    }


    private static void CycleChildActive(int i)
    {
        _MenuItemsReferenceArray[i].SetActive(!_MenuItemsReferenceArray[i].activeSelf);
    }

    private static void CycleChildrenActive()
    {
        for (int i = 0; i < _MenuItemsReferenceArray.Length; i++)
            CycleChildActive(i);
    }

    private void OnEnable()
    {
        Exit.onClick.AddListener(() => SaveLoadUtility.LoadLevel(0));
        Exit.onClick.AddListener(() => Time.timeScale = 1); 
        Resume.onClick.AddListener(() => OnPause()); 
    }

    private void OnDisable()
    {
        Exit.onClick.RemoveAllListeners(); 
        Setting.onClick.RemoveAllListeners(); 
        Resume.onClick.RemoveAllListeners(); 
    }

}
