using UnityEngine;
using UnityEngine.UI;

public class showLatestPhoto : MonoBehaviour
{
    [SerializeField] private PictureMode pics;
    RawImage rawImage;
    private void Start()
    {
        rawImage = GetComponent<RawImage>();
    }
    // Update is called once per frame
    void Update()
    {
        Texture2D photo = pics.getLastestPhoto();
        if (photo != null)
        {
            rawImage.texture = photo;
        }
    }
}
