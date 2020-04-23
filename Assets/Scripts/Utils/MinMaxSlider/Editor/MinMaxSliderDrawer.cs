using UnityEngine;
using UnityEditor;

namespace Ecosystem.MinMaxSlider
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var minMaxAttribute = (MinMaxSliderAttribute)attribute;
            var propertyType = property.propertyType;

            label.tooltip = minMaxAttribute.min.ToString("F2") + " to " + minMaxAttribute.max.ToString("F2");

            //PrefixLabel returns the rect of the right part of the control. It leaves out the label section. We don't have to worry about it. Nice!
            Rect controlRect = EditorGUI.PrefixLabel(position, label);

            Rect[] splitRect = SplitRect(controlRect, 3);
            
            EditorGUI.BeginChangeCheck();

            Vector2 vector = GetFromProperty(property);
            float minVal = vector.x;
            float maxVal = vector.y;

            float prevMinVal = minVal;
            float prevMaxVal = maxVal;

            minVal = EditorGUI.FloatField(splitRect[0], minVal);
            maxVal = EditorGUI.FloatField(splitRect[2], maxVal);

            EditorGUI.MinMaxSlider(splitRect[1], ref minVal, ref maxVal,
                minMaxAttribute.min, minMaxAttribute.max);

            bool changedMin = Mathf.Abs(minVal - prevMinVal) > Mathf.Abs(maxVal - prevMaxVal);

            // F2 limits the float to two decimal places (0.00).
            minVal = float.Parse(minVal.ToString("F2"));
            maxVal = float.Parse(maxVal.ToString("F2"));

            if (changedMin) minVal = Mathf.Clamp(minVal, minVal, maxVal);
            else maxVal = Mathf.Clamp(maxVal, minVal, maxVal);

            if (EditorGUI.EndChangeCheck())
            {
                WriteToProperty(property, minVal, maxVal);
            }
        }

        private static Vector2 GetFromProperty(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                return property.vector2Value;
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                return property.vector2IntValue;
            }

            return default;
        }

        private static void WriteToProperty(SerializedProperty property, float min, float max)
        {
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                property.vector2Value = new Vector2(min, max);
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                property.vector2IntValue = new Vector2Int(Mathf.RoundToInt(min), Mathf.RoundToInt(max));
            }
        }

        private static Rect[] SplitRect(Rect rectToSplit, int n)
        {
            Rect[] rects = new Rect[n];

            for (int i = 0; i < n; i++)
            {

                rects[i] = new Rect(rectToSplit.position.x + (i * rectToSplit.width / n), rectToSplit.position.y, rectToSplit.width / n, rectToSplit.height);

            }

            int padding = (int)rects[0].width - 40;
            int space = 5;

            rects[0].width -= padding + space;
            rects[2].width -= padding + space;

            rects[1].x -= padding;
            rects[1].width += padding * 2;

            rects[2].x += padding + space;


            return rects;
        }
    }
}
