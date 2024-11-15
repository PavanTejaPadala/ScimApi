using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScimApi.Models;
using ScimApi.Helpers;
using System.Linq;

namespace ScimApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SecretServerContext _dbContext;

        public UsersController(SecretServerContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetUsers([FromQuery] string filter)
        {
            var sqlQuery = ScimToSqlMapper.MapScimFilterToSql(filter);
            var users = _dbContext.TbUsers.FromSqlRaw(sqlQuery).ToList();
            return Ok(users);
        }
    }
}
