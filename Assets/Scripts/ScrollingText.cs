using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScrollingText : MonoBehaviour
    {
        public GameObject Prefab;

        protected Vector3 ScrollingTextPosition = new Vector3(0, 1.0f, 0);

        public void ShowText(string value)
        {
            var textPos = Camera.main.WorldToViewportPoint(transform.position + ScrollingTextPosition);

            var x = Mathf.Clamp(textPos.x, 0.05f, 0.95f);
            var y = Mathf.Clamp(textPos.y, 0.05f, 0.95f);

            var gui = (GameObject)Instantiate(Prefab, new Vector3(x, y, 0), Quaternion.identity);
            gui.guiText.text = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
