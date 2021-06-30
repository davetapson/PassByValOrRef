using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PassByValOrRef
{
    class Program
    {
        static void Main(string[] args)
        {
            // ah, bloody C#.  Ok, it's not that simple any more.

            // it's simple with value types, eg integers

            int i = 1;

            PassByRef(ref i, 2);    // I'm passing by ref, eg memory address. I update the int at the memory address that I passed in.

            Assert.AreEqual(2, i);  // see, I'm updated

            PassByRef2(ref i, 3);    // I'm passing by ref, eg memory address. I create a new int at the memory address that I passed in.

            Assert.AreEqual(3, i);  // see, I'm updated

            PassByVal(i, 3);        // I'm passing a copy of i in, at a diffent memory address

            Assert.AreEqual(3, i);  // see, I changed the copy in the method, but the original int is still 3.  The copy has died alone and unloved and will be garbage collected.



            // with reference types, i.e. anything more complicated than a value type (e.g. int) gets passed by reference anyway
            var refBookName1 = "I'm a new book, but with the same address as the original book. The original book is dead.  Long live the book.";
            var refBookName2 = "I'm the same book, I just had my name changed.";
            var valBookName = "I'm the same book, I just had my name changeed. Again.";
            var valBookNameButGetMeBack = "Hey, I went away, but now I'm back";
            var valBookNameButGetNewBookBack = "I'm a new book";



            var book = new Book("Book 1");

            
            // exercise 1
            var bookHashCodeBefore = book.GetHashCode();

            PassByRef1(ref book, refBookName1);

            var bookHashCodeAfter = book.GetHashCode();

            Assert.AreEqual(refBookName1, book.Name);   // new book old address
            Assert.AreNotEqual(bookHashCodeBefore, bookHashCodeAfter);  // object hash codes are different - different books.

            
            // exercise 2
            
            bookHashCodeBefore = book.GetHashCode();

            PassByRef2(ref book, refBookName2);

            bookHashCodeAfter = book.GetHashCode();

            Assert.AreEqual(refBookName2, book.Name);   // same book, new name
            Assert.AreEqual(bookHashCodeBefore, bookHashCodeAfter);  // object hash codes are the same, same book.


            // exercise 3
            bookHashCodeBefore = book.GetHashCode();

            PassByVal(book, valBookName);

            bookHashCodeAfter = book.GetHashCode();

            Assert.AreEqual(valBookName, book.Name);   // same book, new name
            Assert.AreEqual(bookHashCodeBefore, bookHashCodeAfter);  // object hash codes are the same, same book.


            // exercise 4 
            // we don't like passing by refs, this is the way we'd prefer to do it
            bookHashCodeBefore = book.GetHashCode();

            book = PassByValButGetMeBack(book, valBookNameButGetMeBack);

            bookHashCodeAfter = book.GetHashCode();

            Assert.AreEqual(valBookNameButGetMeBack, book.Name);   // same book, new name
            Assert.AreEqual(bookHashCodeBefore, bookHashCodeAfter);  // object hash codes are the same, same book.


            // exercise 5
            // we don't like passing by refs, this is the way we'd prefer to do it - this one brings back new book
            bookHashCodeBefore = book.GetHashCode();

            book = PassByValButGetNewBookBack(book, valBookNameButGetNewBookBack);

            bookHashCodeAfter = book.GetHashCode();

            Assert.AreEqual(valBookNameButGetNewBookBack, book.Name);   // new book
            Assert.AreNotEqual(bookHashCodeBefore, bookHashCodeAfter);  // object hash codes are diffent, different books.

        }

        private static void PassByRef2(ref int i, int v)
        {
            i = v;
        }

        private static Book PassByValButGetNewBookBack(Book book, string valBookNameButGetNewBookBack)
        {
            book = new Book(valBookNameButGetNewBookBack);

            return book;
        }

        private static void PassByRef(ref int i, int v)
        {
            i *= v;
        }

        private static void PassByVal(int i, int v)
        {
            i = i * v;  // this dies unloved and alone in this method, get's garbage collected.
        }

        private static Book PassByValButGetMeBack(Book book, string valBookNameButGetMeBack)
        {
            book.Name = valBookNameButGetMeBack;

            return book;  // I've got a copy of the original book, but I'm going to return this copy - I have to actively return it by saying 'return'
        }

        private static void PassByVal(Book book, string valBookName)
        {
            book.Name = valBookName;  // Reference type objects have their ref passed in even when passing by val, so the orig book will have it's name changed.
        }

        private static void PassByRef2(ref Book book, string name)
        {
            book.Name = name;
        }

        private static void PassByRef1(ref Book book, string name)
        {
            book = new Book(name);  // I'm a new book at the old address
        }
    }

    public class Book
    {
        public Book(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
