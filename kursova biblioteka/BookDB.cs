using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using System.Windows;

namespace kursova_biblioteka
{
    private MyConnection connection;

    private BookDB(MyConnection db)
    {
        this.connection = db;
    }

    public bool Insert(Book book)
    {
        bool result = false;
        if (connection == null)
            return result;

        if (connection.OpenConnection())
        {
            MySqlCommand cmd = connection.CreateCommand("insert into `books` Values (0, @Title, @Author_id, @Year_published, @Genre);");

            cmd.Parameters.Add(new MySqlParameter("title", book.Title));
            cmd.Parameters.Add(new MySqlParameter("author_id", book.Author_id));
            cmd.Parameters.Add(new MySqlParameter("year_published", book.Year_published));
            cmd.Parameters.Add(new MySqlParameter("genre", book.Genre));
           

            try
            {
                // вставка заиписи
                if (cmd.ExecuteNonQuery() > 0)
                {
                    cmd = connection.CreateCommand("select LAST_INSERT_ID();");
                    // получение id последней вставленнойц записи
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        book.ID = id;
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

    internal List<Book> SelectAll()
    {
        List<Book> book = new List<Book>();
        if (connection == null)
            return book;

        if (connection.OpenConnection())
        {
            var command = connection.CreateCommand("select `id`, `title`, `author_id`, `year_published`, `genre`, `is_available` from `books` ");
            try
            {

                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    int id = dr.GetInt32(0);
                    string title = string.Empty;
                    if (!dr.IsDBNull(1))
                        title = dr.GetString(1);
                    string genre = string.Empty;
                    if (!dr.IsDBNull(4))
                        genre = dr.GetString(4);
                    int year_published = dr.GetInt32(3);
                    int author_id = dr.GetInt32(2);
                    bool is_available = dr.GetBoolean(5);

                    book.Add(new Book
                    {
                        ID = id,
                        Title = title,
                        Author_id = author_id,
                        Year_published = year_published,
                        Genre = genre
                        

                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        connection.CloseConnection();
        return book;
    }

    internal bool Update(Book edit)
    {
        bool result = false;
        if (connection == null)
            return result;

        if (connection.OpenConnection())
        {
            var mc = connection.CreateCommand($"update `books` set ` title`=@title, ` author_id`=@author_id, ` year_published`=@year_published, ` genre`=@genre, ` is_available`=@is_available where `id` = {edit.Id}");
            mc.Parameters.Add(new MySqlParameter("title", edit.Title));
            mc.Parameters.Add(new MySqlParameter("genre", edit.Genre));

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


    internal bool Remove(Book remove)
    {
        bool result = false;
        if (connection == null)
            return result;

        if (connection.OpenConnection())
        {
            ///////

            var mc = connection.CreateCommand($"delete from `books` where `id` = {remove.Id}");
            try
            {
                mc.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("точно точно удалить??");
            }
        }
        connection.CloseConnection();
        return result;
    }

    static BookDB db;
    public static BookDB GetDb()
    {
        if (db == null)
            db = new BooksDB(MyConnection.GetDbConnection());
        return db;
    }

    internal IEnumerable<Book> SelectBy(string search)
    {
        List<Book> book = new List<Book>();
        if (connection == null)
            return book;

        if (connection.OpenConnection())
        {
            var command = connection.CreateCommand("select `id`, `title`, `author_id`, `year_published`, " +
                "`genre`, `is_available` from `books` WHERE `title` like @search  or `genre` like @search  or `year_published` like @search");
            try
            {
                command.Parameters.Add(new MySqlParameter("search", "%" + search + "%"));
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    int id = dr.GetInt32(0);
                    string title = string.Empty;
                    if (!dr.IsDBNull(1))
                        title = dr.GetString(1);
                    string genre = string.Empty;
                    if (!dr.IsDBNull(4))
                        genre = dr.GetString(4);
                    int year_published = dr.GetInt32(3);
                    int author_id = dr.GetInt32(2);
                    bool is_available = dr.GetBoolean(5);

                    book.Add(new Book
                    {
                        ID = id,
                        Title = title,
                        Author_id = author_id,
                        Year_published = year_published,
                        Genre = genre
                        
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        connection.CloseConnection();
        return book;
    }
}

