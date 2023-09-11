using Microsoft.Data.SqlClient;
using ProyectoF2.Models;
using System.Data;

namespace ProyectoF2.Logica
{
    public class LO_Usuario
    {

        public Usuario EncontrarUsuarios(string correo, string clave)
        {
            Usuario objeto = new Usuario();

            using (SqlConnection conexion = new SqlConnection("Data Source=LAPTOP-SFGN9P20\\SQLEXPRESS01;Initial Catalog=BibliotecaMarcos;Integrated Security=True;Encrypt=False"))
            {

                string query = "select Identificacion,Nombre,Apellido,Correo,Clave,Edad,IdRol,FechaIngreso from Usuario where Correo=@pcorreo and Clave=@pclave";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@pcorreo", correo);
                cmd.Parameters.AddWithValue("@pclave", clave);
                cmd.CommandType = CommandType.Text;

                conexion.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                       objeto = new Usuario();
                        {
                            objeto.Identificacion = dr["Identificacion"].GetHashCode();
                            objeto.Nombre = dr["Nombre"].ToString();
                            objeto.Apellido = dr["Apellido"].ToString();
                            objeto.Correo = dr["Correo"].ToString();
                            objeto.Clave = dr["Clave"].ToString();
                            objeto.Edad = dr["Edad"].GetHashCode();
                            objeto.IdRol = dr["IdRol"].GetHashCode();
                            objeto.FechaIngreso =(DateTime) dr["FechaIngreso"];


                          
                        };
                    }
                }
            }
            return objeto;
        }

    }
}
