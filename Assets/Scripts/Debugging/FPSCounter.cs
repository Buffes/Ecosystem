using TMPro;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_UI = default;
        [SerializeField]
        private TMP_Text m_Text = default;

        private float fixedDeltaTime;
        private int FPS;

        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        const string display = "{0} FPS";


        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            this.fixedDeltaTime = Time.fixedDeltaTime;
        }


        private void Update()
        {
            if (Input.GetKeyDown("f"))
            {
                m_UI.SetActive(!m_UI.activeSelf);
            }
            if (Input.GetKeyDown("g"))
            {
                if (Time.timeScale == 1.0f)
                    Time.timeScale = 10.0f;
                else
                    Time.timeScale = 1.0f;
            }
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                m_Text.text = string.Format(display, m_CurrentFps);
            }


        }
    }
}
