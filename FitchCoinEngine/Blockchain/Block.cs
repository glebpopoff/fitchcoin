using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using FitchCoinEngine.Util;

namespace FitchCoinEngine.Blockchain
{
    public class Block
    {
        private int m_nonce;
        private IEnumerable<Transaction> m_transactions;

        public int Index { get; set; }
        public IEnumerable<Transaction> Transactions
        {
            get { return m_transactions; }
            set
            {
                m_transactions = value ?? new HashSet<Transaction>();
            }
        }
        public string PreviousHash { get; set; }
        public string CurrentHash { get; set; }
        public long Timestamp { get; set; }
        public int Nonce
        {
            get { return m_nonce; }
            set
            {
                m_nonce = value;
                this.CurrentHash = CalculateBlockHash();
            }
        }

        /// <summary>
        /// TODO: Find out a better way to calculcate has difficulty
        /// </summary>
        /// <value>The hash difficulty.</value>
        public int HashDifficulty
        {
            get {
                int difficulty = 0;
                foreach (char c in this.CurrentHash)
                {
                    if (c != '0')
                        break;
                    difficulty += 1;
                }
                return difficulty;
            }
        }

        /// <summary>
        ///index: index # of block
        ///transactions: list of transactions
        ///previous_hash: previous block hash
        ///current_hash: current block hash
        ///timestamp: timestamp of block mined
        //nonce: nonce
        /// </summary>
        public Block(int index, IEnumerable<Transaction> transactions, string previousHash = null, long? timestamp = null, int nonce= 0)
        {
            this.Index = index;
            this.Transactions = transactions;
            this.PreviousHash = previousHash;
            this.Nonce = nonce;
            this.Timestamp = timestamp ?? DateTime.Now.Ticks;
            this.CurrentHash = CalculateBlockHash();
        }

        private string CalculateBlockHash()
        {
            SHA256 sha256 = SHA256.Create();
            string data = this.ToString();
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] hashA = sha256.ComputeHash(byteConverter.GetBytes(data));
            return hashA.ToString();
        }

        public override string  ToString()
        {
            return string.Format("{0}:{1}:{2}:{3}:{4}",
                                         this.Index,
                                         this.PreviousHash,
                                         this.Timestamp,
                                         string.Join("-", (from p in this.Transactions select p.Hash).ToList()),
                                         this.Nonce
                                        );
        }

        public bool Equals(Block c)
        {
            return (this.Serialize().Equals(c.Serialize())) ? true : false;
        }
    }
}

