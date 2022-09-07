using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace renombrar_archivos
{
    class Program
    {
        static void Main(string[] args)
        {
            //lista para guardar las referencias
            List<string> referencias = new List<string>();
            //lista para guardar las rutas de los archivos
            List<string> rutas = new List<string>();

            using (SqlConnection conexion = new SqlConnection())
            {
                //abrir conexion
                conexion.ConnectionString = "Server=AKES-DB-01-003;Database=TEMP_IMPORT;Trusted_Connection=true";
                conexion.Open();
                //consultar las referencias
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.[SSD-11064_Renombrar2]", conexion);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //guardar las referencias en la lista referencias
                        referencias.Add(String.Format("{0}", reader[0]));
                    }
                }
                //consultar las rutas de los archivos
                SqlCommand command2 = new SqlCommand("SELECT NHA.ExternFile FROM NOVADB.dbo.niHstAtt AS NHA INNER JOIN NOVADB.dbo.niHist AS NH ON NHA.HistoryNo = NH.HistoryNo INNER JOIN NOVADB.dbo.niAct AS N ON NH.ObjectNo = N.ActNo AND ObjectType = 1 INNER JOIN TEMP_IMPORT.dbo.[SSD-11064_Renombrar2] AS C ON C.Reference = N.ReferenceNo", conexion);
                using (SqlDataReader reader = command2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rutas.Add(String.Format("{0}", reader[0]));
                    }
                }
                conexion.Close();
                conexion.Dispose();
            }
            foreach (string referencia in referencias)
            {
                Console.WriteLine(referencia);
            }
            foreach (string ruta in rutas)
            {
                Console.WriteLine(ruta);
            }
        }
    }
}

            
