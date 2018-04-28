using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PermissionsManager : MonoBehaviour {

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
        SceneManager.LoadScene(1);
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
                SharedImageData sid = new SharedImageData();
                sid.bytes = tex.EncodeToPNG();
                string finalPath = SerializationManager.CreatePath(campaignName + "/NewMap.map");
                SerializationManager.SaveObject(finalPath, sid);
            }
        }, "Select a PNG image", "image/png",4096);

        Debug.Log("Permission result: " + permission);
    }
}
