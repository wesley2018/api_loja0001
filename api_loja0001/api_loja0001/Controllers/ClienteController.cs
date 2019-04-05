using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api_loja0001.Models;
using MySql.Data.MySqlClient;

namespace api_loja0001.Controllers
{
    public class ClienteController : ApiController
    {
        //Listar todos os usuários
        public List<Cliente> Get()
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "SELECT * FROM CLIENTE;";

            var lista_cliente = new List<Cliente>();

            try
            {
                conn.Open();
                MySqlDataReader fetch_query = query.ExecuteReader();

                while (fetch_query.Read())
                {
                    lista_cliente.Add(new Cliente(Convert.ToInt16(fetch_query["ID"]),
                                                  fetch_query["CLIENTE"].ToString(),
                                                  fetch_query["SENHA"].ToString(),
                                                  fetch_query["EMAIL"].ToString(),
                                                  null));

                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                lista_cliente.Add(new Cliente(Convert.ToInt16(null), null, null, null, ex.ToString()));
            }

            return lista_cliente;
        }

        //Listar todos os clientes com determinado código
        public List<Cliente> Get(string codigo)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "SELECT * FROM CLIENTE " +
                "WHERE ID = @ID";
            query.Parameters.AddWithValue("@ID", codigo);
            var lista_cliente = new List<Cliente>();

            try
            {
                conn.Open();
                MySqlDataReader fetch_query = query.ExecuteReader();
                while (fetch_query.Read())
                {
                    lista_cliente.Add(new Cliente(Convert.ToInt16(fetch_query["ID"]),
                                                  fetch_query["CLIENTE"].ToString(),
                                                  fetch_query["SENHA"].ToString(),
                                                  fetch_query["EMAIL"].ToString(),
                                                  null));
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                lista_cliente.Add(new Cliente(Convert.ToInt16(null), null, null, null, ex.ToString()));
            }

            return lista_cliente;
        }

        //Busca o próximo código do cliente, passar o parâmetro de valor = cliente
        public string Get(string variavel, string valor)
        {
            //O parâmetro de valor não server para nada, apenas para poder existir esta estrutura
            switch (variavel)
            {
                case "proximoCodigo":
                    return BuscaProximoCodigoCliente();
                default:
                    return "erro";
            }
        }

        //Verifica se existe o cliente e senha, retornando verdadeiro ou falso
        public string Get(string variavel, string cliente, string senha)
        {
            switch (variavel)
            {
                case "existeClienteSenha":
                    return ExisteClienteSenha(cliente, senha);
                default:
                    return "erro";
            }
        }
       

        //Chama o Adicionar Cliente
        public string Post(string variavel, string cliente, string senha, string email)
        {
            switch (variavel)
            {
                case "post":
                    if (AdicionarCliente(cliente, senha, email) == "sucesso")
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

        //Chama o Deletar Cliente
        public string Post(string variavel, string codigo)
        {
            switch (variavel)
            {
                case "delete":
                    if (DeletarCliente(codigo) == "sucesso")
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

        //Chama o Atualizar Cliente
        public string Post(string variavel, string codigo, string cliente, string senha, string email)
        {
            switch (variavel)
            {
                case "put":
                    if (AtualizarCliente(codigo, cliente, senha, email) == "sucesso")
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

        //Implementado a adaptação para a nova regra da Umbler

        public string ExisteClienteSenha(string cliente, string senha)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "SELECT * FROM CLIENTE " +
                "WHERE CLIENTE = @CLIENTE " +
                "AND SENHA = @SENHA;";

            query.Parameters.AddWithValue("@CLIENTE", cliente);
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

        public string BuscaProximoCodigoCliente()
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "SELECT MAX(ID)+1 ID FROM CLIENTE ";

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

        public string AdicionarCliente(string cliente, string senha, string email)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "INSERT INTO CLIENTE (CLIENTE, SENHA, EMAIL) " +
                "VALUES (@CLIENTE, @SENHA, @EMAIL);";

            query.Parameters.AddWithValue("@CLIENTE", cliente);
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

        public string DeletarCliente(string codigo)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            query.CommandText =
                "DELETE FROM CLIENTE " +
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

        public string AtualizarCliente(string codigo, string cliente, string senha, string email)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();

            query.CommandText =
                "UPDATE CLIENTE " +
                "SET CLIENTE = @CLIENTE " +
                ",SENHA = @SENHA " +
                ",EMAIL = @EMAIL " +
                "WHERE ID = @ID;";
            query.Parameters.AddWithValue("@ID", codigo);
            query.Parameters.AddWithValue("@CLIENTE", cliente);
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
