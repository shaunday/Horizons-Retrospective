﻿// <auto-generated />
using System;
using System.Collections.Generic;
using HsR.Journal.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HsR.Journal.Infrastructure.Migrations
{
    [DbContext(typeof(TradingJournalDataContext))]
    partial class TradingJournalDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HsR.Journal.Entities.DataElement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ComponentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CompositeFK")
                        .HasColumnType("integer");

                    b.Property<string>("CostRelevance")
                        .HasColumnType("text");

                    b.Property<bool>("IsRelevantForOverview")
                        .HasColumnType("boolean");

                    b.Property<string>("PriceRelevance")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TradeElementFK")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompositeFK");

                    b.HasIndex("TradeElementFK");

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("HsR.Journal.Entities.JournalData", b =>
                {
                    b.PrimitiveCollection<List<string>>("SavedBrokers")
                        .HasColumnType("jsonb");

                    b.PrimitiveCollection<List<string>>("SavedSectors")
                        .HasColumnType("jsonb");

                    b.ToTable("JournalData");
                });

            modelBuilder.Entity("HsR.Journal.Entities.TradeComposite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ClosedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("OpenedAt")
                        .HasColumnType("timestamp with time zone");

                    b.PrimitiveCollection<List<string>>("Sectors")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SummaryId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SummaryId");

                    b.ToTable("TradeComposites");
                });

            modelBuilder.Entity("HsR.Journal.Entities.TradeElement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CompositeFK")
                        .HasColumnType("integer");

                    b.Property<string>("TradeActionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompositeFK");

                    b.ToTable("TradeElements");
                });

            modelBuilder.Entity("HsR.Journal.Entities.DataElement", b =>
                {
                    b.HasOne("HsR.Journal.Entities.TradeComposite", "CompositeRef")
                        .WithMany()
                        .HasForeignKey("CompositeFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HsR.Journal.Entities.TradeElement", "TradeElementRef")
                        .WithMany("Entries")
                        .HasForeignKey("TradeElementFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("HsR.Journal.Entities.ContentRecord", "ContentWrapper", b1 =>
                        {
                            b1.Property<int>("DataElementFK")
                                .HasColumnType("integer");

                            b1.Property<string>("ChangeNote")
                                .HasColumnType("text");

                            b1.Property<string>("ContentValue")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("DataElementFK");

                            b1.ToTable("Entries");

                            b1.WithOwner()
                                .HasForeignKey("DataElementFK");
                        });

                    b.OwnsMany("HsR.Journal.Entities.ContentRecord", "History", b1 =>
                        {
                            b1.Property<int>("DataElementFK")
                                .HasColumnType("integer");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("ChangeNote")
                                .HasColumnType("text");

                            b1.Property<string>("ContentValue")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("DataElementFK", "Id");

                            b1.ToTable("Entries_History");

                            b1.WithOwner()
                                .HasForeignKey("DataElementFK");
                        });

                    b.Navigation("CompositeRef");

                    b.Navigation("ContentWrapper")
                        .IsRequired();

                    b.Navigation("History");

                    b.Navigation("TradeElementRef");
                });

            modelBuilder.Entity("HsR.Journal.Entities.TradeComposite", b =>
                {
                    b.HasOne("HsR.Journal.Entities.TradeElement", "Summary")
                        .WithMany()
                        .HasForeignKey("SummaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Summary");
                });

            modelBuilder.Entity("HsR.Journal.Entities.TradeElement", b =>
                {
                    b.HasOne("HsR.Journal.Entities.TradeComposite", "CompositeRef")
                        .WithMany("TradeElements")
                        .HasForeignKey("CompositeFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompositeRef");
                });

            modelBuilder.Entity("HsR.Journal.Entities.TradeComposite", b =>
                {
                    b.Navigation("TradeElements");
                });

            modelBuilder.Entity("HsR.Journal.Entities.TradeElement", b =>
                {
                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}