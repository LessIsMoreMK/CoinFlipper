﻿// <auto-generated />
using System;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoinFlipper.Tracer.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231125211722_FearAndGreedIndex")]
    partial class FearAndGreedIndex
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CoinFlipper.Tracer.Infrastructure.Repositories.Models.FearAndGreedDb", b =>
                {
                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Classification")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Value")
                        .HasPrecision(0)
                        .HasColumnType("integer");

                    b.HasKey("DateTime");

                    b.HasIndex("DateTime");

                    b.ToTable("FearAndGreed");
                });
#pragma warning restore 612, 618
        }
    }
}
