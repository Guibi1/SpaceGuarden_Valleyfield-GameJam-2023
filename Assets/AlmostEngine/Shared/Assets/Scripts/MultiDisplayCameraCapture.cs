using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlmostEngine
{
    /// Add this component to the camera you want to capture in multi display.
    /// This component is also automatically added to the main camera to wait for the end of the render pass and copy the framebuffer content in multi display mode.
    /// It is used only on multi-display settings.
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class MultiDisplayCameraCapture : MonoBehaviour
    {
        public static List<MultiDisplayCameraCapture> m_MultiDisplayCamera = new List<MultiDisplayCameraCapture>();

        void OnEnable()
        {
            m_MultiDisplayCamera.Add(this);
        }

        void OnDisable()
        {
            m_MultiDisplayCamera.Remove(this);
        }


        Texture2D m_TargetTexture;
        bool m_WaitForRendering = false;

        public void WaitRenderingAndCopyCameraToTexture(Texture2D targetTexture)
        {
            m_TargetTexture = targetTexture;
            m_WaitForRendering = true;
        }

        public bool CopyIsOver()
        {
            return !m_WaitForRendering;
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest);

            if (m_WaitForRendering && m_TargetTexture != null)
            {
                m_TargetTexture.ReadPixels(new Rect(0, 0, m_TargetTexture.width, m_TargetTexture.height), 0, 0);
                m_TargetTexture.Apply(false);

                m_WaitForRendering = false;
            }

        }

    }
}