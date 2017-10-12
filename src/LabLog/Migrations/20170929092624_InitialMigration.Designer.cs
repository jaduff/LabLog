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
    [Migration("20170929092624_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("LabLog.Domain.Events.LabEvent", b =>
                {
                    b.Property<Guid>("SchoolId");

                    b.Property<int>("Version");

                    b.Property<string>("EventBody");

                    b.Property<string>("EventType")
                        .IsRequired();

                    b.Property<DateTimeOffset>("Timestamp");

                    b.HasKey("SchoolId", "Version");

                    b.ToTable("LabEvents");
                });
#pragma warning restore 612, 618
        }
    }
}
