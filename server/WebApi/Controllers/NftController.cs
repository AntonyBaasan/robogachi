﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moralis;
using Moralis.Models;
using Nethereum.Web3;
using Robotgotchi.Dto.Nft;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NftController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMoralisService moralisService;
        //private readonly string userAddress = "0x0e90709f60fdacea987fcbba0927e0da0be870d9";

        public NftController(IMoralisService moralisService, IMapper mapper)
        {
            this.moralisService = moralisService;
            this.mapper = mapper;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<NftModel>> Get()
        {
            var userAddress = User.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;
            if (userAddress == null) { return Array.Empty<NftModel>(); }

            var t = await GetNfts(userAddress);
            if (t == null) { return Array.Empty<NftModel>(); }

            //await TestWeb3();
            var models = t.result.Select(r => mapper.Map<NftModel>(r)).ToList();
            return models;
        }

        private async Task<MoralisNftResult> GetNfts(string userAddress)
        {
            return await moralisService.GetNftAsync(userAddress);
        }

        private static async Task<decimal> TestWeb3()
        {
            var web3 = new Web3("https://polygon-rpc.com");

            var transactionHash = "0xe7b4133f4f3160b5de56c3e777a7b2b38f2cfc15388de1e60945e50dd2b36335";

            var tx = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transactionHash);

            return 10;
        }
    }
}
