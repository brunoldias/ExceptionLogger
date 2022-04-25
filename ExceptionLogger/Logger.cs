using ExceptionLogger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger
{
    public class Logger
    {
        private static DbConnection _database;
        public virtual Logger WriteFromSQLServer(string connectionStrings)
        {
            if (_database != null)
                return this;

            if (string.IsNullOrWhiteSpace(connectionStrings)) throw new ArgumentNullException("Connection String not declared");

            _database = new DbConnection(connectionStrings);

            return this;
        }

        public async Task LoggerWrite(Exception exception)
        {
            if (_database != null)
                await SaveDatabase(exception);

            return;
        }

        private async Task SaveDatabase(Exception exception)
        {
            await _database.Execute(exception);
        }
    }
}

