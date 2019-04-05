using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_loja0001.Models
{
    public class Usuario
    {
        public int _id { get; set; }
        public string _usuario { get; set; }
        public string _senha { get; set; }
        public string _email { get; set; }
        public string _error { get; set; }

        public Usuario(int id, string usuario, string senha, string email, string error)
        {
            this._id = id;
            this._usuario = usuario;
            this._senha = senha;
            this._email = email;
            this._error = error;
        }
    }
}