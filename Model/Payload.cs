using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Payment.PIX.Model
{
    public class Payload
    {

        /**
        * IDs do Payload do Pix
        * @var string
        */
        public const string ID_PAYLOAD_FORMAT_INDICATOR = "00";
        public const string ID_POINT_OF_INITIATION_METHOD = "01";
        public const string ID_MERCHANT_ACCOUNT_INFORMATION = "26";
        public const string ID_MERCHANT_ACCOUNT_INFORMATION_GUI = "00";
        public const string ID_MERCHANT_ACCOUNT_INFORMATION_KEY = "01";
        public const string ID_MERCHANT_ACCOUNT_INFORMATION_DESCRIPTION = "02";
        public const string ID_MERCHANT_ACCOUNT_INFORMATION_URL = "25";
        public const string ID_MERCHANT_CATEGORY_CODE = "52";
        public const string ID_TRANSACTION_CURRENCY = "53";
        public const string ID_TRANSACTION_AMOUNT = "54";
        public const string ID_COUNTRY_CODE = "58";
        public const string ID_MERCHANT_NAME = "59";
        public const string ID_MERCHANT_CITY = "60";
        public const string ID_ADDITIONAL_DATA_FIELD_TEMPLATE = "62";
        public const string ID_ADDITIONAL_DATA_FIELD_TEMPLATE_TXID = "05";
        public const string ID_CRC16 = "63";

        /**
         * Chave pix
         * @var string
         */
        private string pixKey;

        /**
         * Descrição do pagamento
         * @var string
         */
        private string description;

        /**
         * Nome do titular da conta
         * @var string
         */
        private string merchantName;

        /**
         * Cidade do titular da conta
         * @var string
         */
        private string merchantCity;

        /**
         * ID da transação pix
         * @var string
         */
        private string txid;

        /**
         * Valor da transação
         * @var string
         */
        private string amount;

        /**
         * Define se o pagamento deve ser feito apenas uma vez
         * @var boolean
         */
        private bool uniquePayment = false;

        /**
         * URL do payload dinâmico
         * @var string
         */
        private string url;

        /**
         * Método responsável por definir o valor de pixKey
         * @param string pixKey
         */
        public Payload setPixKey(string _pixKey)
        {
            this.pixKey = _pixKey;
            return this;
        }

        /**
         * Método responsável por definir o valor de uniquePayment
         * @param boolean uniquePayment
         */
        public Payload setUniquePayment(bool _uniquePayment)
        {
            this.uniquePayment = _uniquePayment;
            return this;
        }

        /**
         * Método responsável por definir o valor de url
         * @param string url
         */
        public Payload setUrl(string _url)
        {
            this.url = _url;
            return this;
        }

        /**
         * Método responsável por definir o valor de description
         * @param string description
         */
        public Payload setDescription(string _description)
        {
            this.description = _description;
            return this;
        }

        /**
         * Método responsável por definir o valor de merchantName
         * @param string merchantName
         */
        public Payload setMerchantName(string _merchantName)
        {
            this.merchantName = _merchantName;
            return this;
        }

        /**
         * Método responsável por definir o valor de merchantCity
         * @param string merchantCity
         */
        public Payload setMerchantCity(string _merchantCity)
        {
            this.merchantCity = _merchantCity;
            return this;
        }

        /**
         * Método responsável por definir o valor de txid
         * @param string txid
         */
        public Payload setTxid(string _txid)
        {
            this.txid = _txid;
            return this;
        }

        /**
         * Método responsável por definir o valor de amount
         * @param float amount
         */
        public Payload setAmount(string _amount)
        {
            this.amount = _amount;
            return this;
        }

        /**
         * Responsável por retornar o valor completo de um objeto do payload
         * @param  string id
         * @param  string value
         * @return string id+size+value
         */
        private string getValue(string id, string value)
        {
            //int size = str_pad(value.Length, 2, '0', STR_PAD_LEFT);
            int size = value.PadLeft(2, '0').Length;
            return String.Format("{0}{1}{2}", id, size.ToString(), value);
        }

        /**
         * Método responsável por retornar os valores completos da informação da conta
         * @return string
         */
        private string getMerchantAccountInformation()
        {
            //DOMÍNIO DO BANCO
            string gui = this.getValue(ID_MERCHANT_ACCOUNT_INFORMATION_GUI, "br.gov.bcb.pix");

            //CHAVE PIX
            string key = !String.IsNullOrEmpty(this.pixKey) ? this.getValue(ID_MERCHANT_ACCOUNT_INFORMATION_KEY, this.pixKey) : "";

            //DESCRIÇÃO DO PAGAMENTO
            string description = !String.IsNullOrEmpty(this.description) ? this.getValue(ID_MERCHANT_ACCOUNT_INFORMATION_DESCRIPTION, this.description) : "";

            //URL DO QR CODE DINÂMICO
            string url = !String.IsNullOrEmpty(this.url) ? this.getValue(ID_MERCHANT_ACCOUNT_INFORMATION_URL, Regex.Replace(this.url, @"^(http|https)://", "")) : "";

            //VALOR COMPLETO DA CONTA
            return this.getValue(ID_MERCHANT_ACCOUNT_INFORMATION, String.Format("{0}{1}{2}{3}", gui, key, description, url));
        }

        /**
         * Método responsável por retornar os valores completos do campo adicional do pix (TXID)
         * @return string
         */
        private string getAdditionalDataFieldTemplate()
        {
            //TXID
            txid = this.getValue(ID_ADDITIONAL_DATA_FIELD_TEMPLATE_TXID, this.txid);

            //RETORNA O VALOR COMPLETO
            return this.getValue(ID_ADDITIONAL_DATA_FIELD_TEMPLATE, txid);
        }

        /**
         * Método responsável por retornar o valor do ID_POINT_OF_INITIATION_METHOD
         * @return string
         */
        private string getUniquePayment()
        {
            return this.uniquePayment ? this.getValue(ID_POINT_OF_INITIATION_METHOD, "12") : "";
        }

        /**
         * Método responsável por gerar o código completo do payload Pix
         * @return string
         */
        public string getPayload()
        {
            //CRIA O PAYLOAD
            string payload = this.getValue(ID_PAYLOAD_FORMAT_INDICATOR, "01") +
                     this.getUniquePayment() +
                     this.getMerchantAccountInformation() +
                     this.getValue(ID_MERCHANT_CATEGORY_CODE, "0000") +
                     this.getValue(ID_TRANSACTION_CURRENCY, "986") +
                     this.getValue(ID_TRANSACTION_AMOUNT, this.amount) +
                     this.getValue(ID_COUNTRY_CODE, "BR") +
                     this.getValue(ID_MERCHANT_NAME, this.merchantName) +
                     this.getValue(ID_MERCHANT_CITY, this.merchantCity) +
                     this.getAdditionalDataFieldTemplate();

            //RETORNA O PAYLOAD + CRC16
            return this.getCRC16(payload);
        }

        /**
         * Método responsável por calcular o valor da hash de validação do código pix
         * @return string
         */
        private string getCRC16(string payload)
        {
            //ADICIONA DADOS GERAIS NO PAYLOAD
            payload += ID_CRC16 + "04";
            string resultado = CalcCRC16(payload);
            //RETORNA CÓDIGO CRC16 DE 4 CARACTERES
            return String.Format("{0}{1}{2}", ID_CRC16, "04", resultado.ToUpper());
        }

        /**
         * Método responsável por calcular o valor da hash de validação do código pix
         * @return string
         */
        private string CalcCRC16(string payload)
        {
            payload += ID_CRC16 + "04";

            ushort polinomio = 0x1021;
            ushort resultado = 0xFFFF;

            byte[] data = GetBytesFromHexString(payload);
            for (int i = 0; i < data.Length; i++)
            {
                resultado ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ((resultado & 0x10000) > 0) resultado ^= polinomio;
                    resultado &= 0xFFFF;
                }
            }
            return resultado.ToString("X");
        }

        public Byte[] GetBytesFromHexString(string strInput)
        {
            Byte[] bytArOutput = new Byte[] { };
            if (!string.IsNullOrEmpty(strInput) && strInput.Length % 2 == 0)
            {
                SoapHexBinary hexBinary = null;
                try
                {
                    hexBinary = SoapHexBinary.Parse(strInput);
                    if (hexBinary != null)
                    {
                        bytArOutput = hexBinary.Value;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return bytArOutput;
        }
    }
}
