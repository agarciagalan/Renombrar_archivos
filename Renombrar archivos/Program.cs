using System.Data.SqlClient;


namespace renombrar_archivos
{
    class Program
    {
        static void Main(string[] args)
        {
            //lista que guarda las rutas
            List<string> rutas = new List<string>();
            using (SqlConnection conexion = new SqlConnection())
            {
                //se abre la conexion con la base de datos de NOVADB
                conexion.ConnectionString = "Server=AKES-DB-01-003;Database=NOVADB;Trusted_Connection=true";
                conexion.Open();
                //se extraen las rutas de los archivos que coinciden con el numero de referencia indicado en el excel
                SqlCommand command = new SqlCommand("SELECT NHA.ExternFile FROM NOVADB.dbo.niHstAtt AS NHA INNER JOIN NOVADB.dbo.niHist AS NH ON NHA.HistoryNo = NH.HistoryNo INNER JOIN NOVADB.dbo.niAct AS N ON NH.ObjectNo = N.ActNo AND ObjectType = 1 INNER JOIN TEMP_IMPORT.dbo.[SSD-11064_Renombrar2] AS C ON C.Reference = N.ReferenceNo", conexion);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rutas.Add(String.Format("{0}", reader[0]));
                    }
                }
                conexion.Close();
                conexion.Dispose();
            }

            //se recorren las rutas
            for (int i = 0; i < rutas.Count(); i++)
            {
                //se indica la ruta de los archivos que se estan modificando
                Console.WriteLine(rutas[i]);
                string[] referencia = rutas[i].Split('\\');
                //se extrae la referencia sin los 0
                int referenciaSinCero = Int32.Parse(referencia[7]);
                //se extrae la referencia con los 0
                string referenciaConCero = referencia[7];
                //se generan las rutas sin 0
                string rutaCompletaSaldo = rutas[i] + "\\CertSaldo" + referenciaSinCero + ".pdf";
                string rutaCompletaDispos = rutas[i] + "\\CertDispos" + referenciaSinCero + ".pdf";
                //se generan las rutas con 0
                string nuevaRutaCompletaSaldo = rutas[i] + "\\CertSaldo" + referenciaConCero + ".pdf";
                string nuevaRutaCompletaDispos = rutas[i] + "\\CertDispos" + referenciaConCero + ".pdf";
                
                //si existen los ficheros se cambia el nombre por la referencia precedida de 0
                if (File.Exists(rutaCompletaSaldo))
                {
                    System.IO.File.Move(rutaCompletaSaldo, nuevaRutaCompletaSaldo);
                }
                if (File.Exists(rutaCompletaDispos))
                {
                    System.IO.File.Move(rutaCompletaDispos, nuevaRutaCompletaDispos);
                }
            }
        }
    }
}
