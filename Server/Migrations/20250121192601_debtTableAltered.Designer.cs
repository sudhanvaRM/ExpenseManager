﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Server.Models.Entities;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250121192601_debtTableAltered")]
    partial class debtTableAltered
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Server.Models.Entities.Debt", b =>
                {
                    b.Property<Guid>("CreditorId")
                        .HasColumnType("uuid")
                        .HasColumnOrder(2);

                    b.Property<Guid>("DebtorId")
                        .HasColumnType("uuid")
                        .HasColumnOrder(1);

                    b.Property<Guid>("TripId")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.HasKey("CreditorId", "DebtorId", "TripId");

                    b.HasIndex("DebtorId");

                    b.HasIndex("TripId");

                    b.ToTable("Debt", (string)null);
                });

            modelBuilder.Entity("Server.Models.Entities.Expense", b =>
                {
                    b.Property<Guid>("ExpenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("expense_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("amount");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("category");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<Guid>("PaidUser")
                        .HasColumnType("uuid")
                        .HasColumnName("paid_user");

                    b.Property<Guid?>("TripId")
                        .HasColumnType("uuid")
                        .HasColumnName("trip_id");

                    b.HasKey("ExpenseId");

                    b.HasIndex("PaidUser");

                    b.HasIndex("TripId");

                    b.ToTable("expense", (string)null);
                });

            modelBuilder.Entity("Server.Models.Entities.Trip", b =>
                {
                    b.Property<Guid>("TripId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("trip_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("TripDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("trip_date");

                    b.Property<string>("TripName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("trip_name");

                    b.HasKey("TripId");

                    b.ToTable("trip", (string)null);
                });

            modelBuilder.Entity("Server.Models.Entities.Trip_Participants", b =>
                {
                    b.Property<Guid>("TripId")
                        .HasColumnType("uuid")
                        .HasColumnName("trip_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("TripId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("trip_participants", (string)null);
                });

            modelBuilder.Entity("Server.Models.Entities.Users", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("username");

                    b.HasKey("UserId");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Server.Models.Entities.Debt", b =>
                {
                    b.HasOne("Server.Models.Entities.Users", "Creditor")
                        .WithMany("DebtsAsCreditor")
                        .HasForeignKey("CreditorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.Entities.Users", "Debtor")
                        .WithMany("DebtsAsDebtor")
                        .HasForeignKey("DebtorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.Entities.Trip", "Trip")
                        .WithMany("Debts")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creditor");

                    b.Navigation("Debtor");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Server.Models.Entities.Expense", b =>
                {
                    b.HasOne("Server.Models.Entities.Users", "PaidUserNavigation")
                        .WithMany("Expenses")
                        .HasForeignKey("PaidUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.Entities.Trip", "Trip")
                        .WithMany("Expenses")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("PaidUserNavigation");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Server.Models.Entities.Trip_Participants", b =>
                {
                    b.HasOne("Server.Models.Entities.Trip", "Trip")
                        .WithMany("TripParticipants")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.Entities.Users", "User")
                        .WithMany("TripParticipants")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.Entities.Trip", b =>
                {
                    b.Navigation("Debts");

                    b.Navigation("Expenses");

                    b.Navigation("TripParticipants");
                });

            modelBuilder.Entity("Server.Models.Entities.Users", b =>
                {
                    b.Navigation("DebtsAsCreditor");

                    b.Navigation("DebtsAsDebtor");

                    b.Navigation("Expenses");

                    b.Navigation("TripParticipants");
                });
#pragma warning restore 612, 618
        }
    }
}
