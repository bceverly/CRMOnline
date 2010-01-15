using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseMigration.Linq
{
    public class MigrationLevel
    {
        public int MigrationNumber { get; set; }
        public DateTime MigrationDate { get; set; }
    }
}
