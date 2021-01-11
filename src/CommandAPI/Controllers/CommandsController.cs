using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

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
        //==================================================
        //not async httpget
        //[HttpGet]
        //public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        //{
        //    var commandItems = _repository.GetAllCommands();
        //    return NoContent();
        //    //return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        //}
        //====================================================
        //====================================================
        //async httpget
        [HttpGet]
             public async Task<OkObjectResult> GetAllCommand()
             {
                 var commandItems = _repository.GetAllCommandsAsync();
                 return Ok(commandItems);
                 //return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));

             }
        //======================================================
        //
        // public ActionResult<IEnumerable<string>> Get(){
        //     return new string[] {"This","is","hard" , "code"};

        // }
        [HttpGet("{id}", Name = "GetCommandById")]
            public ActionResult<CommandReadDto> GetCommandByID(int id)
            {
                var commandItemsId = _repository.GetCommandByID(id);
            // if (id == null)
            // {
            //     return NotFound();
            // }
                    return Ok(_mapper.Map<CommandReadDto>(commandItemsId));
            }

            [HttpPost]
            public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
            {
                
                var commandModel = _mapper.Map<Command>(commandCreateDto);
                _repository.CreateCommand(commandModel);
                _repository.SaveChanges();

                var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
                return CreatedAtRoute(nameof(GetCommandByID), 
                    new {id = commandReadDto.Id}, commandReadDto);
            }

            [HttpPut("{id}")]
            public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
            {
                var commandModelFromRepo = _repository.GetCommandByID(id);
                if (commandModelFromRepo == null)
                {
                    return NotFound();
                }

                _mapper.Map(commandUpdateDto, commandModelFromRepo);
                _repository.UpdateCommand(commandModelFromRepo);
                _repository.SaveChanges();
                return NoContent();

            }

            [HttpPatch("{id}")]
            public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
            {
                var commandModelFromRepo = _repository.GetCommandByID(id);
                if (commandModelFromRepo == null)
                {
                    return NotFound();
                }

                var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
                patchDoc.ApplyTo(commandToPatch, ModelState);
                if (!TryValidateModel(commandToPatch))
                {
                    return ValidationProblem(ModelState);
                }

                _mapper.Map(commandToPatch, commandModelFromRepo);
                _repository.UpdateCommand(commandModelFromRepo);
                _repository.SaveChanges();

                return NoContent();
            }
            //update profiles map
            //install dependencies for patch
            // dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
            //dotnet add package Microsoft.AspNetCore.JsonPatch
            //update startap class using and add service, AddNew..=> { s.Ser ... Con= new
            //JsonPD<> pd
            //PartialCommandUpdate patchDoc
            //commandToPatch
            [HttpDelete("{id}")]
            public ActionResult CommandDelete(int id)
            {
                var commandFromRepo = _repository.GetCommandByID(id);
                if (commandFromRepo==null)
                {
                    return NotFound(nameof(commandFromRepo));
                }
                _repository.DeleteCommand(commandFromRepo);
                _repository.SaveChanges();
                return NoContent();
            }

        }
}