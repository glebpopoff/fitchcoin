using System;
using System.Collections.Generic;
using System.Linq;
using FitchCoinEngine.Util;

namespace FitchCoinEngine.Blockchain
{
    public class FitchCoinBlockchain
    {
        private IList<Transaction> m_unconfirmedTransactions = null;
        private IList<Block> m_blocks = null;

        private static readonly Object blockLock = new Object();
        private static readonly Object unconfirmedTransactionsLock = new Object();

        public FitchCoinBlockchain()
        {
            if (m_blocks == null)
            {
                var genesisBlock = this.GetGenesisBlock();
                this.AddBlock(genesisBlock);
            }
            else
            {
                foreach (var b in m_blocks)
                    this.AddBlock(b);
            }
        }

        /// <summary>
        /// There is one special block, also known as the “genesis block”. 
        /// This is the first block in the chain and it is hardcoded into the codebase.
        /// TODO: gene
        /// </summary>
        /// <returns>The genesis block.</returns>
        private Block GetGenesisBlock()
        {
            var genesisTransactionOne = new Transaction(BlockchainConstants.GENESIS_TRX_SOURCE,
                                                        BlockchainConstants.GENESIS_TRX_DESTINATION,
                                                        BlockchainConstants.GENESIS_TRX_AMOUNT
                                                       );
            var genesisTrx = new List<Transaction> { genesisTransactionOne };
            var genesisBlock = new Block(0, genesisTrx);
            return genesisBlock;
        }

        /// <summary>
        /// TODO change this from memory to persistent
        /// </summary>
        private bool AddBlock(Block b)
        {
            lock (blockLock)
            {
                if (this.ValidateBlock(b))
                {
                    if (this.m_blocks == null)
                        this.m_blocks = new List<Block>();
                
                    this.m_blocks.Add(b);
                    return true;
                }
                return false;

            }
        }

        /// <summary>
        /// Verify genesis block integrity
        /// TODO implement and use Merkle tree
        /// </summary>
        /// <returns><c>true</c>, if block was validated, <c>false</c> otherwise.</returns>
        /// <param name="b">block</param>
        private bool ValidateBlock(Block block)
        {

            try
            {
                //if genesis block, check if block is correct
                if (block.Index == 0)
                {
                    CheckGenesisBlock(block);
                    return true;
                }

                //current hash of data is correct and hash satisfies pattern
                this.CheckHashAndHashPattern(block);

                //block index is correct and previous hash is correct
                this.CheckIndexAndPreviousHash(block);

                //block reward is correct based on block index and halving formula
                this.CheckTransactionsAndBlockReward(block);

            } catch (Exception ex)
            {
                //logger.warning("Validation Error (block id: %s): %s", bce.index, bce.message)
            }
            return false;
        }

        /// <summary>
        /// Checks the transactions and block reward.
        /// </summary>
        /// <param name="block">Block.</param>
        private void CheckTransactionsAndBlockReward(Block block)
        {

            var payers = new Dictionary<string, double>();
            foreach (var transaction in block.Transactions.Take(block.Transactions.Count() - 1))
            {
                if (this.FindDuplicateTransactions(transaction.Hash))
                    throw new Exception(string.Format("Transactions not valid.  Duplicate transaction detected. Block Index={0}", block.Index));

                if (!transaction.Verify())
                    throw new Exception(string.Format("Transactions not valid.  Invalid Transaction signature. Block Index={0}", block.Index));

                if (payers.ContainsKey(transaction.Source))
                    payers[transaction.Source] += transaction.Amount;
                else
                    payers[transaction.Source] = transaction.Amount;
            }

            //insufficient fund check
            foreach (var key in payers.Keys)
            {
                var balance = this.GetBalance(key);
                if (payers[key] > balance)
                    throw new Exception(string.Format("Transactions not valid.  Insufficient funds. Block Index={0}", block.Index));
            }

            //last transaction is block reward
            var rewardTransaction = block.Transactions.Last();
            var rewardAmount = this.GetReward(block.Index);
            if (rewardTransaction.Amount != rewardAmount || rewardTransaction.Source != BlockchainConstants.GENESIS_TRX_SOURCE)
                throw new Exception(string.Format("Transactions not valid. Incorrect block reward. Block Index={0}", block.Index));
                                             
        }

        private bool FindDuplicateTransactions(string hash)
        {
            foreach (var block in this.m_blocks)
            {
                foreach (var transaction in block.Transactions)
                {
                    if (transaction.Hash == hash)
                        return true;
                }
            }
            return false;
        }

        private double GetReward(int index)
        {
            double precision = Math.Pow(10, BlockchainConstants.SIGNIFICANT_DIGITS);
            double reward = BlockchainConstants.INITIAL_COINS_PER_BLOCK;
            foreach (int i in Enumerable.Range(1, ((index / BlockchainConstants.HALVING_FREQUENCY) + 1)))
                reward = Math.Floor((reward / 2.0) * precision) / precision;
            return reward;
        }

        private double GetBalance(string address)
        {
            double balance = 0;
            foreach (var block in this.m_blocks)
            {
                foreach (var transaction in block.Transactions)
                {
                    if (transaction.Source == address)
                        balance -= transaction.Amount;
                    if (transaction.Destination == address)
                        balance += transaction.Amount;
                }  
            }
            return balance;
        }

        private void CheckIndexAndPreviousHash(Block block)
        {
            var latestBlock = this.GetLatestBlock();
            if (latestBlock.Index != block.Index - 1)
                throw new Exception(string.Format("Incompatible block index: {0}", (block.Index - 1)));

            if (latestBlock.CurrentHash != block.PreviousHash)
                throw new Exception(string.Format("Incompatible block hash: {0} and hash: {1}", (block.Index - 1), block.PreviousHash));
        }

        /// <summary>
        /// TODO: not sure about this implementation
        /// </summary>
        /// <param name="block">Block.</param>
        private void CheckHashAndHashPattern(Block block)
        {
            int hashDifficulty = this.CalculateHashDifficulty();
            if (block.CurrentHash.Substring(hashDifficulty).IndexOf('0') != hashDifficulty)
                throw new Exception(string.Format("Incompatible Block Hash: {0}", block.CurrentHash));
        }

        /// <summary>
        /// TODO: calculate the delta
        /// </summary>
        /// <returns>The hash difficulty.</returns>
        /// <param name="index">Block index</param>
        private int CalculateHashDifficulty(int? index = null)
        {
            Block block = null;
            if (index == null)
                block = this.GetLatestBlock();
            else
                block = this.GetBlockByIndex(index.Value);

            if (block.Index > BlockchainConstants.DIFFICULTY_ADJUSTMENT_SPAN)
            {
                var blockDelta = (index != null) ? this.GetBlockByIndex(index.Value - BlockchainConstants.DIFFICULTY_ADJUSTMENT_SPAN) : this.GetLatestBlock();
                var timestampDelta = block.Timestamp - blockDelta.Timestamp;
                //blocks were mined quicker than target
                if (timestampDelta < BlockchainConstants.TARGET_TIME_PER_BLOCK - (BlockchainConstants.TARGET_TIME_PER_BLOCK / 10))
                    return block.HashDifficulty + 1;
                //blocks were mined slower than target
                else if (timestampDelta > BlockchainConstants.TARGET_TIME_PER_BLOCK + (BlockchainConstants.TARGET_TIME_PER_BLOCK / 10))
                    return block.HashDifficulty - 1;
                //blocks were mined within the target time window
                return block.HashDifficulty;
            }
            //not enough blocks were mined for an adjustment
            return BlockchainConstants.MINIMUM_HASH_DIFFICULTY;
        }

        private Block GetLatestBlock()
        {
            if (m_blocks.Count > 0)
                return m_blocks[m_blocks.Count - 1];
            
            throw new Exception("No blocks found");
        }

        private Block GetBlockByIndex(int v)
        {
            if (m_blocks.Count > v && m_blocks[v] != null)
                return m_blocks[v];
            
            throw new Exception(string.Format("No blocks found at index {0}", v));
        }

        private void CheckGenesisBlock(Block block)
        {
            if (!block.Equals(this.GetGenesisBlock()))
                throw new Exception(string.Format("Genesis Block Mismatch: {0}", block.Index));
        }


    }
}
