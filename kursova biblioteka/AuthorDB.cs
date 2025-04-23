using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursova_biblioteka
{
    internal class AuthorDB
    {
        MyConnection connection;

        private AuthorDB(MyConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Author author)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = (MySqlCommand)connection.CreateCommand("insert into `author` Values (0, @First_name, @Last_name, @Birthday)");


                cmd.Parameters.Add(new MySqlParameter("first_name", author.FirstName));
                cmd.Parameters.Add(new MySqlParameter("last_name", author.LastName));
                cmd.Parameters.Add(new MySqlParameter("birthday", author.Birthday));


                try
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        cmd = (MySqlCommand)connection.CreateCommand("select LAST_INSERT_ID();");

                        int id = (int)(ulong)cmd.ExecuteScalar();
                        if (id > 0)
                        {
                            MessageBox.Show(id.ToString());

                            author.ID = id;
                            result = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Author> SelectAll()
        {
            List<Author> author = new List<Author>();
            if (connection == null)
                return author;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `first_name`, `last_name`, `birthday` from `author` ");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string first_name = string.Empty;
                        string patrionymic = string.Empty;
                        string last_name = string.Empty;
                        DateOnly birthday = DateOnly.MinValue;

                        if (!dr.IsDBNull(1))
                            first_name = dr.GetString(1);
                        if (dr.IsDBNull(2))
                            last_name = dr.GetString(2);
                        if (dr.IsDBNull(3))
                            birthday = dr.GetDateOnly(3);

                        author.Add(new Author
                        {
                            ID = id,
                            FirstName = first_name,
                            LastName = last_name,
                            Birthday = birthday

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return author;
        }

        internal bool Update(Author edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `author` set `first_name`=@first_name, `patronymic`=@patrionymic, `last_name`=@last_name, `birthday`=@birthday  where `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("first_name", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("patrionymic", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("last_name", edit.LastName));
                mc.Parameters.Add(new MySqlParameter("birthday", edit.Birthday));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Author remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `author` where `id` = {remove.ID}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static AuthorDB db;
        public static AuthorDB GetDb()
        {
            if (db == null)
                db = new AuthorDB(MyConnection.GetDbConnection());
            return db;
        }
    }

}
