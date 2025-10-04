using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Audio;
[CreateAssetMenu(fileName ="NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;//Mark where dialogue ends
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.5f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;

    public DialogueChoice[] choices;
}

[System.Serializable]

public class DialogueChoice
{
    public int dialogueIndex;//Dialogue Line where choices apper
    public string[] choices;//Player response options
    public int[] nextDialogueIndexes;//Where choice leads
}