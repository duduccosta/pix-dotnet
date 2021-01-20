using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.PIX
{
    public class Config
    {
        public const string PIX_KEY = "12345678909";
        public const string PIX_MERCHANT_NAME = "Fulano de Tal";
        public const string PIX_MERCHANT_CITY = "SAO PAULO";

        //DADOS DA API PIX (DINÂMICO)
        public const string API_PIX_URL = "https://api-pix-h.urldoseupsp.com.br";
        public const string API_PIX_CLIENT_ID = "Client_id_100120310230123012301230120312";
        public const string API_PIX_CLIENT_SECRET = "Client_secret_100120310230123012301230120312";
        public const string API_PIX_CERTIFICATE = "/files/certificates/certificado.pem";
    }
}
