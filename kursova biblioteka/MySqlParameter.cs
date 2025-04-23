
namespace kursova_biblioteka
{
    internal class MySqlParameter
    {
        private string v;
        private string title;
        private int year_published;
        private DateOnly birthday;

        public MySqlParameter(string v, string title)
        {
            this.v = v;
            this.title = title;
        }

        public MySqlParameter(string v, int year_published)
        {
            this.v = v;
            this.year_published = year_published;
        }

        public MySqlParameter(string v, DateOnly birthday)
        {
            this.v = v;
            this.birthday = birthday;
        }
    }
}