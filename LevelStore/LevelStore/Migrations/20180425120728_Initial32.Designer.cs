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
    [Migration("20180425120728_Initial32")]
    partial class Initial32
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LevelStore.Models.CartLine", b =>
                {
                    b.Property<int>("CartLineID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("FakeShare");

                    b.Property<int>("Furniture");

                    b.Property<double?>("KoefPriceAfterCheckout");

                    b.Property<int?>("OrderID");

                    b.Property<decimal>("PriceAfterCheckout");

                    b.Property<int?>("ProductID");

                    b.Property<int>("Quantity");

                    b.Property<int>("SelectedColor");

                    b.HasKey("CartLineID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductID");

                    b.ToTable("CartLines");
                });

            modelBuilder.Entity("LevelStore.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryName");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

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

                    b.Property<string>("Alternative");

                    b.Property<bool>("FirstOnScreen");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductID");

                    b.Property<bool>("SecondOnScreen");

                    b.Property<int?>("TypeColorID");

                    b.HasKey("ImageID");

                    b.HasIndex("ProductID");

                    b.HasIndex("TypeColorID");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("LevelStore.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("Comment");

                    b.Property<DateTime?>("DateOfCreation")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliveryWay");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("NovaPoshta");

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.HasKey("OrderID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("LevelStore.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddToCartCounter");

                    b.Property<int>("BuyingCounter");

                    b.Property<DateTime?>("DateOfCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description");

                    b.Property<bool>("HideFromUsers");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("NewProduct");

                    b.Property<decimal>("Price");

                    b.Property<int?>("ShareID");

                    b.Property<string>("Size");

                    b.Property<int?>("SubCategoryID")
                        .IsRequired();

                    b.Property<int>("ViewsCounter");

                    b.HasKey("ProductID");

                    b.HasIndex("ShareID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("LevelStore.Models.Promo", b =>
                {
                    b.Property<int>("PromoId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Discount");

                    b.Property<string>("PromoCode")
                        .IsRequired();

                    b.HasKey("PromoId");

                    b.ToTable("PromoCodes");
                });

            modelBuilder.Entity("LevelStore.Models.Share", b =>
                {
                    b.Property<int>("ShareId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOfEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfStart")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enabled");

                    b.Property<bool>("Fake");

                    b.Property<double>("KoefPrice");

                    b.HasKey("ShareId");

                    b.ToTable("Shares");
                });

            modelBuilder.Entity("LevelStore.Models.SubCategory", b =>
                {
                    b.Property<int>("SubCategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("SubCategoryName");

                    b.HasKey("SubCategoryID");

                    b.HasIndex("CategoryID");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("LevelStore.Models.TypeColor", b =>
                {
                    b.Property<int>("TypeColorID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ColorType");

                    b.HasKey("TypeColorID");

                    b.ToTable("TypeColors");
                });

            modelBuilder.Entity("LevelStore.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.Property<bool>("RememberMe");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LevelStore.Models.CartLine", b =>
                {
                    b.HasOne("LevelStore.Models.Order")
                        .WithMany("Lines")
                        .HasForeignKey("OrderID");

                    b.HasOne("LevelStore.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");
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

                    b.HasOne("LevelStore.Models.TypeColor")
                        .WithMany("Images")
                        .HasForeignKey("TypeColorID");
                });

            modelBuilder.Entity("LevelStore.Models.Product", b =>
                {
                    b.HasOne("LevelStore.Models.Share")
                        .WithMany("Products")
                        .HasForeignKey("ShareID");
                });

            modelBuilder.Entity("LevelStore.Models.SubCategory", b =>
                {
                    b.HasOne("LevelStore.Models.Category")
                        .WithMany("SubCategories")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
