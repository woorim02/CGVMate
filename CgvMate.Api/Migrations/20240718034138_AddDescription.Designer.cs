﻿// <auto-generated />
using System;
using CgvMate.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CgvMate.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240718034138_AddDescription")]
    partial class AddDescription
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("CgvMate.Api.Entities.BannedIP", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descritpion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("BannedIPs");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.Board", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ParentCommentId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("WriterIP")
                        .HasColumnType("longtext");

                    b.Property<string>("WriterName")
                        .HasColumnType("longtext");

                    b.Property<string>("WriterPasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("WriterPasswordSalt")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BoardId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Downvote")
                        .HasColumnType("int");

                    b.Property<int>("No")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Upvote")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.Property<string>("WriterIP")
                        .HasColumnType("longtext");

                    b.Property<string>("WriterName")
                        .HasColumnType("longtext");

                    b.Property<string>("WriterPasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("WriterPasswordSalt")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("BoardId", "No")
                        .IsUnique();

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CgvMate.Data.Entities.Cgv.CuponEvent", b =>
                {
                    b.Property<string>("EventId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ImageSource")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("EventId");

                    b.ToTable("CgvCuponEvents");
                });

            modelBuilder.Entity("CgvMate.Data.Entities.Cgv.GiveawayEvent", b =>
                {
                    b.Property<string>("EventIndex")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DDay")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("EventIndex");

                    b.ToTable("CgvGiveawayEvents");
                });

            modelBuilder.Entity("CgvMate.Data.Entities.LotteCinema.Event", b =>
                {
                    b.Property<string>("EventID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CinemaAreaCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CinemaAreaName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CinemaID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CinemaName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CloseNearYN")
                        .HasColumnType("int");

                    b.Property<int>("DevTemplateYN")
                        .HasColumnType("int");

                    b.Property<string>("EventClassificationCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EventCntnt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("EventSeq")
                        .HasColumnType("int");

                    b.Property<string>("EventTypeCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("EventWinnerYN")
                        .HasColumnType("int");

                    b.Property<int>("ImageDivisionCode")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProgressEndDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProgressStartDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RemainsDayCount")
                        .HasColumnType("int");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("EventID");

                    b.ToTable("LotteEvents");
                });

            modelBuilder.Entity("CgvMate.Data.Entities.LotteCinema.GiveawayEventKeyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Keyword")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Keyword")
                        .IsUnique();

                    b.ToTable("LotteGiveawayEventKeywords");
                });

            modelBuilder.Entity("CgvMate.Data.Entities.LotteCinema.LotteGiveawayEventModel", b =>
                {
                    b.Property<string>("EventID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FrGiftID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FrGiftNm")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("HasNext")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("EventID");

                    b.ToTable("LotteGiveawayEventModels");
                });

            modelBuilder.Entity("CgvMate.Data.Entities.Megabox.CuponEvent", b =>
                {
                    b.Property<string>("EventNo")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EventNo");

                    b.ToTable("MegaboxCuponEvents");
                });

            modelBuilder.Entity("CgvMate.Data.Entities.Megabox.GiveawayEvent", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("MegaboxGiveawayEvents");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.Comment", b =>
                {
                    b.HasOne("CgvMate.Api.Entities.Comment", "ParentComment")
                        .WithMany("Children")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CgvMate.Api.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CgvMate.Api.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentComment");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.Post", b =>
                {
                    b.HasOne("CgvMate.Api.Entities.Board", "Board")
                        .WithMany("Posts")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CgvMate.Api.Entities.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.Board", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.Comment", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("CgvMate.Api.Entities.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}