﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pokemons.DataLayer.Database;

#nullable disable

namespace Pokemons.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240609120526_BattleAdded")]
    partial class BattleAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Battle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("BattleEndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("BattleStartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("BattleState")
                        .HasColumnType("integer");

                    b.Property<int>("EntityType")
                        .HasColumnType("integer");

                    b.Property<long>("Health")
                        .HasColumnType("bigint");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<long>("RemainingHealth")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Battles");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.Property<int>("DamagePerClick")
                        .HasColumnType("integer");

                    b.Property<int>("DefeatedEntities")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Battle", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithMany("Battles")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Player", b =>
                {
                    b.Navigation("Battles");
                });
#pragma warning restore 612, 618
        }
    }
}
