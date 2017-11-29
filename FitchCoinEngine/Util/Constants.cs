using System;
namespace FitchCoinEngine.Util
{
    public class BlockchainConstants
    {
        public const int INITIAL_COINS_PER_BLOCK = 50;
        public const int HALVING_FREQUENCY = 210000;
        public const int MAX_TRANSACTIONS_PER_BLOCK = 2000;
        public const int MINIMUM_HASH_DIFFICULTY = 5;
        public const int TARGET_TIME_PER_BLOCK = 600;
        public const int DIFFICULTY_ADJUSTMENT_SPAN = 100;
        public const int SIGNIFICANT_DIGITS = 8;

        public const string GENESIS_TRX_SOURCE = "0";
        public const string GENESIS_TRX_DESTINATION = "mPOHcCL1OZTbPUWUUGgC3tCY9K0ndAv6bfLkFMfnLGkXNvZ12efxXck6Y9uRjoTWCYd5";
        public const double GENESIS_TRX_AMOUNT = 1000;
    }

    public class NodeApiConstants
    {
        public const int FULL_NODE_PORT = 5001; //ASP.NET Core App URL  
        public const string NODES_URL = "http://{}:{}/nodes";
        public const string TRANSACTIONS_URL = "http://{}:{}/transactions";
        public const string BLOCK_URL = "http://{}:{}/block/{}";
        public const string BLOCKS_RANGE_URL = "http://{}:{}/blocks/{}/{}";
        public const string BLOCKS_URL = "http://{}:{}/blocks";
        public const string TRANSACTION_HISTORY_URL = "http://{}:{}/address/{}/transactions";
        public const string BALANCE_URL = "http://{}:{}/address/{}/balance";
    }
}
