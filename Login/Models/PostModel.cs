using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlConnector;

namespace Login.Models
{
    public class PostModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public int Votes { get; set; }
        public string User { get; set; }



        internal AppDb Db { get; set; }

        //constructor
        public PostModel()
        {

        }

        //read the AppDb class, for the connection
        internal PostModel(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            /*Insert SQL string here*/
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            /*Update SQL string here*/
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            /*Delete SQL string here*/
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {

        }

        private void BindParams(MySqlCommand cmd)
        { }

        //Query the posts
        public class PostQuery
        {
            internal AppDb Db { get; set; }
            internal PostQuery(AppDb db)
            {
                Db = db;
            }

            // api/posts
            public async Task<List<PostModel>> LatestPostsAsync()
            {
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM `Posts`";
                return await ReadAllAsync(await cmd.ExecuteReaderAsync());
            }

            // api/posts/{id}
            public async Task<PostModel> FindOneAsync(int id)
            {
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"select * from `Posts` Where `Post_id` = @id";
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = id
                });
                var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
                return result.Count > 0 ? result[0] : null;
            }

            private async Task<List<PostModel>> ReadAllAsync(DbDataReader reader)
            {
                var posts = new List<PostModel>();
                using (reader)
                {
                    while (await reader.ReadAsync())
                    {
                        var post = new PostModel(Db)
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            Picture = reader.GetString(3),
                            Location = reader.GetString(4),
                            Votes = reader.GetByte(5),
                            User = reader.GetString(6)
                        };
                        posts.Add(post);
                    }
                }
                return posts;
            }
        }
    }
    
}
