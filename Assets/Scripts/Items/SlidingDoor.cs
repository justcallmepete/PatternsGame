using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

public class SlidingDoor : MonoBehaviour
{
    float timer;
    public float openTime = 3;

    public bool lockedWithSwitch = false;
    public bool LockedWithSwitch { get { return lockedWithSwitch; } set { lockedWithSwitch = value; } }
    private Animator animator;

    private SlidingDoorSingle[] listOfDoors;
    [Header("Audio Setting")]
    public AudioClip openSFX;
    public AudioClip closeSFX;
    public AudioClip lockedSFX;

    private int openSFXId;
    private int closeSFXId;
    private int lockedSFXId;

    [Header("This shows the player if the door is closed with a switch")]
    public GameObject[] lockedEffect;

    public enum State
    {
        Open,
        Closed,
    };
    private State currentState = State.Closed;

    // Use this for initialization
    void Start () {
        animator = GetComponentInParent<Animator>();

        listOfDoors = GetComponentsInChildren<SlidingDoorSingle>();
	}
	
	// Update is called once per frame
	void Update () {
        if (animator.GetBool("open")&&!lockedWithSwitch)
        {
            timer += Time.deltaTime;
            if(timer >= openTime)
            {
                animator.SetBool("open", false);
                timer = 0f; 
            }
        }

        if (lockedWithSwitch)
        {
            if (currentState == State.Closed)
            {
                for (int i = 0; i < lockedEffect.Length; i++)
                {
                    lockedEffect[i].SetActive(true);
                }                
            }
            else if(currentState == State.Open)
            {
                for (int i = 0; i < lockedEffect.Length; i++)
                {
                    lockedEffect[i].SetActive(false);
                }
            }
        } else
        {
            for (int i = 0; i < lockedEffect.Length; i++)
            {
                lockedEffect[i].SetActive(false);
            }
        }

    }

    public void OnInteract(GameObject obj)
    {
        if (lockedWithSwitch)
        {
            PlayLockedSFX();
            //Debug.Log("Door is locked by a switch");
            return;
        }

        switch (currentState)
        {
            case State.Closed:
                Open(3f);
                break;
            case State.Open:
                Close();
                break;
        }
    }

    public void Open(float pOpentime)
    {
        openTime = pOpentime;
        //open animation
        currentState = State.Open;
        animator.SetBool("open", true);
    }

    public void Close()
    {
        //close animation
        currentState = State.Closed;
        animator.SetBool("open", false);
    }

    public void ToggleOpenClosed()
    {
        if(currentState == State.Open)
        {
            //Lock door
            Close();
        }
        else
        {
            // Open door
            Open(Mathf.Infinity);
        }
    }

    public void ChangeOutlineSlidingDoor(bool eraseRenderer)
    {
        for (int i = 0; i < listOfDoors.Length; i++)
        {
            listOfDoors[i].outline.eraseRenderer = eraseRenderer;
        }
    }


    public void CameraOpenDoor()
    {
        lockedWithSwitch = false;
        Open(3f);
    }

    public void PlayOpenSFX()
    {
        Audio sound;

        if (openSFXId == 0)
        {
            openSFXId = SoundManager.PlaySound(openSFX, 0.4f, false, gameObject.transform);
            sound = SoundManager.GetAudio(openSFXId);
            sound.Set3DSettings();

            return;
        }
        sound = SoundManager.GetAudio(openSFXId);

        sound.Play();
        sound.Set3DSettings();
    }

    public void PlayCloseSFX()
    {
        Audio sound;

        if (closeSFXId == 0)
        {
            closeSFXId = SoundManager.PlaySound(closeSFX, 0.4f, false, gameObject.transform);
            sound = SoundManager.GetAudio(closeSFXId);
            sound.Set3DSettings();

            return;
        }
        sound = SoundManager.GetAudio(closeSFXId);

        sound.Play();
        sound.Set3DSettings();
    }

    public void PlayLockedSFX()
    {
        Audio sound;
        
        if (lockedSFXId == 0)
        {
            lockedSFXId = SoundManager.PlaySound(lockedSFX, 0.4f, false, gameObject.transform);
            sound = SoundManager.GetAudio(lockedSFX);
            sound.Set3DSettings();

            return;
        }

        sound = SoundManager.GetAudio(lockedSFXId);

        if (sound.playing)
        {
            return;
        }

        sound.Play();
        sound.Set3DSettings();
    }
}
