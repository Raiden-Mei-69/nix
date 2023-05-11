using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.UI
{
    public class EnemyUI : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private EnemyBase m_Enemy;
        [Space(20)]

        [SerializeField] private float _timeShowCanvas = 3f;
        [SerializeField] private float _rotSpeed = 200f;
        [SerializeField] private Vector3 _offsetCanvas;
        [Space(20)]

        [SerializeField] internal Canvas canvas;
        [SerializeField] private Slider Health_bar;
        [SerializeField] private TextMeshProUGUI textName;
        Coroutine showCanvas;

        public void OnStart()
        {
            m_Enemy = GetComponentInParent<EnemyBase>();
            showCanvas = null;
            InitializeHealthBar();
            textName.text = m_Enemy.EnemyName;
            canvas.gameObject.SetActive(false);
        }

        private void InitializeHealthBar()
        {
            Health_bar.minValue = 0;
            Health_bar.maxValue = m_Enemy.MaxHealth;
            Health_bar.value = m_Enemy.Health;
        }

        internal IEnumerator TurnCanvasPlayer()
        {
            do
            {
                if (m_Enemy.player == null)
                    yield break;
                var lookPos = m_Enemy.player.transform.position - transform.position;
                lookPos.y = 0;
                lookPos.x -= _offsetCanvas.x;
                lookPos.z -= _offsetCanvas.z;
                var rotation = Quaternion.LookRotation(lookPos);
                canvas.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotSpeed);
                yield return null;
            } while (canvas.gameObject.activeSelf && m_Enemy.player != null);
        }

        internal IEnumerator ShowingCanvasTarget()
        {
            canvas.gameObject.SetActive(true);
            StartCoroutine(TurnCanvasPlayer());
            do
            {
                canvas.gameObject.SetActive(true);
                yield return null;
            } while (m_Enemy.facingPlayer);
            canvas.gameObject.SetActive(false);
        }

        private IEnumerator ShowCanvas()
        {
            canvas.gameObject.SetActive(true);
            if (showCanvas != null)
                StopCoroutine(showCanvas);
            StartCoroutine(TurnCanvasPlayer());
            yield return new WaitForSeconds(_timeShowCanvas);
            canvas.gameObject.SetActive(false);
        }

        internal void UpdateLife()
        {
            Health_bar.value = m_Enemy.Health;
            StartCoroutine(ShowCanvas());
        }
    }
}
