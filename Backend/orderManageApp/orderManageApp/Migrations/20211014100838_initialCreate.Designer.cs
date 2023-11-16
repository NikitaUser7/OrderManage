﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using orderManageApp.DB;

namespace orderManageApp.Migrations
{
    [DbContext(typeof(orderAppContext))]
    [Migration("20211014100838_initialCreate")]
    partial class initialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("orderManageApp.DB.order", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CustomName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomPatronymic")
                        .HasColumnType("text");

                    b.Property<string>("CustomSurname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("boolean");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("SumMoney")
                        .HasColumnType("numeric");

                    b.HasKey("ID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("orderManageApp.DB.order_position", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CustomPatronymic")
                        .HasColumnType("text");

                    b.Property<string>("CustomSurname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("boolean");

                    b.Property<long>("OrderID")
                        .HasColumnType("bigint");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("ProductPrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("SumMoney")
                        .HasColumnType("numeric");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.ToTable("Order_positions");
                });

            modelBuilder.Entity("orderManageApp.DB.order_position", b =>
                {
                    b.HasOne("orderManageApp.DB.order", "Order")
                        .WithMany("Positions")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("orderManageApp.DB.order", b =>
                {
                    b.Navigation("Positions");
                });
#pragma warning restore 612, 618
        }
    }
}
