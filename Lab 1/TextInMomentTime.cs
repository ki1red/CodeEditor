using Lab_1.AuxiliaryClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public struct TextInMomentTime
    {
        public string Text { get; private set; }
        public int CursorPosition { get; private set; }
        public TextInMomentTime(string text, int cursorPosition)
        {
            Text = text;
            CursorPosition = cursorPosition;
        }
        public static bool operator ==(TextInMomentTime firstText, TextInMomentTime secondText)
        {
            if (firstText.Text == secondText.Text)
            {
                if (secondText.CursorPosition == firstText.CursorPosition)
                    return true;
            }
            return false;
        }
        public static bool operator !=(TextInMomentTime firstText, TextInMomentTime secondText)
        {
            return !(firstText == secondText);
        }
    }
}
