using api_loja0001.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api_loja0001.Controllers
{
    public class UsuarioController : ApiController
    {
        //Listar todos os usuários
        public List<Usuario> Get()
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "SELECT * FROM USUARIO;";               

            var lista_usuario = new List<Usuario>();

            try
            {
                conn.Open();
                MySqlDataReader fetch_query = query.ExecuteReader();

                while (fetch_query.Read())
                {
                    //Encontra a coluna com o nome usuario
                    //return fetch_query["usuario"].ToString();                      
                    lista_usuario.Add(new Usuario(Convert.ToInt16(fetch_query["ID"]),
                                                  fetch_query["USUARIO"].ToString(),
                                                  fetch_query["SENHA"].ToString(),
                                                  fetch_query["EMAIL"].ToString(),
                                                  null));

                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                lista_usuario.Add(new Usuario(Convert.ToInt16(null), null, null, null, ex.ToString()));
            }
            
            return lista_usuario;
        }

        //Listar todos os usuário com determinado código
        public List<Usuario> Get(string codigo)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "SELECT * FROM USUARIO " +
                "WHERE ID = @ID";
            query.Parameters.AddWithValue("@ID", codigo);
            var lista_usuario = new List<Usuario>();

            try
            {
                conn.Open();
                MySqlDataReader fetch_query = query.ExecuteReader();
                while (fetch_query.Read())
                {
                    //Encontra a coluna com o nome usuario
                    //return fetch_query["usuario"].ToString();                      
                    lista_usuario.Add(new Usuario(Convert.ToInt16(fetch_query["ID"]),
                                                  fetch_query["USUARIO"].ToString(),
                                                  fetch_query["SENHA"].ToString(),
                                                  fetch_query["EMAIL"].ToString(),
                                                  null));
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                lista_usuario.Add(new Usuario(Convert.ToInt16(null), null, null, null, ex.ToString()));
            }         
            
            return lista_usuario;
        }

        //Busca o próximo código do usuário, passar o parâmetro de valor = usuario
        public string Get(string variavel, string valor)
        {
            //O parâmetro de valor não server para nada, apenas para poder existir esta estrutura
            switch (variavel)
            {
                case "proximoCodigo":
                    return BuscaProximoCodigoUsuario();
                default:
                    return "erro";
            }
        }

        //Verifica se existe o usuário e senha, retornando verdadeiro ou falso
        public string Get(string variavel, string usuario, string senha)
        {
            switch (variavel)
            {
                case "existeUsuarioSenha":
                    return ExisteUsuarioSenha(usuario, senha);
                default:
                    return "erro";
            }
        }        

        #region public string Post(string usuario, string senha, string email)
        //public string Post(string usuario, string senha, string email)
        //{
        //    string retorno = "";
        //    MySqlConnection conn = WebApiConfig.conn();
        //    MySqlCommand query = conn.CreateCommand();

        //    query.CommandText =
        //        "INSERT INTO USUARIO (USUARIO, SENHA, EMAIL) " +
        //        "VALUES (@USUARIO, @SENHA, @EMAIL);";

        //    query.Parameters.AddWithValue("@USUARIO", usuario);
        //    query.Parameters.AddWithValue("@SENHA", senha);
        //    query.Parameters.AddWithValue("@EMAIL", email);

        //    try
        //    {
        //        conn.Open();
        //        query.ExecuteReader();
        //        conn.Close();
        //        retorno = "sucesso";
        //    }
        //    catch (MySqlException)
        //    {
        //        //throw;
        //        retorno = "erro";
        //    }
        //    return retorno;

        //}
        #endregion
        
        //Chama o Adicionar Usuário
        public string Post(string variavel, string usuario, string senha, string email)
        {
            switch (variavel)
            {
                case "post":
                    if (AdicionarUsuario(usuario, senha, email) == "sucesso")
                    {
                        return "sucesso";
                    }
                    else
                    {
                        return "erro";
                    }
                default:
                    return "erro";
            }
        }

        //Chama o Deletar Usuário
        public string Post(string variavel, string codigo)
        {
            switch (variavel)
            {
                case "delete":
                    if (DeletarUsuario(codigo) == "sucesso")
                    {
                        return "sucesso";
                    }
                    else
                    {
                        return "erro";
                    }
                default:
                    return "erro";
            }
        }

        //Chama o Atualizar Usuário
        public string Post(string variavel, string codigo, string usuario, string senha, string email)
        {
            switch (variavel)
            {
                case "put":
                    if (AtualizarUsuario(codigo, usuario, senha, email) == "sucesso")
                    {
                        return "sucesso";
                    }
                    else
                    {
                        return "erro";
                    }
                default:
                    return "erro";
            }
        }

        #region Código antigo
        public string Delete(int id)
        {
            //usuarios.RemoveAt(usuarios.IndexOf(usuarios.First(x => x._id.Equals(id))));
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            query.CommandText =
                "DELETE FROM USUARIO " +
                "WHERE ID = @ID";

            query.Parameters.AddWithValue("@ID", id);

            try
            {
                conn.Open();
                query.ExecuteReader();
                conn.Close();
                return "sucesso";
            }
            catch (MySqlException)
            {
                return "erro";
            }

        }

        public string Put(int codigo, string usuario, string senha, string email)
        {
            string retorno = "";
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "UPDATE USUARIO " +
                "SET USUARIO = @USUARIO " +
                ",SENHA = @SENHA " +
                ",EMAIL = @EMAIL " +
                "WHERE ID = @ID;";

            query.Parameters.AddWithValue("@ID", codigo);
            query.Parameters.AddWithValue("@USUARIO", usuario);
            query.Parameters.AddWithValue("@SENHA", senha);
            query.Parameters.AddWithValue("@EMAIL", email);

            try
            {
                conn.Open();
                query.ExecuteReader();
                conn.Close();
                retorno = "sucesso";
            }
            catch (MySqlException)
            {
                //throw;
                retorno = "erro";
            }
            return retorno;

        }
        #endregion         

        //Implementado a adaptação para a nova regra da Umbler

        public string ExisteUsuarioSenha(string usuario, string senha)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "SELECT * FROM USUARIO " +
                "WHERE USUARIO = @USUARIO " +
                "AND SENHA = @SENHA;";

            query.Parameters.AddWithValue("@USUARIO", usuario);
            query.Parameters.AddWithValue("@SENHA", senha);

            try
            {
                conn.Open();
                MySqlDataReader dataReader = query.ExecuteReader();
                if (dataReader.Read() == true)
                {
                    return "verdadeiro";
                }
                conn.Close();
                return "falso";
            }
            catch (MySqlException)
            {
                return "erro";
            }
        }

        public string BuscaProximoCodigoUsuario()
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "SELECT MAX(ID)+1 ID FROM USUARIO ";            
            
            string proximoCodigo;
            try
            {
                conn.Open();
                MySqlDataReader dataReader = query.ExecuteReader();

                if (dataReader.Read())
                {                                     
                    proximoCodigo = dataReader["ID"].ToString();
                    conn.Close();
                    return proximoCodigo;
                }    
                else
                {
                    return "erro";
                }
            }
            catch (MySqlException)
            {
                return "erro";
            }    
        }

        public string AdicionarUsuario(string usuario, string senha, string email)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "INSERT INTO USUARIO (USUARIO, SENHA, EMAIL) " +
                "VALUES (@USUARIO, @SENHA, @EMAIL);";

            query.Parameters.AddWithValue("@USUARIO", usuario);
            query.Parameters.AddWithValue("@SENHA", senha);
            query.Parameters.AddWithValue("@EMAIL", email);

            try
            {
                conn.Open();
                query.ExecuteReader();
                conn.Close();
                return "sucesso";
            }
            catch (MySqlException)
            {                
                return "erro";
            }
            
        }        

        public string DeletarUsuario(string codigo)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            query.CommandText =
                "DELETE FROM USUARIO " +
                "WHERE ID = @ID";
            query.Parameters.AddWithValue("@ID", codigo);
            try
            {
                conn.Open();
                query.ExecuteReader();
                conn.Close();
                return "sucesso";
            }
            catch (MySqlException)
            {
                return "erro";
            }           
        }

        public string AtualizarUsuario(string codigo, string usuario, string senha, string email)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "UPDATE USUARIO " +
                "SET USUARIO = @USUARIO " +
                ",SENHA = @SENHA " +
                ",EMAIL = @EMAIL " +
                "WHERE ID = @ID;";
            query.Parameters.AddWithValue("@ID", codigo);
            query.Parameters.AddWithValue("@USUARIO", usuario);
            query.Parameters.AddWithValue("@SENHA", senha);
            query.Parameters.AddWithValue("@EMAIL", email);

            try
            {
                conn.Open();
                query.ExecuteReader();
                conn.Close();
                return "sucesso";
            }
            catch (MySqlException)
            {
                //throw;
                return "erro";
            }

        }
    }
}

