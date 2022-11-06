﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using demo.Models;

#nullable disable

namespace demo.Models.Migrations
{
    [DbContext(typeof(DemoDbContext))]
    [Migration("20221106135000_AddDbSetGoogleLogin")]
    partial class AddDbSetGoogleLogin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Demo.Models.GoogleLogin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("google_login", (string)null);
                });

            modelBuilder.Entity("demo.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Birtday")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("Demo.Models.GoogleLogin", b =>
                {
                    b.HasOne("demo.Models.User", "User")
                        .WithOne("GoogleLogin")
                        .HasForeignKey("Demo.Models.GoogleLogin", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("demo.Models.User", b =>
                {
                    b.Navigation("GoogleLogin");
                });
#pragma warning restore 612, 618
        }
    }
}
