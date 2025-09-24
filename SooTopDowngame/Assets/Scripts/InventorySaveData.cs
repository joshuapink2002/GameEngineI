using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Audio;

[System.Serializable]
public class InventorySaveData
{
    public int itemID;
    public int slotIndex; //The index of the slot where the item is placed
}
