﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using orderManageApp.DB;

namespace orderManageApp.Migrations
{
    [DbContext(typeof(orderAppContext))]
    [Migration("20211016154219_update1")]
    partial class update1
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

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CustomName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomPatronymic")
                        .HasColumnType("text");

                    b.Property<string>("CustomSurname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Removed")
                        .HasColumnType("boolean");

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

                    b.Property<long>("OrderID")
                        .HasColumnType("bigint");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("ProductPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("ProductSku")
                        .HasColumnType("integer");

                    b.Property<bool>("Removed")
                        .HasColumnType("boolean");

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
