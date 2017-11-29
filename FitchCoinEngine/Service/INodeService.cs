using System;
using System.Collections.Generic;
using FitchCoinEngine.Blockchain;

namespace FitchCoinEngine.Service
{
    public interface INodeService
    {
        IEnumerable<Node> GetNodes();
        bool PostAndValidateBlock(RemoteBlock block);
        Block GetBlockById(string id);
        Block GetLatestBlock();
        IEnumerable<Block> GetBlocksRange(string start, string end);
        IEnumerable<Block> GetAllBlocks();
        IEnumerable<Transaction> GetTransactionHistory(string address);
        double GetBalance(string address);
        bool AddTransaction(Transaction trx);
        IEnumerable<Transaction> GetUnconfirmedTransactions();
        bool AddNode(Node node);
    }
}
