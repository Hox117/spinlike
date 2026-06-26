using UnityEngine;

public class FullScreenButton : MonoBehaviour
{
    public void ToggleFullscreen()
    {
        if (Screen.fullScreen)
        {
            Screen.SetResolution(
                1280,
                720,
                FullScreenMode.Windowed
            );
        }
        else
        {
            Resolution r = Screen.currentResolution;

            Screen.SetResolution(
                r.width,
                r.height,
                FullScreenMode.FullScreenWindow
            );
        }
    }
}
