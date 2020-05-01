using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.API.Controllers
{
    public class Value
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // Just for testing front-end
        private IEnumerable<Value> values = new List<Value> {
            new Value{ Id = 1, Name = "Value 101"},
            new Value{ Id = 2, Name = "Value 102"},
            new Value{ Id = 3, Name = "Value 103"}
        };

        // GET api/values
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetValue()
        {
            return Ok(values.ToList());
        }

        // GET api/values/5
        [Authorize(Roles = "Member")]
        [HttpGet("{id}")]
        public IActionResult GetValue(int id)
        {
            var value = values.FirstOrDefault(x => x.Id == id);
            
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}