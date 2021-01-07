
using System;

namespace YAMLCheckerWin
{
    public class CheckerAttribute : Attribute
    {
        public string val { private set; get; }
        public CheckerAttribute(string _pattern)
        {
            this.val = _pattern;
        }
    }
}
