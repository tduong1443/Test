using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KtraMau
{
    internal class ProccessDatabase
    {
        string strcon = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=F:\\C#\\KtraMau\\KtraMau\\NhanVien.mdf;Integrated Security=True";
        SqlConnection con;
        SqlTransaction trans;

        public void KetNoi()
        {
            con = new SqlConnection(strcon);
            if(con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }

        public void DongKetNoi()
        {
            if(con.State != ConnectionState.Closed)
            {
                con.Close();
            }
            con.Dispose();
        }

        public DataTable DocBang(string sql)
        {
            DataTable tb = new DataTable();
            KetNoi();
            SqlDataAdapter ad = new SqlDataAdapter(sql, con);
            ad.Fill(tb);
            DongKetNoi();
            return tb;
        }
        public void CapNhat(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql);
            KetNoi();
            cmd.CommandText = sql;
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            DongKetNoi();
            cmd.Dispose();
        }

        public void CapNhatTS(string sql, SqlParameter[] parameters = null)
        {
            SqlCommand cmd = new SqlCommand(sql);
            KetNoi();
            cmd.CommandText = sql;
            cmd.Connection = con;

            // Thêm tham số vào câu lệnh SQL
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            cmd.ExecuteNonQuery();
            DongKetNoi();
            cmd.Dispose();
        }

        public DataTable LayDuLieu(string sql, params SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            KetNoi();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddRange(parameters);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);
            DongKetNoi();
            return dataTable;
        }

        public object LayGiaTri(string sql, params SqlParameter[] parameters)
        {
            KetNoi();
            SqlCommand cmd = new SqlCommand(sql, con);
            // Thêm tham số vào câu lệnh SQL
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd.ExecuteScalar();
        }

        public object LayGtri(string sql)
        {
            KetNoi();
            SqlCommand cmd = new SqlCommand(sql, con);
            return cmd.ExecuteScalar();
        }

        public string getStringConncet()
        {
            return strcon;
        }
    }
}
