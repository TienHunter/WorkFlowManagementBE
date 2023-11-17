using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;
using System.Reflection;
using WorkFM.Common.Utils;

namespace WorkFM.Common.Commands
{
    public static class QueryCommand
    {
        public static string BuildCommand(Type typeEntity, StateType stateType)
        {

            var tableNameAttribute = typeEntity.GetCustomAttributes<TableAttribute>().FirstOrDefault();
            var properties = typeEntity.GetProperties();
            var keyProperty = properties.FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));

            if (tableNameAttribute == null || keyProperty == null)
            {
                throw new Exception("Invalid typeEntity or keyProperty");
            }
            var sql = "";
            switch (stateType)
            {
                case StateType.Default:
                    sql = $"SELECT * FROM {tableNameAttribute.Name} where {keyProperty.Name}=@{keyProperty.Name} ;";
                    break;
                case StateType.Insert:
                    sql = $"INSERT INTO {tableNameAttribute.Name} (";
                    sql += string.Join(" , ", properties.Select(prop => prop.Name));
                    sql += $") VALUES ({string.Join(", ", properties.Select(prop => $"@{prop.Name}"))});";
                    break;
                case StateType.Update:
                    sql = $"UPDATE {tableNameAttribute.Name} SET ";
                    sql += string.Join(" , ", properties.Where(prop => prop.Name != keyProperty.Name).Select(prop => $"{prop.Name}=@{prop.Name}"));
                    sql += $" WHERE {keyProperty.Name}=@{keyProperty.Name} ;";
                    break;
                case StateType.Delete:
                    sql = $"DELETE FROM {tableNameAttribute.Name} where {keyProperty.Name}=@{keyProperty.Name} ;";
                    break;
                default:
                    throw new Exception("Invalid stateType");
            }

            return sql;
        }

        public static string GetAll<T>()
        {
            var tableNameAttribute = typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault();
            if (tableNameAttribute == null)
            {
                // ghi log

                throw new Exception("InValid typeEntity");
            }

            var sql = $"SELECT * FROM {tableNameAttribute.Name}";
            return sql;
        }

        public static string GetById<T>()
        {
            var tableNameAttribute = typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault();
            var properties = typeof(T).GetProperties();
            var keyProperty = properties.FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));

            if (tableNameAttribute == null || keyProperty == null)
            {
                // ghi log
                throw new Exception("Invalid typeEntity or keyProperty");
            }

            var sql = $"SELECT * FROM {tableNameAttribute.Name} WHERE {keyProperty.Name} = @Id";
            return sql;
        }

        public static string GetPaging<T>(List<string> listCoumns)
        {
            var tableNameAttribute = typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault();
            var properties = typeof(T).GetProperties();

            if (tableNameAttribute == null)
            {
                // ghi log
                throw new Exception("Invalid typeEntity");
            }
            var sql = $"SELECT * FROM  {tableNameAttribute.Name} WHERE " ;
            if(listCoumns.Count() == 0)
            {
                sql += string.Join(" AND ", properties.Select(prop => $"{prop.Name} LIKE \"%@KeySearch%\""));
            }
            else
            {
                sql += string.Join(" AND ", listCoumns.Select(column => $"{column} LIKE \"%@KeySearch%\""));
            }
            sql += $" LIMIT @Limit OFFSET @Offset ;";

            return sql;
        }

        public static string Create<T>()
        {
            var tableName = Helper.GetTableName(typeof(T));
            var properties = typeof(T).GetProperties();
            var sql = $"INSERT INTO {tableName} (";
            sql += string.Join(" , ", properties.Select(prop => prop.Name));
            sql += $") VALUES ({string.Join(", ", properties.Select(prop => $"@{prop.Name}"))});";

            return sql;
        }

        public static string Update<T>()
        {
            var tableNameAttribute = typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault();
            var properties = typeof(T).GetProperties();
            var keyProperty = properties.FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));
            if (tableNameAttribute == null || keyProperty == null)
            {
                // ghi log
                throw new Exception("Invalid typeEntity");
            }
            var sql = $"UPDATE {tableNameAttribute.Name} SET ";
            sql += string.Join(" , ", properties.Where(prop => prop.Name != keyProperty.Name).Select(prop => $"{prop.Name}=@{prop.Name}"));
            sql += $" WHERE {keyProperty.Name}= @Id ;";
            return sql;
        }

        public static string Delete<T>()
        {
            var tableNameAttribute = typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault();
            var properties = typeof(T).GetProperties();
            var keyProperty = properties.FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));

            if (tableNameAttribute == null || keyProperty == null)
            {
                // ghi log
                throw new Exception("Invalid typeEntity or keyProperty");
            }

            var sql = $"DELETE FROM {tableNameAttribute.Name} WHERE {keyProperty} = @Id";
            return sql;
        }

        //--------- user --------------------
        public static string GetUserByUsernameOrEmail()
        {
            var sql = $"SELECT * FROM user WHERE Username = @Username OR Email = @Username ;";
            return sql;
        }

        public static string GetUserByEmail()
        {
            var sql = $"SELECT * FROM user WHERE Email = @Email ;";
            return sql;
        }

        public static string GetUserByUsername()
        {
            var sql = $"SELECT * FROM user WHERE Username = @Username ;";
            return sql;
        }
    }
}
