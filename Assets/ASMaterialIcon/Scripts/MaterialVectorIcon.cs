using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace ASMaterialIcon
{
    [ExecuteInEditMode, Serializable]
    public class MaterialVectorIcon : Text
    {
        public static readonly string DefaultIcon = "\\ue84d";
        public static readonly string DefaultIconName = "3d_rotation";
        private static Dictionary<string, string> _icons;

        [SerializeField]
        private string _iconName = "";
        public string iconName
        {
            get
            {
                if (string.IsNullOrEmpty(this._iconName))
                {
                    this._iconName = DefaultIconName;
                }
                return this._iconName;
            }
            set
            {
                _iconName = value;
            }
        }


        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            this.font = Resources.Load<Font>("ASMaterialIcon/Fonts/MaterialIcons-Regular") as Font;
#endif
            this.alignment = TextAnchor.MiddleCenter;
            this.fontSize = (int)Mathf.Floor(Mathf.Min(this.rectTransform.rect.width, this.rectTransform.rect.height));
            if (string.IsNullOrEmpty(this.text))
            {
                this.text = Decode(DefaultIcon);
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            this.fontSize = (int)Mathf.Floor(Mathf.Min(this.rectTransform.rect.width, this.rectTransform.rect.height));
        }

        public static void LoadIconResource()
        {
            _icons.Clear();
            TextAsset txt = (TextAsset)Resources.Load("ASMaterialIcon/Fonts/MaterialIcons", typeof(TextAsset));
            string[] lines = txt.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

            string key, value;
            foreach (string line in lines)
            {
                if (!line.StartsWith("#") && line.IndexOf("=") >= 0)
                {
                    key = line.Substring(0, line.IndexOf("="));
                    if (!_icons.ContainsKey(key))
                    {
                        value = line.Substring(line.IndexOf("=") + 1,
                            line.Length - line.IndexOf("=") - 1);
                        _icons.Add(key, value);
                    }
                }
            }
        }

        public static Dictionary<string, string> GetIcons()
        {
            if (_icons == null)
            {
                _icons = new Dictionary<string, string>();
                LoadIconResource();
            }
            return _icons;
        }

        public static string Decode(string value)
        {
            return new Regex(@"\\u(?<Value>[a-zA-Z0-9]{4})").Replace(value, m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());
        }
    }
}
