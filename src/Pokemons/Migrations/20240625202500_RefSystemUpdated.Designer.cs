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
    [Migration("20240625202500_RefSystemUpdated")]
    partial class RefSystemUpdated
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

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Market", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("DamagePerClickCost")
                        .HasColumnType("integer");

                    b.Property<int>("DamagePerClickLevel")
                        .HasColumnType("integer");

                    b.Property<int>("EnergyChargeCost")
                        .HasColumnType("integer");

                    b.Property<int>("EnergyChargeLevel")
                        .HasColumnType("integer");

                    b.Property<int>("EnergyCost")
                        .HasColumnType("integer");

                    b.Property<int>("EnergyLevel")
                        .HasColumnType("integer");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<int>("SuperChargeCooldownCost")
                        .HasColumnType("integer");

                    b.Property<int>("SuperChargeCooldownLevel")
                        .HasColumnType("integer");

                    b.Property<int>("SuperChargeCost")
                        .HasColumnType("integer");

                    b.Property<int>("SuperChargeLevel")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId")
                        .IsUnique();

                    b.ToTable("Markets");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.Property<int>("CurrentEnergy")
                        .HasColumnType("integer");

                    b.Property<int>("DamagePerClick")
                        .HasColumnType("integer");

                    b.Property<int>("DefeatedEntities")
                        .HasColumnType("integer");

                    b.Property<int>("Energy")
                        .HasColumnType("integer");

                    b.Property<decimal>("EnergyCharge")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("LastCommitDamageTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("LastSuperChargeActivatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.Property<int>("SuperCharge")
                        .HasColumnType("integer");

                    b.Property<decimal>("SuperChargeCooldown")
                        .HasColumnType("numeric");

                    b.Property<string>("Surname")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Taps")
                        .HasColumnType("integer");

                    b.Property<int>("TotalDamage")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Rating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("GlobalRatingPosition")
                        .HasColumnType("bigint");

                    b.Property<long>("LeaguePosition")
                        .HasColumnType("bigint");

                    b.Property<int>("LeagueType")
                        .HasColumnType("integer");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId")
                        .IsUnique();

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.ReferralNode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Inline")
                        .HasColumnType("integer");

                    b.Property<long>("ReferralId")
                        .HasColumnType("bigint");

                    b.Property<long>("ReferrerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ReferralId");

                    b.HasIndex("ReferrerId");

                    b.ToTable("ReferralNodes");
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

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Market", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithOne("Market")
                        .HasForeignKey("Pokemons.DataLayer.Database.Models.Entities.Market", "PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Rating", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithOne("Rating")
                        .HasForeignKey("Pokemons.DataLayer.Database.Models.Entities.Rating", "PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.ReferralNode", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Referral")
                        .WithMany("ReferrerInfo")
                        .HasForeignKey("ReferralId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Referrer")
                        .WithMany("Referrals")
                        .HasForeignKey("ReferrerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Referral");

                    b.Navigation("Referrer");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Player", b =>
                {
                    b.Navigation("Battles");

                    b.Navigation("Market")
                        .IsRequired();

                    b.Navigation("Rating")
                        .IsRequired();

                    b.Navigation("Referrals");

                    b.Navigation("ReferrerInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
