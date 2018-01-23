using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class LevelCreator : MonoBehaviour {
    Dictionary<GameObject, UndoRedoState> tempUndoList;
    MaxStack<Dictionary<GameObject, UndoRedoState>> undoStack = new MaxStack<Dictionary<GameObject, UndoRedoState>>(10);
    MaxStack<Dictionary<GameObject, UndoRedoState>> redoStack = new MaxStack<Dictionary<GameObject, UndoRedoState>>(10);

    public enum UndoRedoState {Instantiated, Destroyed };

    public void Undo()
    {
        Dictionary<GameObject, UndoRedoState> groupToUndo = undoStack.Pop();
        if(groupToUndo == null)
        {
            return;
        }
        redoStack.Push(groupToUndo);
        foreach (KeyValuePair<GameObject, UndoRedoState> pair in groupToUndo)
        {
            if (UndoRedoState.Instantiated.Equals(pair.Value))
            {
                pair.Key.gameObject.SetActive(false);
            }
            else if (UndoRedoState.Destroyed.Equals(pair.Value))
            {
                pair.Key.gameObject.SetActive(true);
            }
        }
    }

    public void Redo()
    {
        Dictionary<GameObject, UndoRedoState> groupToRedo = redoStack.Pop();
        if (groupToRedo == null)
        {
            return;
        }
        undoStack.Push(groupToRedo);
        foreach (KeyValuePair<GameObject, UndoRedoState> pair in groupToRedo)
        {
            if (UndoRedoState.Instantiated.Equals(pair.Value))
            {
                pair.Key.gameObject.SetActive(true);
            }
            else if (UndoRedoState.Destroyed.Equals(pair.Value))
            {
                pair.Key.gameObject.SetActive(false);
            }
        }
    }

    public void AddToUndo(Dictionary<GameObject, UndoRedoState> undoGroup)
    {
        redoStack.Clear();
        Dictionary<GameObject, UndoRedoState> groupToRemove = undoStack.Push(undoGroup);
        if (groupToRemove == null)
        {
            return;
        }
        RemoveFromUndoStack(groupToRemove);
    }

    void AddToRedo(Dictionary<GameObject, UndoRedoState> undoGroup)
    {
        redoStack.Push(undoGroup);
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
