using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace SqlRestore
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
                return;
            string serverName = args[0];
            string databaseName = args[1];
            string restoreFilePath = args[2];

            Restore restoreDB = new Restore();
            restoreDB.Database = databaseName;
            restoreDB.Action = RestoreActionType.Database;
            restoreDB.Devices.AddDevice(@restoreFilePath, DeviceType.File);

            restoreDB.ReplaceDatabase = true;

            /* If you have a differential or log restore after the current restore,
             * you would need to specify NoRecovery = true, this will ensure no
             * recovery performed and subsequent restores are allowed. It means it
             * the database will be in a restoring state. */
            restoreDB.NoRecovery = false;

            restoreDB.PercentComplete += (obj, e) => { Console.WriteLine("Percent completed: {0}%.", e.Percent); };


            Server srv = new Server(@serverName);

            /* Check if database exists, if not, create one. */
            Database db = srv.Databases[databaseName];
            if (db == null)
            {
                db = new Database(srv,databaseName);
                Console.WriteLine("Creating database...");
                db.Create();
                Console.WriteLine("Created {0}.", databaseName);
            }
            Console.WriteLine("Verifying if backup media...");
            bool valid = restoreDB.SqlVerify(srv);
            if (valid)
            {
                Console.WriteLine("Backup media is valid.");
                Console.WriteLine("Starting restore...");
                restoreDB.SqlRestore(srv);
            }
            else
            {
                Console.WriteLine("Backup media is invalid. Aborting operation...");
            }
            
        }

  
    }
}
