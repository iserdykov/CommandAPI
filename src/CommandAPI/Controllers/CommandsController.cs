using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo _repository;
        public CommandsController(ICommandAPIRepo repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands()
        {
           var commandItems = _repository.GetAllCommands();

            return Ok(commandItems);
            
        }
    
        // public ActionResult<IEnumerable<string>> Get(){
        //     return new string[] {"This","is","hard" , "code"};
    
        // }
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Command>> GetCommandByID(int id)
        {
            var commandItemsId = _repository.GetCommandByID(id);

            if (id == null)
            {
                return NotFound();
            }
                return Ok(commandItemsId);
            
        }

    }
}