using System;
using System.Collections.Generic;
using FitchCoinEngine.Blockchain;

namespace FitchCoinEngine.Service
{
    public class NodeService : INodeService
    {
        public bool AddNode(Node node)
        {
            /*
             * if host == self.host:
            return

        if host not in self.full_nodes:
            self.broadcast_node(host)
            self.full_nodes.add(host)
            */
            throw new NotImplementedException();
        }

        public bool AddTransaction(Transaction trx)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Block> GetAllBlocks()
        {
            throw new NotImplementedException();
        }

        public double GetBalance(string address)
        {
            throw new NotImplementedException();
        }

        public Block GetBlockById(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Block> GetBlocksRange(string start, string end)
        {
            throw new NotImplementedException();
        }

        public Block GetLatestBlock()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Node> GetNodes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> GetTransactionHistory(string address)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> GetUnconfirmedTransactions()
        {
            throw new NotImplementedException();
        }

        public bool PostAndValidateBlock(RemoteBlock block)
        {
            throw new NotImplementedException();
        }
    }
}
