using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System.IO;
using DeepAction;
using Sirenix.Serialization;
using Newtonsoft.Json.Serialization;

public class SaveTester : MonoBehaviour
{
    public string profile;
    public AbilityObject abilityRef;

    [ShowInInspector,ReadOnly]
    public DeepBehavior activeSave;

    public JsonSerializerSettings settings;

    [ShowInInspector,ReadOnly]
    private Ability redAbility;

    private JsonSerializerSettings GetSettings()
    {
        JsonSerializerSettings set = new JsonSerializerSettings();

        set.Formatting = Formatting.Indented;
        set.TypeNameHandling = TypeNameHandling.Auto;
        set.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

        return set;
    }


    [Button]
    private void UpdateSave()
    {
        activeSave = abilityRef.ability.behavior;
    }


    [Button]
    private void OdinSave()
    {
        byte[] bytes = SerializationUtility.SerializeValue(activeSave,DataFormat.JSON);
        File.WriteAllBytes(Application.persistentDataPath + Path.DirectorySeparatorChar + (profile + "_odn" + ".json"),bytes);
    }

    [Button]
    private void LoadFileFromDisk()
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + (profile + ".json")))
        {
            string data = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + (profile + ".json"));

            activeSave = JsonConvert.DeserializeObject<DeepBehavior>(data,GetSettings());

            //activeSave.gridformData = news.gridformData;
            //.settings = news.settings;
        }
        else
        {
            //failed to load. Make new save.
            CreateNewFileOnDisk();
        }
    }
    [Button]
    private void CreateNewFileOnDisk()
    {
        activeSave = new DeepBehavior();

        SaveFileToDisk();
    }


    [Button]
    public void SaveAbilityFileToDisk()
    {

        Ability a = abilityRef.ability;
        

        string result = JsonConvert.SerializeObject(a,GetSettings());

        string savePath = Application.persistentDataPath;

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            File.WriteAllText(savePath + Path.DirectorySeparatorChar + (profile + "_ability"+ ".json"),result);
        }
        else
        {
            File.WriteAllText(savePath + Path.DirectorySeparatorChar + (profile + ".json"),result);
        }
    }
    [Button]
    public void SaveFileToDisk()
    {
        if (activeSave == null)
        {
            Debug.LogError("You tried to save to disk when there was no active save file. a new file was created and saved to disk.");
            CreateNewFileOnDisk();
            return;
        }

        

        string result = JsonConvert.SerializeObject(activeSave,GetSettings());

        string savePath = Application.persistentDataPath;

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            File.WriteAllText(savePath + Path.DirectorySeparatorChar + (profile + ".json"),result);
        }
        else
        {
            File.WriteAllText(savePath + Path.DirectorySeparatorChar + (profile + ".json"),result);
        }
    }
    [Button]
    public void LoadAbilityToRef()
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + (profile + ".json")))
        {
            string data = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + (profile + ".json"));

            redAbility = JsonConvert.DeserializeObject<Ability>(data,GetSettings());

            //activeSave.gridformData = news.gridformData;
            //.settings = news.settings;
        }
    }

    [Button]
    public void NullAbility()
    {
        redAbility = null;
    }

}
