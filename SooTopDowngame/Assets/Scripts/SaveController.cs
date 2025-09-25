using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;
    private Chest[] chests;
    void Start()
    {
        InitializeComponents();
        LoadGame();
    }

    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindAnyObjectByType<InventoryController>();
        hotbarController = FindAnyObjectByType<HotbarController>();
        chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
    }
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems(),
            chestSaveData = GetChestsState()
        };
        File.WriteAllText(saveLocation,JsonUtility.ToJson(saveData));
    }

    private List<ChestSaveData> GetChestsState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();

        foreach (Chest chest in chests)
        {
            ChestSaveData chestSave = new ChestSaveData
            {
                chestID = chest.ChestID,
                isOpened = chest.IsOpened,
            };
            chestStates.Add(chestSave);
        }
        return chestStates;
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            PolygonCollider2D savedMapBoundry = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
            FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D = savedMapBoundry;

            MapController_Manual.Instance?.HighlightArea(saveData.mapBoundary);
            MapController_Dynamic.Instance?.GenerateMap(savedMapBoundry);

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);

            //LoadChestState
            LoadChestStates(saveData.chestSaveData);
        }
        else
        {
            SaveGame();

            inventoryController.SetInventoryItems(new List<InventorySaveData>());
            hotbarController.SetHotbarItems(new List<InventorySaveData>());

            MapController_Dynamic.Instance?.GenerateMap();
        }
    }

    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach(Chest chest in chests)
        {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);
            if (chestSaveData != null)
            {
                chest.SetOpened(chestSaveData.isOpened);
            }
        }
    }
}
