
namespace kursova_biblioteka
{
    internal class MySqlCommand
    {
        public object Parameters { get; internal set; }

        internal int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        internal MySqlDataReader ExecuteReader()
        {
            throw new NotImplementedException();
        }

        internal ulong ExecuteScalar()
        {
            throw new NotImplementedException();
        }
    }
}