using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursova_biblioteka
{
    class WinAddAuthor
    {
        internal class WinAddAuthors : BaseVM
        {
            private Author newAuthor = new();

            public Author NewAuthor
            {
                get => newAuthor;
                set
                {
                    newAuthor = value;
                    Signal();
                }
            }

            public Command InsertAuthor { get; set; }
            public WinAddAuthors()
            {
                InsertAuthor = new Command(() =>
                {
                    AuthorDB.GetDb().Insert(NewAuthor);
                    close?.Invoke();
                },
                    () =>
                    !string.IsNullOrEmpty(newAuthor.FirstName) &&
                    !string.IsNullOrEmpty(newAuthor.LastName));
            }
            Action close;
            internal void SetClose(Action close)
            {
                this.close = close;
            }
        }
    }

    public class Command
    {
        public Command(Action value1, Func<bool> value2)
        {
        }
    }
}

