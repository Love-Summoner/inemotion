using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace NuitrackSDK.Frame
{
    [AddComponentMenu("NuitrackSDK/Frame/Frame Mixer")]
    public class S16_FrameMixer : MonoBehaviour
    {
        public enum Mode
        {
            Cut,
            ReverseCut,
            Mul,
            Mix
        }

        [SerializeField, NuitrackSDKInspector] bool useStaticMainTexture;
        [SerializeField, NuitrackSDKInspector] Texture staticMainTexture;

        [SerializeField, NuitrackSDKInspector] bool useStaticMaskTexture;
        [SerializeField, NuitrackSDKInspector] Texture staticMaskTexture;

        [SerializeField] Mode mode = Mode.Cut;
        [SerializeField] UnityEvent<Texture> onFrameUpdate;

        RenderTexture renderTexture;
        Texture overrideMainTexture = null;
        Texture overrideMaskTexture = null;

        private bool isRandomSwitchActive = false;

        void Start()
        {
            StartCoroutine(SwitchModes());
        }

        void Update()
        {
            Texture mainTexture = useStaticMainTexture ? staticMainTexture : overrideMainTexture;
            Texture maskTexture = useStaticMaskTexture ? staticMaskTexture : overrideMaskTexture;

            if (mainTexture == null || maskTexture == null)
                return;

            switch (mode)
            {
                case Mode.Cut:
                    FrameUtils.TextureUtils.Cut(mainTexture, maskTexture, ref renderTexture);
                    break;
                case Mode.ReverseCut:
                    FrameUtils.TextureUtils.ReverseCut(mainTexture, maskTexture, ref renderTexture);
                    break;
                case Mode.Mul:
                    FrameUtils.TextureUtils.Mul(mainTexture, maskTexture, ref renderTexture);
                    break;
                case Mode.Mix:
                    FrameUtils.TextureUtils.MixMask(mainTexture, maskTexture, ref renderTexture);
                    break;
            }

            onFrameUpdate.Invoke(renderTexture);
        }

        IEnumerator SwitchModes()
        {
            // Initial wait of 20 seconds before random switching starts
            yield return new WaitForSeconds(2000f);

            // Toggle random switching
            isRandomSwitchActive = true;

            // Continue random switching at random intervals
            while (isRandomSwitchActive)
            {
                // Switch between Cut and ReverseCut
                mode = (mode == Mode.Cut) ? Mode.ReverseCut : Mode.Cut;

                // Random interval between 5 to 15 seconds
                float randomInterval = Random.Range(5f, 15f);
                yield return new WaitForSeconds(randomInterval);
            }
        }

        public void MainTexture(Texture texture)
        {
            overrideMainTexture = texture;
        }

        public void MaskTexture(Texture texture)
        {
            overrideMaskTexture = texture;
        }
    }
}