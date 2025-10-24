using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PictureMode : MonoBehaviour
{
    public bool rightClickPressed = false;
    public CinemachineCamera pictureCam;

    [SerializeField] private float zoomedFOV = 30f;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private Camera outputCam;
    [SerializeField] private Texture2D defaultPic;
    [SerializeField] private RawImage crosshair;

    public int photoWidth = 512;
    public int photoHeight = 512;

    [HideInInspector]public List<Texture2D> photoAlbum = new List<Texture2D>();
    private RenderTexture renderTex;
    
    public int availablePhotos = 15;
    public float roundTime = 15f;
    public TextMeshProUGUI roundTimeText;
    public TextMeshProUGUI remainingText;
    [HideInInspector]public int remainingPhotos;
    public GameObject gameOver;
    public PauseGameManager pauseManager;
    private bool endRound;

    private void Start()
    {
        remainingPhotos = availablePhotos;
        remainingText.text = remainingPhotos + " Photos Remaining";
        renderTex = new RenderTexture(photoWidth, photoHeight, 24);
        outputCam = Camera.main;
    }

    private Texture2D CaptureFromCamera(Camera cam, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;
        Texture2D image = new Texture2D(width, height, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt); // cleanup

        return image;
    }

    public void TakePhoto()
    {
        if(Time.timeScale < 1)
            return;
        int photoWidth = 512; // You can make this adjustable if needed

        // Calculate height based on current screen aspect ratio
        float aspectRatio = (float)Screen.height / Screen.width;
        int photoHeight = Mathf.RoundToInt(photoWidth * aspectRatio);

        Texture2D photo = CaptureFromCamera(outputCam, photoWidth, photoHeight);
        photoAlbum.Add(photo);

        remainingPhotos--;
        remainingText.text = remainingPhotos + " Photos Remaining";

        Debug.Log($"Photo taken! {photoWidth}x{photoHeight} | Total: {photoAlbum.Count}");
    }


    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rightClickPressed = true;
        }
        if (context.canceled)
        {
            rightClickPressed = false;
        }
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed && rightClickPressed)
        {
            TakePhoto();
        }
    }

    private void Update()
    {
        roundTime = Mathf.Clamp(roundTime - Time.deltaTime, 0, roundTime);
        roundTimeText.text = roundTime.ToString("0.00");
        if ((roundTime <= 0 || remainingPhotos <= 0)&& !endRound)
        {
            endRound = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true;
            gameOver.SetActive(true);
            pauseManager.canPause = false;
        }
        
        if (rightClickPressed)
        {
            Color c = crosshair.color;
            c.a = 1f;
            crosshair.color = c;
        }
        else
        {
            Color c = crosshair.color;
            c.a = 0f;
            crosshair.color = c;
        }
            float targetFOV = rightClickPressed ? zoomedFOV : normalFOV;

        // Smoothly interpolate toward the target FOV
        pictureCam.Lens.FieldOfView = Mathf.Lerp(
            pictureCam.Lens.FieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );
    }
    public Texture2D getLastestPhoto()
    {
        if (photoAlbum.Count > 0)
        {
            return photoAlbum[photoAlbum.Count - 1];
        }
        else
        {
            
            return defaultPic;
        }
    }
}
