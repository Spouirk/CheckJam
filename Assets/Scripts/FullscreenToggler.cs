using UnityEngine;
using UnityEngine.Device;

public class FullscreenToggler : MonoBehaviour
{
    public void toggleFullscreen()
    {
        UnityEngine.Screen.fullScreen = !UnityEngine.Screen.fullScreen;
    }
}
