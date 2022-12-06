using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HsNsH.ApiWorks.DataApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    author_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    last_name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    first_name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    phone = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 12, nullable: false, defaultValueSql: "('UNKNOWN')"),
                    address = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    city = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    state = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 2, nullable: false),
                    zip = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 5, nullable: false),
                    email_address = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.author_id);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    job_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    job_desc = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('New Position - title not formalized yet')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.job_id);
                });

            migrationBuilder.CreateTable(
                name: "Publisher",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    publisher_name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 40, nullable: false),
                    city = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: false),
                    state = table.Column<string>(type: "TEXT", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    country = table.Column<string>(type: "TEXT", unicode: false, maxLength: 30, nullable: false, defaultValueSql: "('USA')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Publishe__2515F222DDC013AD", x => x.pub_id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    role_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    role_desc = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('New Position - title not formalized yet')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    store_id = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 4, nullable: false),
                    store_name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    store_address = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    city = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    state = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 2, nullable: false),
                    zip = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.store_id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "TEXT", unicode: false, maxLength: 80, nullable: false),
                    type = table.Column<string>(type: "TEXT", unicode: false, fixedLength: true, maxLength: 12, nullable: false, defaultValueSql: "('UNDECIDED')"),
                    pub_id = table.Column<int>(type: "INTEGER", nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: true),
                    advance = table.Column<decimal>(type: "money", nullable: true),
                    royalty = table.Column<int>(type: "INTEGER", nullable: true),
                    ytd_sales = table.Column<int>(type: "INTEGER", nullable: true),
                    notes = table.Column<string>(type: "TEXT", unicode: false, maxLength: 200, nullable: false),
                    published_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.book_id);
                    table.ForeignKey(
                        name: "FK__Book__pub_id__6166761E",
                        column: x => x.pub_id,
                        principalTable: "Publisher",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email_address = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: false),
                    source = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: false),
                    first_name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: false),
                    middle_name = table.Column<string>(type: "TEXT", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    last_name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 30, nullable: false),
                    role_id = table.Column<short>(type: "INTEGER", nullable: false),
                    pub_id = table.Column<int>(type: "INTEGER", nullable: false),
                    hire_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_id_2", x => x.user_id);
                    table.ForeignKey(
                        name: "FK__User__role_id__6E565CE8",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__User2__pub_id__60083D91",
                        column: x => x.pub_id,
                        principalTable: "Publisher",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthor",
                columns: table => new
                {
                    author_id = table.Column<int>(type: "INTEGER", nullable: false),
                    book_id = table.Column<int>(type: "INTEGER", nullable: false),
                    author_order = table.Column<byte>(type: "INTEGER", nullable: true),
                    royality_percentage = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthor", x => new { x.author_id, x.book_id });
                    table.ForeignKey(
                        name: "FK__BookAutho__autho__43D61337",
                        column: x => x.author_id,
                        principalTable: "Author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__BookAutho__book___42E1EEFE",
                        column: x => x.book_id,
                        principalTable: "Book",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    sale_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    store_id = table.Column<string>(type: "TEXT", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    order_num = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    quantity = table.Column<short>(type: "INTEGER", nullable: false),
                    pay_terms = table.Column<string>(type: "TEXT", unicode: false, maxLength: 12, nullable: false),
                    book_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.sale_id);
                    table.ForeignKey(
                        name: "FK__Sale2__book_id__756D6ECB",
                        column: x => x.book_id,
                        principalTable: "Book",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Sale2__store_id__76619304",
                        column: x => x.store_id,
                        principalTable: "Store",
                        principalColumn: "store_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    token_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    token = table.Column<string>(type: "TEXT", unicode: false, maxLength: 200, nullable: false),
                    expiry_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.token_id);
                    table.ForeignKey(
                        name: "FK__RefreshTo__user___60FC61CA",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_pub_id",
                table: "Book",
                column: "pub_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_book_id",
                table: "BookAuthor",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_user_id",
                table: "RefreshToken",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_book_id",
                table: "Sale",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_store_id",
                table: "Sale",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_pub_id",
                table: "User",
                column: "pub_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthor");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Publisher");
        }
    }
}
