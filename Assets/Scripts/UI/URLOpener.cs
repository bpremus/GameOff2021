using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLOpener : MonoBehaviour
{
   public void OpenWebsite(string url)
    {
        Application.OpenURL(url);
    }
}

