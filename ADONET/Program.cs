using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONET
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lee valor del app config
            var mensaje = ConfigurationManager.AppSettings["mensaje1"];
            Console.WriteLine(mensaje);

            // Creacion de un connection string
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"NombreDelServidor";
            builder.InitialCatalog = "NombreDeLaBaseDeDatos";
            builder.IntegratedSecurity = true;
            builder.UserID = "nombreUsuario";
            builder.Password = "contraseñaUsuario";

            var connectionString = builder.ToString();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
            }

            /*
             * Se puede hacer de esta forma, pero es mejor y mas mantenible llamar a tag ConnectionString del app.congif
             * y llamarlo de la siguiente manera:
             * */

            var conecctionString2 = ConfigurationManager.ConnectionStrings["nombreDelConnectionString"].ConnectionString;

            // query desde C#

            int numero = 0;
            string query = @"SELECT * FROM tabla WHERE numero = " + numero;
            using (SqlConnection conn = new SqlConnection(conecctionString2))
            {
                using (SqlCommand comando = new SqlCommand(query, conn))
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    conn.Open();
                    adaptador.Fill(dt);
                }
            }

            /*
             *                        OJO!
             * Esta forma es peligrosa por que permite inyeccion de SQL
             * Para evitar esto se debe pasar la variable numero como un parametro:
             * */

            int numero2 = 0;
            string query2 = @"SELECT * FROM tabla WHERE numero = @numero";
            using (SqlConnection conn = new SqlConnection(conecctionString2))
            {
                using (SqlCommand comando = new SqlCommand(query2, conn))
                {
                    comando.Parameters.Add(new SqlParameter("@numero", numero2));
                    DataTable dt = new DataTable();
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    conn.Open();
                    adaptador.Fill(dt);
                }
            }

            // Store Procedure SELECT
            int numero3 = 0;
            using (SqlConnection conn = new SqlConnection(conecctionString2))
            {
                using (SqlCommand comando = new SqlCommand("nombre_del_store_procedure", conn))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@numero", numero3));
                    DataTable dt = new DataTable();
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    conn.Open();
                    adaptador.Fill(dt);
                }
            }

            // Store Procedure INSERT, DELETE, UPDATE
            string nombre = "Juan";
            string apellido = "Perez";
            int edad = 33;
            using (SqlConnection conn = new SqlConnection(conecctionString2))
            {
                using (SqlCommand comando = new SqlCommand("nombre_del_store_procedure", conn))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@nombre", nombre));
                    comando.Parameters.Add(new SqlParameter("@apellido", apellido));
                    comando.Parameters.Add(new SqlParameter("@edad", edad));
                    conn.Open();
                    // Al no devolver datos (solo se hace una accion en la base)
                    // es suficiente con la siguiente linea para ejecutar el sp
                    comando.ExecuteNonQuery();
                }
            }

            // DataTables
            int numero4 = 0;
            DataTable tabla = DevuelveDataTable(numero4);

            // Agregar columnas
            tabla.Columns.Add("Nombre", typeof(string));
            tabla.Columns.Add("Edad", typeof(string));

            // Agrega filas
            tabla.Rows.Add("Juan", 23);
            tabla.Rows.Add("Juancita", 58);

            // Leer un valor de una fila
            var nombreDt = tabla.Rows[0]["Nombre"]; // Juan
            var edadDt = tabla.Rows[1]["Edad"]; // 58

            // Borra la data del DataTable
            tabla.Clear();

            // Iterar un DataTable
            foreach (DataRow row in tabla.Rows)
            {
                Console.WriteLine(row["Nombre"] + " - " + row["Edad"]);
            }

            // Transacciones
            int id = 5;
            try
            {
                using (SqlConnection conn = new SqlConnection(conecctionString2))
                {
                    conn.Open();

                    var transaction = conn.BeginTransaction();

                    using (SqlCommand comando = new SqlCommand("nombre_del_store_procedure", conn, transaction))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", id));
                        comando.ExecuteNonQuery();
                    }
                    // si ocurrio un error, salta una excepcion y el commit nunca llega a ejecutarse

                    // si salio todo bien, llega hasta aca y termina la transaccion exitosamente
                    transaction.Commit();
                }
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);

            }

            // LINQ en DataTables
            /*
             * AsEnumerable() => sirve para utilizar LINQ en DataTables
             * */

            // Filtrando
            var mayoresDe45 = tabla.AsEnumerable().Where(x => x.Field<int>("Edad") > 45).AsDataView();

            // Ordenando
            var ordenarPorEdadAsc = tabla.AsEnumerable().OrderBy(x => x.Field<string>("Edad")).AsDataView();

            // Convertimos los datos del DataTable en un objeto,
            // que tiene las propiedades Nombre y Edad
            var objetos = tabla.AsEnumerable().Select(x =>
            new
            {
                Nombre = x.Field<string>("Nombre"),
                Edad = x.Field<int>("Edad")
            });

            foreach (var objeto in objetos)
            {
                Console.WriteLine(objeto.Nombre + " - " + objeto.Edad);
            }

            // DataSet
            // Un DataSet es una coleccion de DataTables
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(conecctionString2))
            {
                using (SqlCommand comando = new SqlCommand("nombre_del_store_procedure", conn))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    conn.Open();
                    adaptador.Fill(ds);
                }
            }
            // La query que se ejecuta en el store procedure es algo asi como:
            /*
             * SELECT col1, col2, col3 FROM tabla1
             * SELECT col1, col2, col3 FROM tabla2
             * SELECT col1, col2, col3 FROM tabla3
             * */
            // Esto retorna 3 DataTables
            var tabla1 = ds.Tables[0];
            var tabla2 = ds.Tables[1];
            var tabla3 = ds.Tables[2];

            // Ahora se pueden manipular los DataTables

            Console.ReadLine();
        }
        public static DataTable DevuelveDataTable(int numero4)
        {
            // Esta linea no debe ir, solo para el ejemplo
            var conecctionString2 = ConfigurationManager.ConnectionStrings["nombreDelConnectionString"].ConnectionString;
            // -------------------------------------------


            using (SqlConnection conn = new SqlConnection(conecctionString2))
            {
                using (SqlCommand comando = new SqlCommand("nombre_del_store_procedure", conn))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@numero", numero4));
                    DataTable dt = new DataTable();
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    conn.Open();
                    adaptador.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
