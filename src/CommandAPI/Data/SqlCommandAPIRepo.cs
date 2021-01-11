using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandAPI.Data
{
    public class SqlCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandContext _context;

        public SqlCommandAPIRepo(CommandContext context)
        {
            _context = context;
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.CommandItems;
        }

        public async IAsyncEnumerable<Command> GetAllCommandsAsync()
        {
            var getAllAsync = await _context.CommandItems.ToListAsync();
            foreach (var command in getAllAsync) 
                yield return command;
        }

        public Command GetCommandByID(int id)
        {
            return _context.CommandItems.FirstOrDefault(items=>items.Id == id);
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.CommandItems.Add(cmd);
        }

        public void UpdateCommand(Command cmd)
        {
            //throw new System.NotImplementedException();
            //We don't need to do anything here
            _context.CommandItems.Update(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            _context.CommandItems.Remove(cmd);
        }
    }
}