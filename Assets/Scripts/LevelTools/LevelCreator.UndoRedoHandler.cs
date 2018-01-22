using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class LevelCreator : MonoBehaviour {
    Dictionary<GameObject, UndoRedoState> tempUndoList;
    MaxStack<Dictionary<GameObject, UndoRedoState>> undoStack;
    MaxStack<Dictionary<GameObject, UndoRedoState>> redoStack;

    public enum UndoRedoState {Instantiated, Destroyed };

    public void Undo()
    {
        Dictionary<GameObject, UndoRedoState> groupToUndo = undoStack.Pop();
        foreach (KeyValuePair<GameObject, UndoRedoState> pair in groupToUndo)
        {
            if (UndoRedoState.Instantiated.Equals(pair.Value))
            {
                pair.Key.gameObject.SetActive(false);
                //TO DO: Remove object or set to redo
            }
            else if (UndoRedoState.Destroyed.Equals(pair.Value))
            {
                pair.Key.gameObject.SetActive(true);
            }
        }
    }

    public void Redo()
    {

    }

    public void AddToUndo(Dictionary<GameObject, UndoRedoState> undoGroup)
    {
        Dictionary<GameObject, UndoRedoState> groupToRemove = undoStack.Push(undoGroup);
        if (groupToRemove != null)
        {
            RemoveFromUndoStack(groupToRemove);
        }
    }

    void RemoveFromUndoStack(Dictionary<GameObject, UndoRedoState> groupToRemove)
    {
        //TO DO: Destroy all 'destroyed'/disbaled objects
        foreach (KeyValuePair<GameObject, UndoRedoState> pair in groupToRemove)
        {
            if (UndoRedoState.Destroyed.Equals(pair.Value))
            {
                pair.Key.gameObject.SetActive(false);
            }
        }
    }
}
