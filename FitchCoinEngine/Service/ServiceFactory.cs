using System;
namespace FitchCoinEngine.Service
{
    /// <summary>
    /// TODO: make this a singleton
    /// </summary>
    public class ServiceFactory
    {
        public static INodeService GetNodeService() => new NodeService();
        public static IBlockchainService GetBlockchainService() => new BlockchainService();
    }
}
