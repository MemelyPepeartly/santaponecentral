using System;

namespace Profile.Logic.Objects
{
    public class Tag
    {
        public Guid tagID { get; set; }
        public string tagName { get; set; }
        public bool deletable { get; set; }
        public bool tagImmutable { get; set; }
    }
}
