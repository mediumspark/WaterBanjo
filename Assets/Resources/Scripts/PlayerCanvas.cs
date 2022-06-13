using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public class PlayerCanvas : MonoBehaviour
{
    private static Player _PlayerSave;
    
    public Image WaterSlider, NozzleType;
    public TextMeshProUGUI Coins;

    private static TextMeshProUGUI _CoinText;
    private static Image _NozzleType;


    private void Awake()
    {
        _PlayerSave = PlayerScriptableReference.PlayerSO;
        _CoinText = Coins;
        _NozzleType = NozzleType;

        UpdateUI();
        new GameObject().AddComponent< PlayerLocationCacheObject>();
    }


    public static void UpdateUI()
    {
        UpdateCoinText();
        UpdateNozzelImage(); 
    }

    private static void UpdateCoinText()
    {
        _CoinText.text = $"{_PlayerSave.Coins}";
    }

    private static void UpdateNozzelImage()
    {
        Hydro Player;

        try 
        {
            Player = FindObjectOfType<Hydro>();
            _NozzleType.sprite = Player.CurrentNozzleSprite; 
        }
        catch
        {
            Debug.LogError("Error Updating Nozzle Image");
        }
    }

    public class PlayerLocationCacheObject : MonoBehaviour
    {
        private static Vector3 _LocationCache;
        public static Vector3 LocationCache { set => _LocationCache = value; }

        private void OnDisable()
        {
            SaveLoadUtility.SavePlace(_PlayerSave,
                SceneManager.GetActiveScene(),
                _LocationCache);
        }
    }
}