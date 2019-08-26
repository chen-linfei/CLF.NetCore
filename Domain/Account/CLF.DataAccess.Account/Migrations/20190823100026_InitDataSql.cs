using System;
using CLF.DataAccess.Account.DataInitial;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace CLF.DataAccess.Account.Migrations
{
    public partial class InitDataSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            DBSqlInitializer.Sql();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
