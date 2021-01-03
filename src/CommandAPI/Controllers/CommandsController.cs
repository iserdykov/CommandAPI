using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        public class CommandsController : ControllerBase
        {
            private readonly ICommandAPIRepo _repository;
            private readonly IMapper _mapper;
            public CommandsController(ICommandAPIRepo repository, IMapper mapper)
            {
            _repository = repository;
            _mapper = mapper;
            }
            [HttpGet]
            public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
            {
                var commandItems = _repository.GetAllCommands();
                return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
            }
                    // public ActionResult<IEnumerable<string>> Get(){
                //     return new string[] {"This","is","hard" , "code"};
    
                // }
            [HttpGet("{id}")]
            public ActionResult<CommandReadDto> GetCommandByID(int id)
            {
                var commandItemsId = _repository.GetCommandByID(id);
            // if (id == null)
            // {
            //     return NotFound();
            // }
                    return Ok(_mapper.Map<CommandReadDto>(commandItemsId));
            }
    }
}