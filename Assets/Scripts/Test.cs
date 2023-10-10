using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public Transform from;
    public Transform to;

    public GameFileData GameFileData;

    public Canvas Canvas; 
    
    public TestData data;
    public string path = "playerData.sav";
    // Start is called before the first frame update
    void Start()
    {
        //GameObject obj = Instantiate(ResourceManager.Instance.GetGameObject("Operation_Panel", ResourceType.UI), Canvas.transform);
       //ResourceManager.Instance.GetGameObject("Operation_Panel", ResourceType.UI, Canvas.transform);
       //ResourceManager.Instance.GetGameObject("Settings_Panel", ResourceType.UI, Canvas.transform);
       
       //SceneLoadingManager.Instance.ASyncLoadingScene(SceneType.MainMenuScene);
       /*AsyncOperation operation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
       if (operation == null)
       {
           Debug.Log("!!");
       }*/
       PlayerPrefs.DeleteAll();
       
       string fileName = System.DateTime.Now.ToString();
       fileName = fileName.Replace('/', '-');
       fileName = fileName.Replace(':', '_');
       Debug.Log(fileName);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Angle(to.forward, from.forward));
        
        //Debug.Log(Vector3.SignedAngle(to.forward, from.forward, Vector3.up));
        //Debug.Log(Vector3.Angle(Direction,to.forward));

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveManager.Instance.Save(path,data);
            Debug.Log("Successfully Save Data");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TestData loadData = SaveManager.Instance.Load<TestData>(path);
            data.nickName = loadData.nickName;
            data.mPosition = loadData.mPosition;
        }*/
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    GameFileData gameFileData = new GameFileData();
        //    gameFileData.time = System.DateTime.Now.ToString();
        //    string sprintTime = System.DateTime.Now.ToString().Replace('/', '-').Split(' ')[0];
        //    string fileName = SystemDefine.filePath+sprintTime + ".sav";
        //    Debug.Log(fileName);
        //    SaveManager.Instance.SaveGameData(fileName,gameFileData);
        //}
        /*
         * {"time":"10/02/2023 15:16:38","scene":1,"playerStatsManager":{"instanceID":-20436},"playerInventoryManager":{"instanceID":-20440},"position":{"x":-0.5189199447631836,"y":0.0,"z":1.713639736175537},"rotation":{"x":0.0,"y":-0.2090572863817215,"z":0.0,"w":0.977903425693512}}
         */
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            string jsonData =
                @"{""time"":""10/02/2023 16:34:18"",""scene"":1,""statsData"":{""healthLevel"":10,""staminaLevel"":10,""focusLevel"":10,""poiseLevel"":10,""strengthLevel"":10,""dexterityLevel"":10,""intelligenceLevel"":10,""faithLevel"":10,""currentHealth"":100,""currentStamina"":100.0,""currentFocusPoints"":100.0},""inventoryData"":{""weaponInventory"":[],""currentHelmetEquipment"":{""instanceID"":24886},""currentTorsoEquipment"":{""instanceID"":25000},""currentHipEquipment"":{""instanceID"":25084},""currentHandEquipment"":{""instanceID"":25246},""weaponsInRightHandSlots"":[{""instanceID"":25372},{""instanceID"":0}],""weaponsInLeftHandSlots"":[{""instanceID"":25318},{""instanceID"":0}],""currentRightWeaponIndex"":0,""currentLeftWeaponIndex"":0},""position"":{""x"":-0.5772343873977661,""y"":0.0,""z"":1.7029285430908204},""rotation"":{""x"":0.0,""y"":-0.18262706696987153,""z"":0.0,""w"":0.9831823110580444}}";
            GameFileData = JsonUtility.FromJson<GameFileData>(jsonData);
        }*/
    }
}
