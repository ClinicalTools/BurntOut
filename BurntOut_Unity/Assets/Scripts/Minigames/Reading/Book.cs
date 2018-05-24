using System;

namespace Minigames.Reading
{
    [Serializable]
    public class Book
    {
        public Page[] pages = new Page[5];

        public Book()
        {
            pages = new Page[5];
        }
    }
}