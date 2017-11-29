using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace FitchCoinEngine.Blockchain
{
    [Serializable]
    public class Transaction : ISerializable
    {
        public DateTime Timestamp { get; set; }
        public double Amount { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Hash { get; set; }
        public string Signature { get; set; }

        public Transaction(string source, string destination, double amount, string signature = null)
        {
            Timestamp = DateTime.Now;
            Amount = amount;
            Source = source;
            Destination = destination;
            Hash = null;
            Signature = signature;
            if (signature != null)
                this.Hash = CalculateTransactionHash();
        }

        /// <summary>
        /// Calculates sha-256 hash of transaction(source, destination, amount, timestamp, signature)
        /// return: sha - 256 hash
        /// </summary>
        /// <returns>The trx hash.</returns>
        public string CalculateTransactionHash()
        {
            SHA256 sha256 = SHA256.Create();
            string data = this.ToString();
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] hashA = sha256.ComputeHash(byteConverter.GetBytes(data));
            return hashA.ToString();
        }

        /// <summary>
        /// TODO: Do we need this?
        /// </summary>
        /// <param name="info">Info.</param>
        /// <param name="context">Context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Needs verification code
        /// </summary>
        /// <returns>The verify.</returns>
        public bool Verify()
        {
            //return coincurve.PublicKey(self._source).verify(self._signature, self.to_signable())
            return false;
        }

        /// <summary>
        /// TODO: needs implementation
        /// </summary>
        /// <returns>The sign.</returns>
        /// <param name="privateKey">Private key.</param>
        public string Sign(string privateKey)
        {
            string signature = null;//coincurve.PrivateKey.from_hex(private_key).sign(self.to_signable()).encode('hex');
            this.Signature = signature;
            this.Hash = CalculateTransactionHash();
            return signature;
        }


        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}:{3}:{4}",
                                         this.Timestamp,
                                         this.Amount,
                                         this.Source,
                                         this.Destination,
                                         this.Signature
                                        );
        }
    }
}