using System;
using UnityEngine;

namespace Scenes.ChapterMenu
{
    /// <summary>
    /// Having your SpriteRenderer fit to Camera,
    /// Useful to make background. 
    /// </summary>
    [ExecuteAlways]
    public class SpriteFittingCamera : MonoBehaviour
    {
        public SpriteRenderer _spriteRenderer;
        public Camera _camera;

        private bool GoFit()
        {
            if(_camera == null || _spriteRenderer == null)
                return false;
            
            var height = _camera.orthographicSize * 2;
            var width = height * Screen.width / Screen.height;
            gameObject.transform.localScale = new Vector3(width, height, 1f);
            
            return true;
        }

        private void TryAssign()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (GoFit() == false)
            {
                TryAssign();
            }
        }

        private void Start()
        {
            if (GoFit() == false)
            {
                TryAssign();
            }
        }
    }
}
