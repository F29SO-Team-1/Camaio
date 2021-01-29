using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlConnector;

namespace Login.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }


        internal AppDb Db { get; set; }

        //constructor
        public UserModel()
        {

        }

        //read the AppDb class, for the connection
        internal UserModel(AppDb db)
        {
            Db = db;
        }

        // public async Task InsertAsync()
        // {
        //     using var cmd = Db.Connection.CreateCommand();
        //     /*Insert SQL string here*/
        //     BindParams(cmd);
        //     await cmd.ExecuteNonQueryAsync();
        //     Id = cmd.LastInsertedId;
        // }

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
        public class UserQuery
        {
            internal AppDb Db { get; set; }
            internal UserQuery(AppDb db)
            {
                Db = db;
            }

            public async Task<UserModel> FindUser(string id)
            {
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"SELECT `UserName` from `AspNetUsers` Where `UserName` = @id";
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    DbType = DbType.String,
                    Value = id
                });
                var reader = await cmd.ExecuteReaderAsync();
                using (reader)
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new UserModel(Db)
                        {
                            Id = id,
                            UserName = reader.GetString(0)
                        };
                        return user;
                    }
                }
                return null;
            }
        }
    }
    
}