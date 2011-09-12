using System.Collections.Generic;

namespace slidePuzzle
{
    class SlidePanel
    {
        private int value;
        public char view;

        public SlidePanel(char label)
        {
            view = label;

            if (label == '=')
                value = -1;
            else if (label >= '0' && label <= '9')
                value = (label - '0');
            else
                value = (label - 'A') + 10;
        }
    }

    class Problem
    {
        public int width;
        public int height;
        public string answer;
        public bool answerLocked;
        public List<SlidePanel> panels = null;

        public Problem(int w, int h, string content)
        {
            if (panels == null)
                panels = new List<SlidePanel>();

            width = w;
            height = h;
            answer = "";
            answerLocked = false;

            foreach (char c in content)
            {
                SlidePanel item = new SlidePanel(c);
                panels.Add(item);
            }
        }
    }
}
