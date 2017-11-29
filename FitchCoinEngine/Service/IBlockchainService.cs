using System;
using FitchCoinEngine.Blockchain;

namespace FitchCoinEngine.Service
{
    public interface IBlockchainService
    {
        FitchCoinBlockchain LoadBlockchain(string path);
    }
}
