using System;
using System.Collections.Generic;
using System.Data.SqlClient;//引用数据库客户端
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
namespace Month11.MVC
{
    public static class DBHelper
    {
        //连接数据库
        static SqlConnection conn = new SqlConnection("Data Source=LAPTOP-6UCFJHTH\\SQLEXPRESS;Initial Catalog=Month11Week04db;User ID=sa;pwd=1234");
        static SqlDataReader sdr;
        /// <summary>
        /// 获取数据流  查询、显示、绑定下拉
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static SqlDataReader GetDataReader(string sql)
        {
            try
            {
                //打开
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //命令对象
                SqlCommand cmd = new SqlCommand(sql, conn);
                sdr = cmd.ExecuteReader();
                return sdr;
            }
            catch (Exception)
            {
                if (sdr != null && !sdr.IsClosed)//数据流关闭
                {
                    sdr.Close();
                }
                throw;
            }

        }
        /// <summary>
        /// 返回受影响行数  
        /// 添加、删除、修改
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql)
        {
            try
            {
                //打开
                //判断状态
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                //命令对象
                SqlCommand cmd = new SqlCommand(sql, conn);
                int n = cmd.ExecuteNonQuery();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                return n;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        /// <summary>
        /// 返回受影响行数
        /// 添加、删除、修改
        /// </summary>
        /// <param name="procName">存储过程名字</param>
        /// <param name="parameter">存储过程需要的参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string procName, SqlParameter[] parameter = null)
        {
            try
            {
                //打开
                //判断状态
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                //命令对象
                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameter);
                int n = cmd.ExecuteNonQuery();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                return n;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 数据流转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sdr"></param>
        /// <returns></returns>
        private static List<T> DataReaderToList<T>(SqlDataReader sdr)
        {
            Type t = typeof(T);//获取类型
            //获取所有属性
            PropertyInfo[] p = t.GetProperties();
            //定义集合
            List<T> list = new List<T>();
            //遍历数据流
            while (sdr.Read())
            {
                //创建对象
                T obj = (T)Activator.CreateInstance(t);
                //数据流列数
                string[] sdrFileName = new string[sdr.FieldCount];
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    sdrFileName[i] = sdr.GetName(i).Trim();
                }
                foreach (PropertyInfo item in p)
                {
                    //判断Model中的属性是否在流的列名中
                    if (sdrFileName.ToList().IndexOf(item.Name) > -1)
                    {
                        if (sdr[item.Name] != null)
                        {
                            item.SetValue(obj, sdr[item.Name]);//对象属性赋值
                        }
                        else
                        {
                            item.SetValue(obj, null);//对象属性赋值
                        }
                    }
                    else
                    {
                        item.SetValue(obj, null);//对象属性赋值
                    }

                }
                list.Add(obj);
            }
            return list;
        }
        /// <summary>
        /// 获取list集合
        ///  查询、显示、绑定下拉
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetList<T>(string sql)
        {
            //获取流数据
            SqlDataReader sdr = GetDataReader(sql);
            List<T> list = DataReaderToList<T>(sdr);
            if (!sdr.IsClosed)//数据流关闭
            {
                sdr.Close();
            }
            return list;
        }

        #region 1

        private static List<T> TableToList<T>(DataTable dt, bool isStoreDB = true)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            //List<string> listColums = new List<string>();
            PropertyInfo[] pArray = type.GetProperties(); //集合属性数组
            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>(); //新建对象实例 
                foreach (PropertyInfo p in pArray)
                {
                    if (!dt.Columns.Contains(p.Name) || row[p.Name] == null || row[p.Name] == DBNull.Value)
                    {
                        continue;  //DataTable列中不存在集合属性或者字段内容为空则，跳出循环，进行下个循环   
                    }
                    if (isStoreDB && p.PropertyType == typeof(DateTime) && Convert.ToDateTime(row[p.Name]) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }
                    try
                    {
                        var obj = Convert.ChangeType(row[p.Name], p.PropertyType);//类型强转，将table字段类型转为集合字段类型  
                        p.SetValue(entity, obj, null);
                    }
                    catch (Exception)
                    {
                        // throw;
                    }
                }
                list.Add(entity);
            }
            return list;
        }


        /// <summary>
        /// 调用存储过程返回泛型集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameter">存储过程需要的参数</param>
        /// <returns></returns>
        public static List<T> GetList<T>(string procName, SqlParameter[] parameter = null)
        {
            try
            {
                //打开
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DataSet set = new DataSet();

                SqlCommand cmd = new SqlCommand(procName, conn);
                //定义类型
                cmd.CommandType = CommandType.StoredProcedure;

                //添加参数
                cmd.Parameters.AddRange(parameter);

                //sqldataAdapt
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //填充数据
                adapter.Fill(set);
                //判断数据是否拿到了
                if (set.Tables.Count > 0)
                {
                    DataTable dt = set.Tables[0];
                    return TableToList<T>(dt, true);
                }
            }
            catch (Exception)
            {
                if (sdr != null && !sdr.IsClosed)//数据流关闭
                {
                    sdr.Close();
                }
                throw;
            }          
            return null;
        }

        #endregion

        /// <summary>
        /// 查询实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T Get<T>(string sql)
        {
            List<T> lis = GetList<T>(sql);
            if (lis.Count > 0)
            {
                return GetList<T>(sql)[0];
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql)
        {
            try
            {
                //打开
                //判断状态
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                //命令对象
                SqlCommand cmd = new SqlCommand(sql, conn);
                object n = cmd.ExecuteScalar();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                return n;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql)
        {
            //打开
            //判断状态
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //创建适配器
            SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
                //创建数据
                DataSet dataSet = new DataSet();
                sda.Fill(dataSet);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            return dataSet;
        }

    }
}
