using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LevelCreator : MonoBehaviour {
    List<Dictionary<GameObject, string>> tempUndoList;
    MaxStack<List<Dictionary<GameObject, string>>> undoStack;
    MaxStack<List<Dictionary<GameObject, string>>> redoStack;

    public void Undo()
    {

    }

    public void Redo()
    {

    }

    public void AddToUndo(List<Dictionary<GameObject,string>> undoGroup)
    {

    }
}
