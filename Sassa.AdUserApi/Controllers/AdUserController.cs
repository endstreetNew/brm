using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Sassa.BRM.Models;

namespace Sassa.AdUserApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdUserController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AdUserController> _logger;

        public AdUserController(ILogger<AdUserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<AdUser> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new AdUser
            {

            })
            .ToArray();
        }
    }
}
