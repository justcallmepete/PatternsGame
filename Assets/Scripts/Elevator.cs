#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using EazyTools.SoundManager;

/*
 * Elevator script opens the elevator if the player has a keycard. 
 * Use on elivator object. 
 */ 

public class Elevator : Interactable {

    private Animator anim;
    public Animator Anim { get { return anim; } }
    private bool locked = true;

    [Header("Audio Setting")]
    public AudioClip openSFX;
    public AudioClip closeSFX;
    public AudioClip arriveSFX;
    public AudioClip departSFX;
    public AudioClip lockedSFX;
    private AudioClip lockedSFXId;

    public bool isExitElevator = true;
    GameObject[] players;
    Vector3[] playerPositions;
    GameObject floor;

    private ElevatorDoor[] listOfDoors;

    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponent<Animator>();

        listOfDoors = GetComponentsInChildren<ElevatorDoor>();

        if (!isExitElevator) {
            players = GameObject.FindGameObjectsWithTag("Player");
            playerPositions = new Vector3[2];
            floor = GameObject.FindGameObjectWithTag("Floor");
            floor.GetComponent <BoxCollider>().enabled = false;
            for (int i = 0; i < players.Length; i++)
            {
                playerPositions[i] = players[i].transform.position;
                players[i].transform.position = new Vector3(playerPositions[i].x, playerPositions[i].y - 10, playerPositions[i].z);
            }
            anim.runtimeAnimatorController = Resources.Load("Animations/Elevator_arrive", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }
        else
        {
            anim.runtimeAnimatorController = Resources.Load("Animations/Elevator_depart", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }
        Debug.Log("anim.runtimeAnimatorController: " + anim.runtimeAnimatorController.ToString());
	}

    public override void OnInteract(GameObject obj)
    {
        if (obj.GetComponent<MainPlayer>().inventory.Keycard || !locked)
        {
            UnlockElevator();
            Open();

            return;
        }
        else
        {
            PlayLockedSFX();
        }
        Debug.Log("Elevator is Locked");
    }

    public void Open()
    {
        anim.SetBool("doOpen", true);
    }

    public void UnlockElevator()
    {
        locked = false;
    }

    public void ElevatorArrived()
    {       
        floor.GetComponent<BoxCollider>().enabled = true;
    }

    public void DepartElevator()
    {
        if (isExitElevator)
        {
            anim.SetBool("doOpen", false);
        }
    }

    public void LoadNextScene()
    {
        GameManager.Instance.LoadNextScene();
    }

    public void ChangeOutlineSlidingDoor(bool eraseRenderer)
    {
        for (int i = 0; i < listOfDoors.Length; i++)
        {
            listOfDoors[i].outline.eraseRenderer = eraseRenderer;
        }
    }

    public void PlayOpenSFX()
    {
        int id = SoundManager.PlaySound(openSFX, 0.4f, false, gameObject.transform);
        Audio open = SoundManager.GetAudio(id);
        open.audioSource.rolloffMode = AudioRolloffMode.Custom;
        open.Set3DDistances(2f, 15f);
        open.audioSource.spatialBlend = 1f;
    }

    public void PlayCloseSFX()
    {
        int id = SoundManager.PlaySound(openSFX, 0.4f, false, gameObject.transform);
        Audio open = SoundManager.GetAudio(id);
        open.audioSource.rolloffMode = AudioRolloffMode.Custom;

        open.Set3DDistances(2f, 15f);
        open.audioSource.spatialBlend = 1f;
    }

    public void PlayArriveSFX()
    {
        int id = SoundManager.PlaySound(arriveSFX, 0.4f, false, gameObject.transform);
        Audio open = SoundManager.GetAudio(id);
        open.audioSource.rolloffMode = AudioRolloffMode.Custom;
    }

    public void PlayDepartSFX()
    {   
        int id = SoundManager.PlaySound(departSFX, 0.4f, false, gameObject.transform);
        Audio open = SoundManager.GetAudio(id);
        open.audioSource.rolloffMode = AudioRolloffMode.Custom;
    }

    public void PlayLockedSFX()
    {
        Audio sound = SoundManager.GetAudio(lockedSFX);

        if (sound == null)
        {
            SoundManager.PlaySound(lockedSFX, 0.4f, false, gameObject.transform);

            sound = SoundManager.GetAudio(lockedSFX);
            sound.Set3DSettings();
            return;
        }

        if (sound.playing)
        {
            return;
        }

        sound.Play();
        sound.Set3DSettings();
    }
}
