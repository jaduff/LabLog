﻿// <auto-generated />
using LabLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace LabLog.Migrations
{
    [DbContext(typeof(EventModelContext))]
    partial class EventModelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("LabLog.Domain.Events.LabEvent", b =>
                {
                    b.Property<Guid>("SchoolId");

                    b.Property<int>("Version");

                    b.Property<string>("EventAuthor");

                    b.Property<string>("EventBody");

                    b.Property<string>("EventType")
                        .IsRequired();

                    b.Property<DateTimeOffset>("Timestamp");

                    b.HasKey("SchoolId", "Version");

                    b.ToTable("LabEvents");
                });

            modelBuilder.Entity("LabLog.Models.ComputerModel", b =>
                {
                    b.Property<string>("SerialNumber")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ComputerNumber");

                    b.Property<Guid?>("RoomModelId");

                    b.Property<string>("School");

                    b.HasKey("SerialNumber");

                    b.HasIndex("RoomModelId");

                    b.ToTable("ComputerModel");
                });

            modelBuilder.Entity("LabLog.RoomModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("SchoolModelId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("SchoolModelId");

                    b.ToTable("RoomModel");
                });

            modelBuilder.Entity("LabLog.SchoolModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("LabLog.Models.ComputerModel", b =>
                {
                    b.HasOne("LabLog.RoomModel")
                        .WithMany("Computers")
                        .HasForeignKey("RoomModelId");
                });

            modelBuilder.Entity("LabLog.RoomModel", b =>
                {
                    b.HasOne("LabLog.SchoolModel")
                        .WithMany("Rooms")
                        .HasForeignKey("SchoolModelId");
                });
#pragma warning restore 612, 618
        }
    }
}
