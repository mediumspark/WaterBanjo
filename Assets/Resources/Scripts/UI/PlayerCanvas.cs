using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public class PlayerCanvas : MonoBehaviour
{
    private static Player _PlayerSave;
    
    public Image NozzleType;
    public TextMeshProUGUI Coins;

    public GameObject HealthPrefab;
    public HorizontalLayoutGroup PlayerHealthContainer; 

    private static TextMeshProUGUI _CoinText;
    private static Image _NozzleType;
    private static GameObject HPF;
    private static HorizontalLayoutGroup PHC;

    [SerializeField]
    private List<Image> halfHearts;
    private static List<Image> HalfHearts = new List<Image>(); 

    private void Awake()
    {
        _PlayerSave = PlayerScriptableReference.PlayerSO;
        _CoinText = Coins;
        _NozzleType = NozzleType;
        HPF = HealthPrefab;
        PHC = PlayerHealthContainer;
        UpdateTotalHearts();
        UpdateUI();
        new GameObject().AddComponent< PlayerLocationCacheObject>();

    }

    private void Update()
    {
        halfHearts = HalfHearts;
    }

    public static void UpdateUI()
    {
        UpdateCoinText();
        UpdateHealth();
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

    public static void UpdateTotalHearts()
    {
        HalfHearts.Clear(); 
        for (int i = 0; i < _PlayerSave.Hearts; i++)
        {
           Image Instant =  Instantiate(HPF, PHC.transform).GetComponent<Image>();
           HalfHearts.Add(Instant.transform.GetChild(0).gameObject.GetComponent<Image>());
           HalfHearts.Add(Instant.transform.GetChild(1).gameObject.GetComponent<Image>());
        }
        //Update how many hearts are damaged
    }


    public static void UpdateHealth()
    {
        int MaxHealth = _PlayerSave.Hearts * 2;
        int CurrentHealth = _PlayerSave.Health;

        foreach(Image i in HalfHearts)
        {
            i.color = Color.white; 
        }

        for (int i = MaxHealth - 1; i >= CurrentHealth; i--)
        {
            HalfHearts[i].color = Color.red;
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