using System;
using FitchCoinEngine.Blockchain;
using FitchCoinEngine.Service;
using Microsoft.AspNetCore.Mvc;

namespace FitchCoin.Controllers
{
    [Route("api/[controller]")]
    public class FitchCoinController : Controller
    {
        private INodeService m_nodeService;

        public FitchCoinController()
        {
            m_nodeService = ServiceFactory.GetNodeService();
        }

        /// <summary>
        /// Get All Nodes
        /// </summary>
        /// <returns>The nodes.</returns>
        [HttpGet("Nodes")]
        public JsonResult Nodes()
        {
            return Json(m_nodeService.GetNodes());
        }

        /// <summary>
        /// Add a new node
        /// </summary>
        /// <returns>Add node.</returns>
        /// <param name="node">Node.</param>
        [HttpPost("Nodes")]
        public JsonResult Nodes([FromBody] Node node)
        {
            return Json(m_nodeService.AddNode(node));
        }

        /// <summary>
        /// Get All Unconfirmed Transactions
        /// </summary>
        /// <returns>The transactions.</returns>
        [HttpGet("Transactions")]
        public JsonResult Transactions()
        {
            return Json(m_nodeService.GetUnconfirmedTransactions());
        }

        /// <summary>
        /// Adds unconfirmed transaction
        /// </summary>
        /// <returns>true/false</returns>
        /// <param name="trx">Transaction.</param>
        [HttpPost("Transactions")]
        public JsonResult Transactions([FromBody] Transaction trx)
        {
            return Json(m_nodeService.AddTransaction(trx));
        }

        /// <summary>
        /// Returns balance based on the address
        /// </summary>
        /// <returns>The balance.</returns>
        /// <param name="address">Address.</param>
        [HttpGet("Address/{address}")]
        public JsonResult Balance(string address)
        {
            return Json(m_nodeService.GetBalance(address));
        }

        /// <summary>
        /// Returns transaction history based on the address
        /// </summary>
        /// <returns>The transaction history.</returns>
        /// <param name="address">Address.</param>
        [HttpGet("Address/{address}/Transactions")]
        public JsonResult TransactionHistory(string address)
        {
            return Json(m_nodeService.GetTransactionHistory(address));
        }

        /// <summary>
        /// Returns all blocks on blockchain
        /// </summary>
        /// <returns>The blocks.</returns>
        [HttpGet("Blocks")]
        public JsonResult Blocks()
        {
            return Json(m_nodeService.GetAllBlocks());
        }

        /// <summary>
        /// Blocks by range.
        /// </summary>
        /// <returns>The range.</returns>
        /// <param name="start">Start block identifier.</param>
        /// <param name="end">End block identifier.</param>
        [HttpGet("Blocks/{start}/{end}")]
        public JsonResult BlocksRange(string start, string end)
        {
            return Json(m_nodeService.GetBlocksRange(start, end));
        }

        /// <summary>
        /// Returns latest block
        /// </summary>
        /// <returns>The block.</returns>
        [HttpGet("LatestBlock")]
        public JsonResult LatestBlock()
        {
            return Json(m_nodeService.GetLatestBlock());
        }

        /// <summary>
        /// Block the specified id.
        /// </summary>
        /// <returns>The block.</returns>
        /// <param name="id">Block Identifier.</param>
        [HttpGet("Block/{id}")]
        public JsonResult Block(string id)
        {
            return Json(m_nodeService.GetBlockById(id));
        }

        /// <summary>
        /// Returns all blocks on blockchain
        /// </summary>
        /// <returns>The blocks.</returns>
        [HttpPost("Blocks")]
        public JsonResult Blocks([FromBody] RemoteBlock block)
        {
            return Json(m_nodeService.PostAndValidateBlock(block));
        }
    }
}


    