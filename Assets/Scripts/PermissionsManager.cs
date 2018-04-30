using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PermissionsManager : MonoBehaviour {

    public Image map;

    /// <summary>
    /// Check if the suers has the proper android permissions to use the photo gallery
    /// </summary>
    void CheckPermissions()
    {
        if (NativeGallery.CheckPermission() == NativeGallery.Permission.ShouldAsk)
            NativeGallery.RequestPermission();
    }

    /// <summary>
    /// Allows the user to select an image from the gallery
    /// </summary>
    /// <param name="cN">Name of the campaign</param>
    public void GetGalleryImage(string cN)
    {
        //SceneManager.LoadScene(1);
        CheckPermissions();
        if (NativeGallery.IsMediaPickerBusy())
            return;
        PickImage(cN);
    }

    /// <summary>
    /// Select the image and process the request
    /// </summary>
    /// <param name="campaignName">name of the campaign</param>
    private void PickImage(string campaignName) {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => 
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D tex = NativeGallery.LoadImageAtPath(path,4096);
                if (tex == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                
                tex.Apply();
                var v = new Texture2D(2,2);
                v.LoadImage(tex.EncodeToPNG(), false);
                v.Apply();


                ///map.sprite = Sprite.Create(v, new Rect(0.0f, 0.0f, v.width, v.height), new Vector2(0.5f, 0.5f), 100.0f);

                SharedImageData sid = new SharedImageData();
                sid.bytes = v.EncodeToPNG();
                sid.info = null;
                string finalPath = SerializationManager.CreatePath(campaignName + "/NewMap.map");
                SerializationManager.SaveObject(finalPath, sid);
            }
        }, "Select a PNG image", "image/png",4096);

        Debug.Log("Permission result: " + permission);
    }
}
