﻿// <auto-generated />
using LevelStore.Models.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace LevelStore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180119110303_initial3")]
    partial class initial3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LevelStore.Models.Color", b =>
                {
                    b.Property<int>("ColorID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductID");

                    b.Property<int>("TypeColorID");

                    b.HasKey("ColorID");

                    b.HasIndex("ProductID");

                    b.HasIndex("TypeColorID");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("LevelStore.Models.Image", b =>
                {
                    b.Property<int>("ImageID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductID");

                    b.HasKey("ImageID");

                    b.HasIndex("ProductID");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("LevelStore.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccessoriesForBags");

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("NewProduct");

                    b.Property<decimal>("Price");

                    b.Property<string>("Size");

                    b.HasKey("ProductID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("LevelStore.Models.TypeColor", b =>
                {
                    b.Property<int>("TypeColorID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ColorType");

                    b.HasKey("TypeColorID");

                    b.ToTable("TypeColors");
                });

            modelBuilder.Entity("LevelStore.Models.Color", b =>
                {
                    b.HasOne("LevelStore.Models.Product")
                        .WithMany("Color")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LevelStore.Models.TypeColor")
                        .WithMany("Color")
                        .HasForeignKey("TypeColorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LevelStore.Models.Image", b =>
                {
                    b.HasOne("LevelStore.Models.Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}