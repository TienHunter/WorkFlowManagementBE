using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WorkFM.Common.Utils
{
    public static class Helper
    {
        public static string HandleSQLColumn(string columns)
        {
            return Regex.Replace(columns, "[^0-9a-zA-Z,_]", "");
        }
        public static string GetTableName(Type entityType)
        {
            // Kiểm tra xem lớp có thuộc tính [Table] hay không
            var tableAttribute = entityType.GetCustomAttribute<TableAttribute>();
            if (tableAttribute != null)
            {
                return tableAttribute.Name;
            }

            // Nếu không có thuộc tính [Table], chuyển đổi tên lớp thành kiểu snake_case
            string className = entityType.Name;
            StringBuilder tableName = new StringBuilder();
            tableName.Append(char.ToLower(className[0])); // Bắt đầu bằng ký tự viết thường

            for (int i = 1; i < className.Length; i++)
            {
                char currentChar = className[i];
                if (char.IsUpper(currentChar))
                {
                    tableName.Append('_');
                    tableName.Append(char.ToLower(currentChar));
                }
                else
                {
                    tableName.Append(currentChar);
                }
            }

            return tableName.ToString();
        }
    }
}
