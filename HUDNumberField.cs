using System.Collections.Generic;
using UnityEngine;

namespace BDMk22Plugin
{
    public class HudNumberField
    {
        public enum HudNumberAlign
        {
            Left,
            Right
        }

        private readonly HudNumberAlign _alignment;
        private readonly float _charPixelWidth;

        private readonly int[] _digits;

        private readonly int _maxValue;
        private readonly UvTransformer[] _uvTs;

        private float _charWidth;

        private int _currValue = -1;
        private Transform _transform;

        public GameObject[] DigitObjects;

        public HudNumberField(Transform transform, int digitCount, float scale, GameObject digitReference,
            float charWidth, float charPixelWidth, HudNumberAlign alignment)
        {
            _transform = transform;
            _charWidth = charWidth;
            _charPixelWidth = charPixelWidth;
            _alignment = alignment;
            DigitCount = digitCount;

            DigitObjects = new GameObject[digitCount];
            _uvTs = new UvTransformer[digitCount];
            _digits = new int[digitCount];
            for (var i = 0; i < digitCount; i++)
            {
                var digitObject = Object.Instantiate(digitReference);
                DigitObjects[i] = digitObject;
                digitObject.transform.parent = transform;
                digitObject.transform.localScale = Vector3.one;
                digitObject.transform.localPosition = Vector3.zero;
                digitObject.transform.localRotation = Quaternion.identity;
                digitObject.SetActive(true);
                if (alignment == HudNumberAlign.Right)
                    digitObject.transform.localPosition -= charWidth*digitCount*Vector3.right;
                digitObject.transform.localPosition += charWidth*i*Vector3.right;

                _uvTs[i] = new UvTransformer(digitObject);

                transform.localScale = scale*Vector3.one;

                _digits[i] = -1;
            }

            var maxValChars = new char[digitCount];
            for (var i = 0; i < digitCount; i++)
                maxValChars[i] = '9';
            var maxValString = new string(maxValChars);
            _maxValue = int.Parse(maxValString);
        }

        public int DigitCount { get; }

        public void Destroy()
        {
            if (DigitObjects == null) return;

            for (var i = 0; i < DigitObjects.Length; i++)
            {
                if (DigitObjects[i])
                {
                      Object.Destroy(DigitObjects[i]);
                }                  
            }
              
        }

        public void SetValue(int val)
        {
            val = Mathf.Clamp(val, -1, _maxValue);

            if (_currValue != val)
            {
                //Debug.Log ("Setting value: "+val);
                _currValue = val;

                if (val < 0)
                {
                    for (var i = 0; i < DigitCount; i++)
                    {
                         SetDigit(-1, i);
                    }                      
                }
                  

                int[] digits;
                if (val == 0)
                {
                    digits = new[] {0};
                }
                else
                {
                    var listOfInts = new List<int>();
                    var num = val;
                    while (num > 0)
                    {
                        listOfInts.Add(num%10);
                        num = num/10;
                    }
                    listOfInts.Reverse();
                    digits = listOfInts.ToArray();
                }

                var valDigitCount = digits.Length; //intString.Length;
                var extraChars = DigitCount - valDigitCount;

                if (_alignment == HudNumberAlign.Right)
                {
                    //set extra spaces to blank
                    for (var i = 0; i < extraChars; i++)
                        SetDigit(-1, i);

                    for (var i = 0; i < valDigitCount; i++)
                    {
                        var digit = digits[i]; //int.Parse(intString.Substring(i, 1));	
                        var index = extraChars + i;
                        SetDigit(digit, index);
                    }
                }
                else
                {
                    for (var i = 0; i < valDigitCount; i++)
                    {
                        var digit = digits[i]; //int.Parse(intString.Substring(i, 1));	
                        SetDigit(digit, i);
                    }

                    for (var i = 0; i < extraChars; i++)
                    {
                        var index = valDigitCount + i;
                        SetDigit(-1, index);
                    }
                }
            }
        }

        private void SetDigit(int digit, int index)
        {
            if (digit != _digits[index])
            {
                _uvTs[index].UpdateUvTransformation(new Vector2(_charPixelWidth*(digit + 1), 0), 0, Vector2.zero,
                    Vector2.zero);
                _digits[index] = digit;
            }
        }
    }
}