using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildGrid : MonoBehaviour
{
    public GridLayoutGroup layout;
    public PictureMode pictureMode;
    [Header("References")]
    public GameObject imagePrefab;  // Prefab with an Image component
    public Transform contentParent;
    
    [Header("Data")]
    public List<Texture2D> textures; // Your list of textures

   

    void Start()
    {
        PopulateGrid();
    }

    private void Update()
    {
        if (textures != pictureMode.photoAlbum)
        {
            textures = pictureMode.photoAlbum;
            PopulateGrid();
        }
    }

    public void PopulateGrid()
    {
       
            
        // Clear old children (if you want to refresh the grid)
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Texture2D tex in textures)
        {
            
            GameObject newItem = Instantiate(imagePrefab, contentParent);
            Image img = newItem.GetComponent<Image>();

            if (img != null)
            {
                // Convert Texture2D to a Sprite
                Sprite newSprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f)
                );
                img.sprite = newSprite;
            }
        }
        
          

        // Force the layout to update immediately (Unity sometimes delays it by a frame)
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent.GetComponent<RectTransform>());
    }

}
