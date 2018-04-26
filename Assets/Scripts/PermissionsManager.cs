using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermissionsManager : MonoBehaviour {

    void CheckPermissions() {
        if (NativeGallery.CheckPermission() == NativeGallery.Permission.ShouldAsk) {
            NativeGallery.RequestPermission();
        }
    }

    public void GetGalleryImage(string cN) {
        CheckPermissions();
        if (NativeGallery.IsMediaPickerBusy())
            return;
        PickImage(cN);
    }

    private void PickImage(string campaignName) {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {
            Debug.Log("Image path: " + path);
            if (path != null) {
                // Create Texture from selected image
                Texture2D tex = NativeGallery.LoadImageAtPath(path);
                if (tex == null) {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                SharedImageData sid = new SharedImageData();
                sid.bytes = tex.EncodeToPNG();
                string finalPath = SerializationManager.CreatePath(campaignName + "/NewMap");
                SerializationManager.SaveObject(finalPath, sid);
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
    }
}
