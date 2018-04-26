using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermissionsManager : MonoBehaviour {

    void CheckPermissions() {
        if (NativeGallery.CheckPermission() == NativeGallery.Permission.ShouldAsk) {
            NativeGallery.RequestPermission();
        }
    }

    void GetGalleryImage() {
        CheckPermissions();
        if (NativeGallery.IsMediaPickerBusy())
            return;

        if (Input.mousePosition.x < Screen.width * 2 / 3) {
            // Pick a PNG image from Gallery/Photos
            // If the selected image's width and/or height is greater than 512px, down-scale the image
            PickImage(512);
        }
    }

    private void PickImage(int maxSize) {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {
            Debug.Log("Image path: " + path);
            if (path != null) {
                // Create Texture from selected image
                Texture2D tex = NativeGallery.LoadImageAtPath(path, maxSize);
                if (tex == null) {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                SharedImageData sid = new SharedImageData();
                sid.bytes = tex.EncodeToPNG();
                string finalPath = SerializationManager.CreatePath("NewMap.mp");
                SerializationManager.SaveObject(finalPath, sid);
            }
        }, "Select a PNG image", "image/png", maxSize);

        Debug.Log("Permission result: " + permission);
    }
}
